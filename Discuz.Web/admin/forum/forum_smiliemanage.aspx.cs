using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Cache;

namespace Discuz.Web.Admin
{
    public partial class smiliemanage : AdminPage
    {
        private ArrayList dirList = new ArrayList();

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SmilesGridBind();
            }
            SmilesListBind();
        }

        private void SmilesGridBind()
        {
            #region 绑定数据
            string emptySmilieList = Discuz.Forum.Smilies.ClearEmptySmiliesType();
            DirectoryInfo[] dirInfo = GetSmilesDirList();
            foreach (DirectoryInfo dir in dirInfo)
            {
                dirList.Add(dir.Name);
            }
            string d = "";
            foreach (DataRow dr in Discuz.Forum.Smilies.GetSmiliesTypes().Rows)
            {
                dirList.Remove(dr["url"]);
                d += dr["code"].ToString() + ",";
            }
            smilesgrid.TableHeaderName = "论坛表情列表";
            smilesgrid.BindData(Smilies.GetSmilies());
            ViewState["dir"] = d;
            ViewState["dirList"] = dirList;
            if (emptySmilieList != "")
            {
                base.RegisterStartupScript("", "<script>alert('" + emptySmilieList + " 为空,已经被移除!');</script>");
            }            
            #endregion
        }

        private DirectoryInfo[] GetSmilesDirList()
        {
            #region 获取表情下所有文件夹
            string path = BaseConfigs.GetForumPath + "editor/images/smilies";
            DirectoryInfo dirinfo = new DirectoryInfo(Utils.GetMapPath(path));
            return dirinfo.GetDirectories();
            #endregion
        }

        private void SmilesListBind()
        {
            #region 绑定表情文件内的内容
            dirinfoList.Text = "";
            dirList = (ArrayList)ViewState["dirList"];
            if (dirList.Count == 0)
            {
                SubmitButton.Visible = false;
            }
            else
            {
                int i = 1;
                foreach (string dir in dirList)
                {
                    dirinfoList.Text += "<tr class='mouseoutstyle' onmouseover='this.className=\"mouseoverstyle\"' onmouseout='this.className=\"mouseoutstyle\"' >\n";
                    dirinfoList.Text += "<td nowrap='nowrap' style='border: 1px solid rgb(234, 233, 225); width: 20px;'><input type='checkbox' id='id" + i + "' name='id" + i + "' value='" + i + "'/></td>\n";
                    dirinfoList.Text += "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='group" + i + "' name='group" + i + "' value='" + dir + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" /></td>\n";
                    dirinfoList.Text += "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='order" + i + "' name='order" + i + "' value='" + i + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" /></td>\n";
                    dirinfoList.Text += "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='hidden' name='url" + i + "' value='" + dir + "' />" + dir + "</td>\n";
                    dirinfoList.Text += "</tr>\n";
                    i++;
                }
            }
            #endregion
        }

        public void SubmitButton_Click(object sender, EventArgs e)
        {
            #region 提交所选的表情分类
            for (int i = 1; i <= dirList.Count; i++)
            {
                if (DNTRequest.GetFormString("id" + i) != null && DNTRequest.GetFormString("id" + i) != "")
                {
                    AdminForums.CreateSmilies(DNTRequest.GetInt("order" + i, 0), 0, DNTRequest.GetFormString("group" + i),
                                                 DNTRequest.GetFormString("url" + i),
                                                 userid, username, usergroupid, grouptitle, ip);

                    //将新增表情分类中的表情入库
                    int maxSmilieId = Smilies.GetMaxSmiliesId() - 1;
                    int order = 1;
                    string url = DNTRequest.GetFormString("url" + i);
                    ArrayList fileList = GetSmilesFileList(DNTRequest.GetFormString("url" + i));
                    foreach (string file in fileList)
                    {
                        if (file.ToLower() == "thumbs.db")  //过滤掉thumbs.db文件
                            continue;

                        AdminForums.CreateSmilies(order, maxSmilieId,
                                                 ":" + url + order + ":",
                                                 url + "/" + file, userid, username, usergroupid, grouptitle, ip);
                        order++;
                    }
                }
            }
            base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            #endregion
        }

        private ArrayList GetSmilesFileList(string smilesPath)
        {
            string path = BaseConfigs.GetForumPath + "editor/images/smilies/" + smilesPath;
            DirectoryInfo dir = new DirectoryInfo(Utils.GetMapPath(path));
            if (!dir.Exists)
            {
                throw new IOException("分类文件夹不存在!");
            }
            FileInfo[] files = dir.GetFiles();
            ArrayList temp = new ArrayList();
            foreach (FileInfo file in files)
            {
                temp.Add(file.Name);
            }
            return temp;
        }

        private void SaveSmiles_Click(object sender, EventArgs e)
        {
            #region 保存对表情信息的编辑
            if (this.CheckCookie())
            {
                int rowid = -1;
                bool error = false;
                foreach (object o in smilesgrid.GetKeyIDArray())
                {
                    string id = o.ToString();
                    string code = smilesgrid.GetControlValue(rowid, "code");
                    string displayorder = smilesgrid.GetControlValue(rowid, "displayorder");
                    string type = smilesgrid.GetControlValue(rowid, "type");
                    string url = smilesgrid.GetControlValue(rowid, "url");
                    rowid++;
                    if (code == "" || !Utils.IsNumeric(displayorder) || Smilies.IsExistSameSmilieCode(code, int.Parse(id)))
                    {
                        error = true;
                        continue;
                    }
                    AdminForums.UpdateSmilies(int.Parse(id), int.Parse(displayorder), Utils.StrToInt(type, 0), code, url, userid, username, usergroupid, grouptitle, ip);
                }
                if (error)
                    base.RegisterStartupScript("", "<script>alert('某些记录输入不完整或数据库中已存在相同的表情组名称');window.location.href='forum_smiliemanage.aspx';</script>");
                else
                    base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            }
            #endregion
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除所选的表情分类
            if (this.CheckCookie())
            {
                string[] delIds = DNTRequest.GetString("id").Split(',');

                //不允许全部删除，至少得保留一组表情
                if (smilesgrid.Items.Count == delIds.Length)
                {
                    base.RegisterStartupScript("", "<script>alert('请至少保留一组默认表情，或者添加一组新表情后，再删除本组表情！');window.location.href='forum_smiliemanage.aspx';</script>");
                    return;
                }
                foreach (string id in delIds)
                {
                    AdminForums.DeleteSmilies(id, userid, username, usergroupid, grouptitle, ip);
                    smilesgrid.EditItemIndex = -1;
                    SmilesGridBind();
                    Smilies.DeleteSmilyByType(int.Parse(id));
                }
                base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            }
            #endregion
        }

        //private void UpdateSmiliesCache()
        //{
        //    #region 更新表情缓存
        //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesList");
        //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListFirstPage");
        //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListWithInfo");
        //    #endregion
        //}

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
            this.SubmitButton.Attributes.Add("onclick", "return validate()");
            this.SaveSmiles.Click += new EventHandler(this.SaveSmiles_Click);
            smilesgrid.ColumnSpan = 5;
            smilesgrid.AllowPaging = false;
        }
        #endregion
    }
}
