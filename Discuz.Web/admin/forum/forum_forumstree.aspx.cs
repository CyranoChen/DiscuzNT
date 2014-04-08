using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Plugin.Mall;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 论坛版块托动
    /// </summary>
    
    public partial class forumstree : AdminPage
    {
        #region 图标信息变量声明

        private string T_rootpic = "<img src=../images/lines/tplus.gif align=absmiddle>";
        private string L_rootpic = "<img src=../images/lines/lplus.gif align=absmiddle>";
        private string L_TOP_rootpic = "<img src=../images/lines/rplus.gif align=absmiddle>";
        private string I_rootpic = "<img src=../images/lines/dashplus.gif align=absmiddle>";
        private string T_nodepic = "<img src=../images/lines/tminus.gif align=absmiddle>";
        private string L_nodepic = "<img src=../images/lines/lminus.gif align=absmiddle>";
        private string I_nodepic = "<img src=../images/lines/i.gif align=absmiddle>";
        private string No_nodepic = "<img src=../images/lines/noexpand.gif align=absmiddle>";

        #endregion

        public string str = "";
        public int noPicCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DNTRequest.GetString("currentfid") != "")
            {
                if (!AdminForums.MovingForumsPos(DNTRequest.GetString("currentfid"), DNTRequest.GetString("targetfid"), DNTRequest.GetString("isaschildnode") == "1" ? true : false))
                {
                    base.RegisterStartupScript( "", "<script>alert('当前版块下面有子版块,因此无法移动!');window.location.href='forum_forumsTree.aspx';</script>");
                }
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "移动论坛版块", "移动论坛版块ID:" + DNTRequest.GetString("currentfid") + "到ID:" + DNTRequest.GetString("targetfid"));
            }

            if (!Page.IsPostBack)
            {
                DataTable dt = Forums.GetForumListForDataTable();

                ViewState["dt"] = dt;

                if (dt.Rows.Count == 0)
                {
                    Server.Transfer("forum_AddFirstForum.aspx"); //如果版块表中没有任何版块, 则跳转到"添加第一个版块"页面. 
                }
                else
                {
                    AddTree(0, dt.Select("layer=0 AND [parentid]=0"), "");

                    str = "<script type=\"text/javascript\">\r\n  var obj = [" + str;
                    str = str.Substring(0, str.Length - 3);
                    str += "];\r\n var newtree = new tree(\"newtree\",obj,\"reSetTree\");";
                    str += "</script>";
                }
                ShowTreeLabel.Text = str;
            }
        }


        private void AddTree(int layer, DataRow[] drs, string currentnodestr)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (layer == 0)
            {
                #region 作为根结点

                for (int n = 0; n < drs.Length; n++)
                {
                    string mystr = "";
                    if (drs.Length == 1)
                    {
                        mystr += I_rootpic; //
                        currentnodestr = No_nodepic;
                    }
                    else
                    {
                        if (n == 0)
                        {
                            mystr += L_TOP_rootpic; //
                            currentnodestr = I_nodepic;
                        }
                        else
                        {
                            if ((n > 0) && (n < (drs.Length - 1)))
                            {
                                mystr += T_rootpic; //
                                currentnodestr = I_nodepic;
                            }
                            else
                            {
                                mystr += L_rootpic;
                                currentnodestr = No_nodepic;
                            }
                        }
                    }

                    str += "{fid:" + drs[n]["fid"].ToString() + ",name:\"" +
                           Utils.HtmlEncode(drs[n]["name"].ToString().Trim().Replace("\\", "\\\\ ")) + "\",subject:\" " +
                           mystr + " <img src=../images/folders.gif align=\\\"absmiddle\\\" > <a href=\\\"../../showforum.aspx?forumid=" + drs[n]["fid"].ToString() + "\\\" target=\\\"_blank\\\">" +
                           Utils.HtmlEncode(drs[n]["name"].ToString().Trim().Replace("\\", "\\\\ ")) + "</a>\",linetitle:\"" +
                           mystr + "\",parentidlist:0,layer:" + drs[n]["layer"].ToString() + ",subforumcount:" +
                           drs[n]["subforumcount"].ToString() + ",istrade:" + GetIsTrade(drs[n]["istrade"].ToString()) + "},\r\n";

                    if (Convert.ToInt32(drs[n]["subforumcount"].ToString()) > 0)
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["fid"].ToString();
                        AddTree(mylayer, dt.Select(selectstr), currentnodestr);
                    }
                }

                #endregion
            }
            else
            {
                #region 作为版块

                for (int n = 0; n < drs.Length; n++)
                {
                    string mystr = "";
                    mystr += currentnodestr;
                    string temp = currentnodestr;

                    if ((n >= 0) && (n < (drs.Length - 1)))
                    {
                        mystr += T_nodepic; //
                        temp += I_nodepic;
                    }
                    else
                    {
                        mystr += L_nodepic;
                        noPicCount++;
                        temp += No_nodepic;
                    }

                    str += "{fid:" + drs[n]["fid"].ToString() + ",name:\"" +
                          Utils.HtmlEncode(drs[n]["name"].ToString().Trim().Replace("\\", "\\\\ ")) + "\",subject:\" " +
                          mystr + " <img src=../images/folder.gif align=\\\"absmiddle\\\" > <a href=\\\"../../showforum.aspx?forumid=" + drs[n]["fid"].ToString() + "\\\" target=\\\"_blank\\\">" +
                          Utils.HtmlEncode(drs[n]["name"].ToString().Trim().Replace("\\", "\\\\ ")) + "</a>\",linetitle:\"" +
                          mystr + "\",parentidlist:\"" + drs[n]["parentidlist"].ToString().Trim() + "\",layer:" +
                          drs[n]["layer"].ToString() + ",subforumcount:" + drs[n]["subforumcount"].ToString() + ",istrade:" + GetIsTrade(drs[n]["istrade"].ToString()) +
                          "},\r\n";


                    if (Convert.ToInt32(drs[n]["subforumcount"].ToString()) > 0)
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["fid"].ToString();
                        AddTree(mylayer, dt.Select(selectstr), temp);
                    }
                }

                #endregion
            }
        }

        private int GetIsTrade(string istrade)
        {
            if (istrade == "0")
                return 0;
            return MallPluginProvider.GetInstance() == null ? 0 : 1;
        }

        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion
    }
}