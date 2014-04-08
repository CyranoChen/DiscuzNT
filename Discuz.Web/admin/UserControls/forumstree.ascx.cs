using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    ///		ForumsTree ��ժҪ˵����
    /// </summary>
    public partial class forumtree : UserControl
    {
        #region ͼƬ���ر���

        private string T_rootpic = "<img src=../images/lines/tplus.gif align=absmiddle>";
        private string L_rootpic = "<img src=../images/lines/lplus.gif align=absmiddle>";
        private string L_TOP_rootpic = "<img src=../images/lines/rplus.gif align=absmiddle>";
        private string I_rootpic = "<img src=../images/lines/dashplus.gif align=absmiddle>";

        private string T_nodepic = "<img src=../images/lines/tminus.gif align=absmiddle>";
        private string L_nodepic = "<img src=../images/lines/lminus.gif align=absmiddle>";
        private string I_nodepic = "<img src=../images/lines/i.gif align=absmiddle>";
        private string No_nodepic = "<img src=../images/lines/noexpand.gif align=absmiddle>";

        #endregion

        private int noPicCount = 0;
        public bool WithCheckBox = true;

        public string PageName = "forumbatchset";
        private string SelectForumStr = "";


        public StringBuilder sb = new StringBuilder();


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadForumTree();
        }

        public void LoadForumTree()
        {
            //��ȡ��̳�����
            DataTable dt = Forums.GetForumListForDataTable();

            if (dt.Rows.Count == 0) Server.Transfer("../forum/forum_AddFirstForum.aspx"); //���������û���κΰ��, ����ת��"��ӵ�һ�����"ҳ��. 

            ViewState["dt"] = dt;

            sb.Append("<table border=\"0\"  width=\"100%\" align=\"center\" cellspacing=\"0\" cellpadding=\"0\">");

            if (PageName.ToLower() != "advertisement") //���ֻ����ͨ��ʾ[�ǹ����ӻ�༭ʱ��ʾ]
            {
                if (WithCheckBox)
                {
                    sb.Append("<div style=\"height:30px\"><input class=\"input1\" title=\"ѡ��/ȡ��ѡ��\" onclick=\"CheckAllTreeByName(this.form,'" + this.ClientID + "','null')\" type=\"checkbox\" name=\"" + this.ClientID + "_chkall\"	id=\"" + this.ClientID + "_CheckAll\">ȫѡ/ȡ��ȫѡ</div>");
                }
                AddTree(0, dt.Select("layer=0 AND [parentid]=0"), "");
            }
            else //�����ӻ�༭ʱ��ʾ
            {
                int advid = DNTRequest.GetInt("advid", 0);
                DataTable ad_dt =Advertisements.GetAdvertisement(advid);
                if (ad_dt.Rows.Count > 0)
                {
                    this.SelectForumStr = "," + ad_dt.Rows[0]["targets"].ToString() + ",";
                }

                if (this.SelectForumStr.IndexOf("ȫ��") >= 0)
                {
                    sb.Append("<tr><td class=treetd> " + L_TOP_rootpic + "<img class=treeimg src=../images/aspx.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"ȫ��\"   checked> ȫ��</td></tr>");
                }
                else
                {
                    sb.Append("<tr><td class=treetd> " + L_TOP_rootpic + "<img class=treeimg src=../images/aspx.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"ȫ��\"   > ȫ��</td></tr>");
                }

                if ((this.SelectForumStr.IndexOf("��ҳ") >= 0) && (this.SelectForumStr.IndexOf("ȫ��") < 0))
                {
                    sb.Append("<tr><td class=treetd> " + T_rootpic + "<img class=treeimg src=../images/htm.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"��ҳ\"   checked> ��ҳ</td></tr>");
                }
                else
                {
                    sb.Append("<tr><td class=treetd> " + T_rootpic + "<img class=treeimg src=../images/htm.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"��ҳ\"   > ��ҳ</td></tr>");
                }
                AddAdsTree(0, dt.Select("layer=0 AND [parentid]=0"), "");
            }
            sb.Append("</table>");

            TreeContent.Text = sb.ToString();
        }

        private void AddTree(int layer, DataRow[] drs, string currentnodestr)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (layer == 0) //��Ϊ�����
            {
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

                    if (WithCheckBox)
                    {
                        sb.Append("<tr><td class=treetd> " + mystr + "<img border=0 src=../images/folders.gif align=\\\"absmiddle\\\" > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[n]["fid"].ToString().Trim() + "\"  onclick=\"javascript:Tree_SelectOneNode(this)\" > <a href=\"../../showforum-" + drs[n]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[n]["name"].ToString().Trim() + "</a></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td class=treetd> " + mystr + " <img border=0 src=../images/folders.gif align=\\\"absmiddle\\\" >  <a href=\"../../showforum-" + drs[n]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[n]["name"].ToString().Trim() + "</a></td></tr>");
                    }

                    if (Convert.ToInt32(drs[n]["subforumcount"].ToString()) > 0)
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["fid"].ToString();
                        AddTree(mylayer, dt.Select(selectstr), currentnodestr);
                    }
                }

            }
            else // ��Ϊ�ӽ��
            {
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

                    if (WithCheckBox)
                    {
                        sb.Append("<tr><td class=treetd> " + mystr + " <img class=treeimg  src=../images/folder.gif align=\\\"absmiddle\\\" > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[n]["fid"].ToString().Trim() + "\" onclick=\"javascript:Tree_SelectOneNode(this)\" > <a href=\"../../showforum-" + drs[n]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[n]["name"].ToString().Trim() + "</a></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td class=treetd> " + mystr + " <img class=treeimg  src=../images/folder.gif align=\\\"absmiddle\\\" > <a href=\"../../showforum-" + drs[n]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[n]["name"].ToString().Trim() + "</a></td></tr>");
                    }

                    if (Convert.ToInt32(drs[n]["subforumcount"].ToString()) > 0)
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["fid"].ToString();
                        AddTree(mylayer, dt.Select(selectstr), temp);
                    }
                }
            }
        }


        private void AddAdsTree(int layer, DataRow[] drs, string currentnodestr)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (layer == 0) //��Ϊ�����
            {
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
                        if ((n >= 0) && (n < (drs.Length - 1)))
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
                    sb.Append("<tr><td class=treetd> " + mystr + "<img src=../images/folders.gif class=treeimg > " + drs[n]["name"].ToString().Trim() + "</td></tr>");

                    if (Convert.ToInt32(drs[n]["subforumcount"].ToString()) > 0)
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["fid"].ToString();
                        AddAdsTree(mylayer, dt.Select(selectstr), currentnodestr);
                    }
                }

            }
            else // ��Ϊ�ӽ��
            {
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

                    if ((this.SelectForumStr.IndexOf("," + drs[n]["fid"].ToString().Trim() + ",") >= 0) && (this.SelectForumStr.IndexOf("ȫ��") < 0))
                    {
                        sb.Append("<tr><td class=treetd> " + mystr + " <img src=../images/folder.gif class=treeimg > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[n]["fid"].ToString().Trim() + "\"  checked> " + drs[n]["name"].ToString().Trim() + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td class=treetd> " + mystr + " <img src=../images/folder.gif class=treeimg > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[n]["fid"].ToString().Trim() + "\" > " + drs[n]["name"].ToString().Trim() + "</td></tr>");
                    }

                    if (Convert.ToInt32(drs[n]["subforumcount"].ToString()) > 0)
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["fid"].ToString();
                        AddAdsTree(mylayer, dt.Select(selectstr), temp);
                    }
                }
            }
        }


		private string _hintTitle = "";
		public string HintTitle
		{
			get { return _hintTitle; }
			set { _hintTitle = value; }
		}


		private string _hintInfo = "";
		public string HintInfo
		{
			get { return _hintInfo; }
			set { _hintInfo = value; }
		}

		private int _hintLeftOffSet = 0;
		public int HintLeftOffSet
		{
			get { return _hintLeftOffSet; }
			set { _hintLeftOffSet = value; }
		}

		private int _hintTopOffSet = 0;
		public int HintTopOffSet
		{
			get { return _hintTopOffSet; }
			set { _hintTopOffSet = value; }
		}

		private string _hintShowType = "up";//��"down"
		public string HintShowType
		{
			get { return _hintShowType; }
			set { _hintShowType = value; }
		}

		private int _hintHeight = 30;
		public int HintHeight
		{
			get { return _hintHeight; }
			set { _hintHeight = value; }
		}
    }
}