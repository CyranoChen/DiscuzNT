using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using System.IO;
using NVelocity.Runtime;
using System.Text;
using Discuz.Common;
using Discuz.Config;


namespace Discuz.Forum
{

    public class NVelocity
    {
        public static VelocityEngine engine;
        public static ExtendedProperties props;
        public static string filePath = "";
        public static Template template;
        static NVelocity()
        {
            filePath = Utils.GetMapPath(BaseConfigs.GetForumPath);
            engine = new VelocityEngine();
            props = new ExtendedProperties();
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, filePath);
            engine.Init(props);
        }

        public static string GetTemplateData(string name)
        {
            template = engine.GetTemplate(name);
            return template.Data.ToString();
        }

        public static string TemplateMerge(string name, Hashtable table)
        {
            template = engine.GetTemplate(filePath+name);
            VelocityContext content = new VelocityContext();
            foreach (DictionaryEntry entry in table)
            {
                content.Put(entry.Key.ToString(), entry.Value);
            }
            StringWriter writer = new StringWriter();
            template.Merge(content, writer);
            return writer.GetStringBuilder().ToString();
        }
        public static string StringMerge(string templateContext, Hashtable table)
        {
            var engine = new VelocityEngine();
            engine.Init();
            var content = new VelocityContext();
            if (table != null)
                foreach (DictionaryEntry entry in table)
                {
                    content.Put(entry.Key.ToString(), entry.Value);
                }
            using (var writer = new StringWriter())
            {
                engine.Evaluate(content, writer, "", templateContext);
                return writer.GetStringBuilder().ToString();
            }
        }

        public static bool CreateTemplate(string path, string str)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath +path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    str = str.Replace("%include", "#include");
                    Byte[] info = Encoding.UTF8.GetBytes(str);
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
