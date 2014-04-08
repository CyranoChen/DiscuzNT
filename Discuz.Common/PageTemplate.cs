using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using Discuz.Common.Generic;

namespace Discuz.Common
{
	/// <summary>
	/// TemplateΪҳ��ģ����.
	/// </summary>
	public abstract class PageTemplate
	{
		public static Regex[] r = new Regex[25];

	    private Dictionary<string, string> headerTemplateCache = new Dictionary<string, string>();

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        public abstract string ReplaceSpecialTemplate(string forumPath, string skinName, string strTemplate);		

		static PageTemplate()
		{

			RegexOptions options = Utils.GetRegexCompiledOptions();

			r[0] = new Regex(@"<%template ([^\[\]\{\}\s]+)%>", options);
			r[1] = new Regex(@"<%loop ((\(([a-zA-Z]+)\) )?)([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", options);
			r[2] = new Regex(@"<%\/loop%>", options);
			r[3] = new Regex(@"<%while ([^\[\]\{\}\s]+)%>", options);
			r[4] = new Regex(@"<%\/while ([^\[\]\{\}\s]+)%>", options);
			r[5] = new Regex(@"<%if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?)(?:\s*)%>", options);
			r[6] = new Regex(@"<%else(( (?:\s*)if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?))?)(?:\s*)%>", options);
			r[7] = new Regex(@"<%\/if%>", options);

			//����{var.a}
			r[8] = new Regex(@"(\{strtoint\(([^\s]+?)\)\})", options);

			//����{request[a]}
			r[9] = new Regex(@"(<%urlencode\(([^\s]+?)\)%>)", options);

			//����{var[a]}
			r[10] = new Regex(@"(<%datetostr\(([^\s]+?),(.*?)\)%>)", options);
			r[11] = new Regex(@"(\{([^\.\[\]\{\}\s]+)\.([^\[\]\{\}\s]+)\})", options);

			//������ͨ����{}
			r[12] = new Regex(@"(\{request\[([^\[\]\{\}\s]+)\]\})", options);

			//����==���ʽ
			r[13] = new Regex(@"(\{([^\[\]\{\}\s]+)\[([^\[\]\{\}\s]+)\]\})", options);

			//����==���ʽ
			r[14] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

			//������ͨ����{}
			r[15] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

			//����==���ʽ
			r[16] = new Regex(@"(([=|>|<|!]=)\\" + "\"" + @"([^\s]*)\\" + "\")", options);
			
			//�����ռ�
			r[17] = new Regex(@"<%namespace (?:""?)([\s\S]+?)(?:""?)%>", options);
			
			//C#����
			r[18] = new Regex(@"<%csharp%>([\s\S]+?)<%/csharp%>", options);

			//set��ǩ
			r[19] = new Regex(@"<%set ((\(([a-zA-Z]+)\))?)(?:\s*)\{([^\s]+)\}(?:\s*)=(?:\s*)(.*?)(?:\s*)%>", options);

			//��ȡ�ַ���
			r[20] = new Regex(@"(<%getsubstring\(([^\s]+?),(.\d*?),([^\s]+?)\)%>)", options);
		  
			//repeat��ǩ
			r[21] = new Regex(@"<%repeat\(([^\s]+?)(?:\s*),(?:\s*)([^\s]+?)\)%>", options);

			//�̳���Inherits
			r[22] = new Regex(@"<%inherits (?:""?)([\s\S]+?)(?:""?)%>", options);

			r[23] = new Regex(@"<%continue%>");
			r[24] = new Regex(@"<%break%>");
		}

