using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Services.Manyou.Actions;
using Newtonsoft.Json;

namespace Discuz.Web.Services
{
    public class My : Page
    {
        public static RegexOptions options = RegexOptions.None;

        private static Regex regex1 = new Regex(@",""params"":({([\s\S]+?)})},{""module", options);//匹配除最后一个任务params的所有params项目

        private static Regex regex2 = new Regex(@",""params"":({([\s\S]+?)})}]$", options);//匹配最后一个任务params项目

        //public static string logtext = "";

        public My()
        {
            this.Load += new EventHandler(MYServer_Load);
        }

        void MYServer_Load(object sender, EventArgs e)
        {
            Response.Clear();

            string className = DNTRequest.GetString("module");
            string method = DNTRequest.GetString("method");
            string myParams = DNTRequest.GetString("params");
            string sign = DNTRequest.GetString("sign");


            if (!CheckSignature(sign, className, method, myParams))//校验请求签名合法性
                return;

            string result = "";
            if (className == "Batch" && method == "run")//如果是批量操作任务
            {
                List<string> paramsList = new List<string>();

                for (Match m1 = regex1.Match(myParams); m1.Success; m1 = m1.NextMatch())//从myParams找到params参数的值，并按顺序提取出来
                {
                    paramsList.Add(m1.Groups[1].Value);
                }
                string subParams = regex1.Replace(myParams, "},{\"module");//从原始params中移除除最后一个任务params的所有params

                paramsList.Add(regex2.Match(subParams).Groups[1].Value);

                TaskInfo[] taskList = JavaScriptConvert.DeserializeObject<TaskInfo[]>(regex2.Replace(subParams, "}]"));//移除最后一个任务params项目，并反序列化出任务列表，循环执行

                if (taskList.Length != paramsList.Count)//如果任务列表长度和提取出的任务params列表长度不一致，则结束此处批量任务
                    return;

                int i = 0;
                foreach (TaskInfo taskInfo in taskList)
                {
                    result += GetServiceExecutionResult(taskInfo.ClassName, taskInfo.Method, paramsList[i++]) + ",";//此处还未确定返回信息格式
                }
                result = "[" + result.TrimEnd(',') + "]";
            }
            else
            {
                result = GetServiceExecutionResult(className, method, myParams);
            }

            Response.Write(result);

        }

        private bool CheckSignature(string sign, string className, string method, string myParams)
        {
            string siteKey = GeneralConfigs.GetConfig().Mysitekey;
            return sign == Utils.MD5(string.Format("{0}|{1}|{2}|{3}", className, method, myParams, siteKey));
        }

        private string GetServiceExecutionResult(string className, string method, string myParams)
        {
            string data = "";
            ActionBase action;
            Type type = Type.GetType(string.Format("Discuz.Web.Services.Manyou.Actions.{0}, Discuz.Web.Services", className), false, true);
            action = (ActionBase)Activator.CreateInstance(type);
            action.Module = className;
            action.Method = method;
            action.JsonParams = myParams;
            data += type.InvokeMember(method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase, null, action, new object[] { }).ToString();
            return data;
        }

        public static void WriteLogFile(string logtext)
        {
            using (FileStream fs = new FileStream(Utils.GetMapPath(BaseConfigs.GetForumPath + "config/manyoulog/") + "manyouservicelogs-" + DateTime.Now.Ticks + ".txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Byte[] info = Encoding.Unicode.GetBytes(logtext);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }
    }
}
