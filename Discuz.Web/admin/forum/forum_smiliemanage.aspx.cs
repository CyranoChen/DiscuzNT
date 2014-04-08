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
            #region ������
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
            smilesgrid.TableHeaderName = "��̳�����б�";
            smilesgrid.BindData(Smilies.GetSmilies());
            ViewState["dir"] = d;
            ViewState["dirList"] = dirList;
            if (emptySmilieList != "")
            {
                base.RegisterStartupScript("", "<script>alert('" + emptySmilieList + " Ϊ��,�Ѿ����Ƴ�!');</script>");
            }            
            #endregion
        }

        private DirectoryInfo[] GetSmilesDirList()
        {
            #region ��ȡ�����������ļ���
            string path = BaseConfigs.GetForumPath + "editor/images/smilies";
            DirectoryInfo dirinfo = new DirectoryInfo(Utils.GetMapPath(path));
            return dirinfo.GetDirectories();
            #endregion
        }

        private void SmilesListBind()
        {
            #region �󶨱����ļ��ڵ�����
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
            #region �ύ��ѡ�ı������
            for (int i = 1; i <= dirList.Count; i++)
            {
                if (DNTRequest.GetFormString("id" + i) != null && DNTRequest.GetFormString("id" + i) != "")
                {
                    AdminForums.CreateSmilies(DNTRequest.GetInt("order" + i, 0), 0, DNTRequest.GetFormString("group" + i),
                                                 DNTRequest.GetFormString("url" + i),
                                                 userid, username, usergroupid, grouptitle, ip);

                    //��������������еı������
                    int maxSmilieId = Smilies.GetMaxSmiliesId() - 1;
                    int order = 1;
                    string url = DNTRequest.GetFormString("url" + i);
                    ArrayList fileList = GetSmilesFileList(DNTRequest.GetFormString("url" + i));
                    foreach (string file in fileList)
                    {
                        if (file.ToLower() == "thumbs.db")  //���˵�thumbs.db�ļ�
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
                throw new IOException("�����ļ��в�����!");
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
            #region ����Ա�����Ϣ�ı༭
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
                    base.RegisterStartupScript("", "<script>alert('ĳЩ��¼���벻���������ݿ����Ѵ�����ͬ�ı���������');window.location.href='forum_smiliemanage.aspx';</script>");
                else
                    base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            }
            #endregion
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            #region ɾ����ѡ�ı������
            if (this.CheckCookie())
            {
                string[] delIds = DNTRequest.GetString("id").Split(',');

                //������ȫ��ɾ�������ٵñ���һ�����
                if (smilesgrid.Items.Count == delIds.Length)
                {
                    base.RegisterStartupScript("", "<script>alert('�����ٱ���һ��Ĭ�ϱ��飬�������һ���±������ɾ��������飡');window.location.href='forum_smiliemanage.aspx';</script>");
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
        //    #region ���±��黺��
        //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesList");
        //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListFirstPage");
        //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListWithInfo");
        //    #endregion
        //}

        #region Web ������������ɵĴ���
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
