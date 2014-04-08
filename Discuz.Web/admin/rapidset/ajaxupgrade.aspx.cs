using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Xml;
using System.Threading;

using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Admin.AutoUpdateManager;
using Discuz.Forum;
using ICSharpCode.SharpZipLib.Zip;


namespace Discuz.Web.Admin
{
    public class ajaxupgrade : AdminPage
    {
        private string ver;
        private string upgradedir;
        protected void Page_Load(object sender, EventArgs e)
        {
            ver = DNTRequest.GetString("ver");
            upgradedir = BaseConfigs.GetForumPath.ToLower() + "cache/upgrade/" + ver;
            try
            {
                if (DNTRequest.GetString("op") == "downupgradefile")
                {
                    bool isrequired = DNTRequest.GetString("upgradetype") == "required" ? true : false;
                    if(!isrequired)
                    {
                        ver = "dnt" + Utils.GetAssemblyVersion() + "/" + ver;
                    }
                    SaveFile(BaseConfigs.GetDbType.ToLower(),isrequired,ver, "begin.aspx");
                    SaveFile(BaseConfigs.GetDbType.ToLower(),isrequired,ver, "sql.config");
                    SaveFile(BaseConfigs.GetDbType.ToLower(),isrequired,ver, "end.aspx");
                }
                if (DNTRequest.GetString("op") == "downzip")
                {
                    bool isrequired = DNTRequest.GetString("upgradetype") == "required" ? true : false;
                    if(!isrequired)
                    {
                        ver = "dnt" + Utils.GetAssemblyVersion() + "/" + ver;
                    }
                    SaveFile(BaseConfigs.GetDbType.ToLower(), isrequired, ver, "upgrade.zip");
                }
                if (DNTRequest.GetString("op") == "unzip")
                {
                    if (File.Exists(Utils.GetMapPath(upgradedir + "/upgrade.zip")))
                    {
                        UnZipFile(Utils.GetMapPath(upgradedir + "/upgrade.zip"),
                              Utils.GetMapPath(upgradedir + "/upgrade"));
                    }
                }
                if (DNTRequest.GetString("op") == "dispose")
                {
                    if (Directory.Exists(Utils.GetMapPath(upgradedir)))
                    {
                        if (!Directory.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "cache/upgradebackup/" + ver)))
                        {
                            Directory.CreateDirectory(
                                Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "cache/upgradebackup/" + ver));
                        }
                        DisposeFile(""); 
                    }
                }
                if(DNTRequest.GetString("op") == "runsql")
                {
                    string step = DNTRequest.GetString("step");
                    string optional = DNTRequest.GetString("optional") != "" ? "&optional=true" : "";
                    if(step == "1")
                    {
                        if(File.Exists(Utils.GetMapPath(upgradedir + "/begin.aspx")))
                        {
                            Server.Transfer(upgradedir + "/begin.aspx?ver=" + ver + optional);
                        }
                        else
                        {
                            step = "2";
                        }
                    }
                    if(step == "2")
                    {
                        if (File.Exists(Utils.GetMapPath(upgradedir + "/sql.config")))
                        {
                            RunSql();
                        }
                        step = "3";
                    }
                    if(step =="3")
                    {
                        if (File.Exists(Utils.GetMapPath(upgradedir + "/end.aspx")))
                        {
                            Server.Transfer(upgradedir + "/end.aspx?ver=" + ver + optional);
                        }
                        else
                        {
                            step = "4";
                        }
                    }
                    if(step == "4")
                    {
                        if(optional == "")
                        {
                            SaveRequiredUpgradeInfo();
                        }
                        else
                        {
                            SaveOptionalUpgradeInfo();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException exp)
            {
                Response.Write(exp.Message);
            }
            catch(ThreadAbortException exp)
            {
                string err = exp.Message;
            }
            catch (Exception exp)
            {
                Response.Write(exp.Message);
            }
        }

        private void SaveRequiredUpgradeInfo()
        {
            XmlDocument lastupdate = new XmlDocument();
            lastupdate.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            lastupdate.SelectSingleNode("/localupgrade/requiredupgrade").InnerText = ver;
            lastupdate.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if(Directory.Exists(Utils.GetMapPath(upgradedir)))
            {
                Directory.Delete(Utils.GetMapPath(upgradedir),true);
            }
        }

        private void SaveOptionalUpgradeInfo()
        {
            XmlDocument lastupdate = new XmlDocument();
            lastupdate.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            XmlNode dntver = lastupdate.SelectSingleNode("/localupgrade/optionalupgrade/dnt" + Utils.GetAssemblyVersion());
            if (dntver == null)
            {
                dntver = lastupdate.CreateElement("dnt" + Utils.GetAssemblyVersion());
            }
            XmlElement item = lastupdate.CreateElement("item");
            item.InnerText = ver;
            dntver.AppendChild(item);
            if (lastupdate.SelectSingleNode("/localupgrade/optionalupgrade") == null)
                lastupdate.SelectSingleNode("/localupgrade").AppendChild(lastupdate.CreateElement("optionalupgrade"));
            lastupdate.SelectSingleNode("/localupgrade/optionalupgrade").AppendChild(dntver);
            lastupdate.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if (Directory.Exists(Utils.GetMapPath(upgradedir)))
            {
                Directory.Delete(Utils.GetMapPath(upgradedir), true);
            }
        }

        private void RunSql()
        {
            StringBuilder sb = new StringBuilder();
            BaseConfigInfo baseconfig = BaseConfigs.GetBaseConfig();
            string sqlpath = Utils.GetMapPath(upgradedir + "/sql.config");
            if (!File.Exists(sqlpath))
            {
                sqlpath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "upgrade/" + ver + "/sql.config");
            }
            using (StreamReader objReader = new StreamReader(sqlpath, Encoding.UTF8))
            {
                sb.Append(objReader.ReadToEnd());
                objReader.Close();
            }

            string[] sqlArray = sb.Replace("dnt_", baseconfig.Tableprefix).ToString().Trim().Split(new string[] { "GO\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string sqlstr in sqlArray)
            {
                if (sqlstr.Trim() == "")
                {
                    continue;
                }
                try
                {
                    Databases.RunSql(sqlstr);
                }
                catch(Exception e)
                {
                    Response.Write(e.Message);
                }
            }

            //foreach (string sqlstr in sb.Replace("dnt_", baseconfig.Tableprefix).ToString().Split(';'))
            //{
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstr);
            //}
        }

        /// <summary>
        /// 功能：解压zip格式的文件。
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <returns>解压是否成功</returns>
        private bool UnZipFile(string zipFilePath, string unZipDir)
        {
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("\\"))
                unZipDir += "\\";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(unZipDir + directoryName);
                    }
                    if (!directoryName.EndsWith("\\"))
                        directoryName += "\\";
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }//while
            }
            return true;
        }

        /// <summary>
        /// 部署文件
        /// </summary>
        private void DisposeFile(string path)
        {
            string upgradepath = Utils.GetMapPath(upgradedir + "/upgrade/" + path);
            string targetpath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + path);
            string backuppath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "cache/upgradebackup/" + DNTRequest.GetString("ver") + "/" + path);
            DirectoryInfo sourceDir = new DirectoryInfo(upgradepath);
            if(!Directory.Exists(targetpath))   //如果目标文件夹不存在则建立
            {
                Directory.CreateDirectory(targetpath);
            }
            if (!Directory.Exists(backuppath))   //如果备份文件夹不存在则建立
            {
                Directory.CreateDirectory(backuppath);
            }
            foreach (DirectoryInfo xDir in sourceDir.GetDirectories())
            {
                DisposeFile(path + xDir.Name + "/");
            }
            foreach(FileInfo xfile in sourceDir.GetFiles())
            {
                try
                {
                    if (File.Exists(targetpath + xfile.Name))
                    {
                        if (File.Exists(backuppath + xfile.Name))
                        {
                            File.Delete(backuppath + xfile.Name);
                        }
                        File.Move(targetpath + xfile.Name, backuppath + xfile.Name);
                    }
                    File.Move(upgradepath + xfile.Name, targetpath + xfile.Name);
                }
                catch (UnauthorizedAccessException exp)
                {
                    string err = exp.Message;
                    throw new UnauthorizedAccessException("对路径\"" + targetpath + "\"的访问被拒绝。可能无写权限。");
                }
            }
        }

        private void SaveFile(string dbtype, bool isrequired, string version, string filename)
        {
            AutoUpdate autoUpdate = new AutoUpdate();
            byte[] context = autoUpdate.GetFile(dbtype,isrequired,version,filename);
            if(context.Length == 0)
            {
                return;
            }
            if(!Directory.Exists(Utils.GetMapPath(upgradedir)))
            {
                Directory.CreateDirectory(Utils.GetMapPath(upgradedir));
            }
            FileStream fs = new FileStream(Utils.GetMapPath(upgradedir + "/" + filename), FileMode.Create);
            fs.Write(context,0,context.Length);
            fs.Close();
        }
    }
}