        public virtual string GetTemplate(string forumPath, string skinName, string templateName, int nest, int templateId)
        {
            return GetTemplate(forumPath, skinName, templateName, "", nest, templateId);
        }
		/// <summary>
		/// ���ģ���ַ���. ���Ȳ��һ���. ������ڻ�������������е�ģ��·������ȡģ���ļ�.
		/// ģ���ļ���·����Web.config�ļ�������.
		/// �����ȡ�ļ��ɹ���Ὣ���ݷ��ڻ�����.
		/// </summary>
		/// <param name="forumPath">ģ��·��</param>
		/// <param name="skinName">ģ����</param>
		/// <param name="templateName">ģ���ļ����ļ�����, Ҳ�ǻ����е�ģ������.</param>
        /// <param name="templateSubDirectory">��ģ���ļ�������,���ɶ���ģ���ļ�ʱΪ��,���������ļ����е�ģ����Ϊ���ļ�������</param>
		/// <param name="nest">Ƕ�״���</param>
		/// <param name="templateId">ģ��id</param>
		/// <returns>stringֵ,���ʧ����Ϊ"",�ɹ���Ϊģ�����ݵ�string</returns>
		public virtual string GetTemplate(string forumPath,string skinName, string templateName, string templateSubDirectory, int nest,int templateId)
		{
            if(nest == 2 && headerTemplateCache.ContainsKey(forumPath + skinName + "/" + templateName)) //�����һ����ģ��,�����Ѿ������ڻ�����
                return headerTemplateCache[forumPath + skinName + "/" + templateName];

            StringBuilder strReturn = new StringBuilder(220000);
			if (nest < 1)
				nest = 1;
            else if (nest > 5)
				return "";

			string extNamespace = "";
            if (templateSubDirectory != string.Empty && !templateSubDirectory.EndsWith("\\"))
            {
                templateSubDirectory += "\\";
            }
			//����ģ��config���ȣ������htm
			string configPathFormatStr = "{0}\\{1}\\{2}{3}.config";
			string htmlPathFormatStr = "{0}\\{1}\\{2}{3}.htm";
			string createFilePath;
			string inherits = "Discuz.Web." + templateName;

            if (File.Exists(string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, templateSubDirectory, templateName))) //��ǰģ�嵱ǰ���ļ�����config����
                createFilePath = string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, templateSubDirectory, templateName);
            else if (File.Exists(string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, "", templateName))) //��ǰģ�嵱ǰ���ļ�����config����
                createFilePath = string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, "", templateName);
            else if (File.Exists(string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", templateSubDirectory, templateName))) //Ĭ��ģ�嵱ǰ���ļ�����config����
                createFilePath = string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", templateSubDirectory, templateName);
            else if (File.Exists(string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", "", templateName))) //Ĭ��ģ�嵱ǰ���ļ�����config����
                createFilePath = string.Format(configPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", "", templateName);
            else if (File.Exists(string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, templateSubDirectory, templateName))) //��ǰģ�嵱ǰ���ļ�����html����
                createFilePath = string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, templateSubDirectory, templateName);
            else if (File.Exists(string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, "", templateName))) //��ǰģ�嵱ǰ���ļ�����html����
                createFilePath = string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), skinName, "", templateName);
            else if (File.Exists(string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", templateSubDirectory, templateName))) //Ĭ��ģ�嵱ǰ���ļ�����html����
                createFilePath = string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", templateSubDirectory, templateName);
            else if (File.Exists(string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", "", templateName))) //Ĭ��ģ�嵱ǰ���ļ�����html����
                createFilePath = string.Format(htmlPathFormatStr, Utils.GetMapPath(forumPath + "templates"), "default", "", templateName);
            else //���ļ�����
                return "";

			using (StreamReader objReader = new StreamReader(createFilePath, Encoding.UTF8))
			{
                StringBuilder textOutput = new StringBuilder(70000);
				
				textOutput.Append(objReader.ReadToEnd());
				objReader.Close();

				//���������ռ�
				if (nest == 1)
				{
					//�����ռ�
					foreach (Match m in r[17].Matches(textOutput.ToString()))
					{
						extNamespace += "\r\n<%@ Import namespace=\"" + m.Groups[1] + "\" %>";
						textOutput.Replace(m.Groups[0].ToString(), string.Empty);
					}

					//inherits
					foreach (Match m in r[22].Matches(textOutput.ToString()))
					{
						inherits = m.Groups[1].ToString();
						textOutput.Replace(m.Groups[0].ToString(), string.Empty);
						break;
					}
					if ("\"".Equals(inherits))
					{
						inherits = "Discuz.Forum.PageBase";
					}

				}
				//����Csharp���
				foreach (Match m in r[18].Matches(textOutput.ToString()))
				{
					textOutput.Replace(m.Groups[0].ToString(), m.Groups[0].ToString().Replace("\r\n", "\r\t\r"));
				}

				textOutput.Replace("\r\n", "\r\r\r");
				textOutput.Replace("<%", "\r\r\n<%");
				textOutput.Replace("%>", "%>\r\r\n");
				textOutput.Replace("<%csharp%>\r\r\n", "<%csharp%>").Replace("\r\r\n<%/csharp%>", "<%/csharp%>");
				
				string[] strlist = Utils.SplitString(textOutput.ToString(), "\r\r\n");
				int count = strlist.GetUpperBound(0);
		
				for (int i = 0; i <= count; i++)
				{
                    if (strlist[i] == "")
                        continue;
					strReturn.Append(ConvertTags(nest,forumPath, skinName, templateSubDirectory, strlist[i], templateId));
				}
			}
			if (nest == 1)
			{
                string template = string.Format("<%@ Page language=\"c#\" AutoEventWireup=\"false\" EnableViewState=\"false\" Inherits=\"{0}\" %>\r\n<%@ Import namespace=\"System.Data\" %>\r\n<%@ Import namespace=\"Discuz.Common\" %>\r\n<%@ Import namespace=\"Discuz.Forum\" %>\r\n<%@ Import namespace=\"Discuz.Entity\" %>\r\n<%@ Import namespace=\"Discuz.Config\" %>\r\n{1}\r\n<script runat=\"server\">\r\noverride protected void OnInit(EventArgs e)\r\n{{\r\n\r\n\t/* \r\n\t\tThis page was created by Discuz!NT Template Engine at {2}.\r\n\t\t��ҳ�������Discuz!NTģ������������ {2}. \r\n\t*/\r\n\r\n\tbase.OnInit(e);\r\n\r\n\ttemplateBuilder.Capacity = {3};\r\n{4}\r\n\tResponse.Write(templateBuilder.ToString());\r\n}}\r\n</script>\r\n", inherits, extNamespace, DateTime.Now, strReturn.Capacity, Regex.Replace(strReturn.ToString(), @"\r\n\s*templateBuilder\.Append\(""""\);", ""));

				string pageDir = Utils.GetMapPath(forumPath + "aspx\\" + templateId + "\\");
				if (!Directory.Exists(pageDir))
					Utils.CreateDir(pageDir);

                string outputPath = pageDir  + templateName + ".aspx";
				
				using (FileStream fs = new FileStream(outputPath, FileMode.Create,FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					Byte[] info = Encoding.UTF8.GetBytes(template);
					fs.Write(info, 0, info.Length);
					fs.Close();
				}
				
			}
            if(nest == 2)
                headerTemplateCache.Add(forumPath + skinName + "/" + templateName, strReturn.ToString());

            return strReturn.ToString();
		}
		
		/// <summary>
		/// ת����ǩ
		/// </summary>
		/// <param name="nest">���</param>
		/// <param name="forumPath">ģ��·��</param>
		/// <param name="skinName">ģ������</param>
		/// <param name="inputStr">ģ������</param>
		/// <param name="templateid">ģ��id</param>
		/// <returns></returns>
        private string ConvertTags(int nest, string forumPath, string skinName, string templateSubDirectory, string inputStr, int templateid)
		{
			string strReturn = "";
			string strTemplate;
			strTemplate = inputStr.Replace("\\", "\\\\");
			strTemplate = strTemplate.Replace("\"", "\\\"");
            strTemplate = strTemplate.Replace("</script>", "</\");\r\n\ttemplateBuilder.Append(\"script>");
			bool IsCodeLine = false;		

			foreach (Match m in r[0].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "\r\n" + GetTemplate(forumPath,skinName, m.Groups[1].ToString(), templateSubDirectory, nest + 1, templateid) + "\r\n");
			}
			foreach (Match m in r[1].Matches(strTemplate))
			{
				IsCodeLine = true;
				if (m.Groups[3].ToString() == "")
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("\r\n\tint {0}__loop__id=0;\r\n\tforeach(DataRow {0} in {1}.Rows)\r\n\t{{\r\n\t\t{0}__loop__id++;\r\n", m.Groups[4], m.Groups[5]));
				}
				else
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("\r\n\tint {1}__loop__id=0;\r\n\tforeach({0} {1} in {2})\r\n\t{{\r\n\t\t{1}__loop__id++;\r\n", m.Groups[3], m.Groups[4], m.Groups[5]));
				}
			}

			foreach (Match m in r[2].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\t//end loop\r\n");
			}

			foreach (Match m in r[3].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					string.Format("\r\n\tint {0}__loop__id=0;\r\nwhile({0}.Read())\r\n\t{{\r\n{0}__loop__id++;\r\n", m.Groups[1]));
			}

			foreach (Match m in r[4].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\t//end while\r\n" + m.Groups[1] + ".Close();\r\n");
			}

			foreach (Match m in r[5].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\tif (" + m.Groups[1].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
			}

			foreach (Match m in r[6].Matches(strTemplate))
			{
				IsCodeLine = true;
				if (m.Groups[1].ToString() == string.Empty)
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\r\n\telse\r\n\t{\r\n");
				}
				else
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						"\r\n\t}\r\n\telse if (" + m.Groups[3].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
				}
			}

			foreach (Match m in r[7].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"\r\n\t}\t//end if\r\n");
			}

			//����set
			foreach (Match m in r[19].Matches(strTemplate))
			{
				IsCodeLine = true;
				string type = "";
				if (m.Groups[3].ToString() != string.Empty)
				{
					type = m.Groups[3].ToString();
				}
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					string.Format("\t{0} {1} = {2};\r\n\t", type, m.Groups[4], m.Groups[5]).Replace("\\\"", "\"")
					);
			}

            foreach (Match m in r[21].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                                                 "\tfor (int i = 0; i < " + m.Groups[2] + "; i++)\r\n\t{\r\n\t\ttemplateBuilder.Append(" + m.Groups[1].ToString().Replace("\\\"", "\"").Replace("\\\\", "\\") + ");\r\n\t}\r\n");
            }

			foreach (Match m in r[23].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "\tcontinue;\r\n");			
			}

			foreach (Match m in r[24].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "\tbreak;\r\n");
			}
            
			foreach (Match m in r[8].Matches(strTemplate))
			{
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
					"Utils.StrToInt(" + m.Groups[2] + ", 0)");
			}

			foreach (Match m in r[9].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "templateBuilder.Append(Utils.UrlEncode(" + m.Groups[2] + "));");
			}

			foreach (Match m in r[10].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                              string.Format("\ttemplateBuilder.Append(TypeConverter.StrToDateTime({0}).ToString(\"{1}\"));", m.Groups[2], m.Groups[3].ToString().Replace("\\\"", string.Empty)));
			}

			//����substring
			foreach (Match m in r[20].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
				              string.Format("\ttemplateBuilder.Append(Utils.GetUnicodeSubString({0},{1},\"{2}\"));", m.Groups[2], m.Groups[3], m.Groups[4].ToString().Replace("\\\"", string.Empty)));
			}
					
			//����{var.a}
			foreach (Match m in r[11].Matches(strTemplate))
			{
				if (IsCodeLine)
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
						string.Format("{0}.{1}{2}", m.Groups[2], Utils.CutString(m.Groups[3].ToString(), 0, 1).ToUpper(), m.Groups[3].ToString().Substring(1, m.Groups[3].ToString().Length - 1)));
				else
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                        string.Format("\");\r\n\ttemplateBuilder.Append({0}.{1}{2}.ToString().Trim());\r\n\ttemplateBuilder.Append(\"", m.Groups[2], Utils.CutString(m.Groups[3].ToString(), 0, 1).ToUpper(), m.Groups[3].ToString().Substring(1, m.Groups[3].ToString().Length - 1)));
			}

			//����{request[a]}
			foreach (Match m in r[12].Matches(strTemplate))
			{
				if (IsCodeLine)
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "DNTRequest.GetString(\"" + m.Groups[2] + "\")");
				else
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + DNTRequest.GetString(\"{0}\") + \"", m.Groups[2]));
			}

			//����{var[a]}
			foreach (Match m in r[13].Matches(strTemplate))
			{
				if (IsCodeLine)
				{
					if (Utils.IsNumeric(m.Groups[3].ToString()))
						strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2] + "[" + m.Groups[3] + "].ToString().Trim()");
					else
					{
						if (m.Groups[3].ToString() == "_id")
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2] + "__loop__id");
						else
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2] + "[\"" + m.Groups[3] + "\"].ToString().Trim()");
					}
				}
				else
				{
					if (Utils.IsNumeric(m.Groups[3].ToString()))
						strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + {0}[{1}].ToString().Trim() + \"", m.Groups[2], m.Groups[3]));
					else
					{
						if (m.Groups[3].ToString() == "_id")
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + {0}__loop__id.ToString() + \"", m.Groups[2]));
						else
							strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + {0}[\"{1}\"].ToString().Trim() + \"", m.Groups[2], m.Groups[3]));
					}
				}
			}

			strTemplate = ReplaceSpecialTemplate(forumPath,skinName,strTemplate);

			foreach (Match m in r[14].Matches(strTemplate))
			{
				if (m.Groups[0].ToString() == "{commonversion}")
				{
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(), Utils.GetAssemblyVersion());
				}
			}
					
			//������ͨ����{}
			foreach (Match m in r[15].Matches(strTemplate))
			{
				if (IsCodeLine)
					strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2].ToString());
				else
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                        string.Format("\");\r\n\ttemplateBuilder.Append({0}.ToString());\r\n\ttemplateBuilder.Append(\"", m.Groups[2].ToString().Trim()));
			}
					
					
			//����==���ʽ
			foreach (Match m in r[16].Matches(strTemplate))
			{
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2] + "\"" + m.Groups[3] + "\"");
			}
				
				
			//����csharpcode
			foreach (Match m in r[18].Matches(strTemplate))
			{
				IsCodeLine = true;
				strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[1].ToString().Replace("\r\t\r", "\r\n\t").Replace("\\\"", "\""));
			}
					
			if (IsCodeLine)
			{
				strReturn = strTemplate + "\r\n";
			}
			else
			{
				if (strTemplate.Trim() != "")
				{
                    //StringBuilder sb = new StringBuilder(35000);
                    //foreach (string temp in Utils.SplitString(strTemplate, "\r\r\r"))
                    //{
                    //    if (temp.Trim() == "")
                    //        continue;
                    //    sb.Append("\ttemplateBuilder.Append(\"" + temp + "\\r\\n\");\r\n");
                    //}
                    //strReturn = sb.ToString();
                    //����Ч�ʲ�֧����һ����ʹ��if else endif,���磺<%if {isnarrowpage}%>�л������<%else%>�л���խ��<%/if%>
                    strReturn = "\ttemplateBuilder.Append(\"" + strTemplate.Replace("\r\r\r", "\\r\\n") + "\");";
                    strReturn = strReturn.Replace("\\r\\n<?xml", "<?xml");
                    strReturn = strReturn.Replace("\\r\\n<!DOCTYPE", "<!DOCTYPE");
				}
			}
			return strReturn;
		}		
	}    
}