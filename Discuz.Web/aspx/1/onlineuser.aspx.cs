using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
	/// <summary>
	/// 在线用户列表页
	/// </summary>
	public class onlineuser : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 在线用户列表
		/// </summary>
        public DataTable onlineuserlist = CreateUserTable();
        /// <summary>
        /// 在线用户数
        /// </summary>
        public int onlineusernumber = 0;
		/// <summary>
        /// 当前页码
		/// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 总页数
        /// </summary>
		public int pagecount = 0;
        /// <summary>
        /// 分页页码链接
        /// </summary>
		public string  pagenumbers = "";
        /// <summary>
        /// 在线用户总数
        /// </summary>
		public int totalonline;
        /// <summary>
        /// 在线注册用户数
        /// </summary>
		public int totalonlineuser;
        /// <summary>
        /// 在线游客数
        /// </summary>
		public int totalonlineguest;
        /// <summary>
        /// 在线隐身用户数
        /// </summary>
		public int totalonlineinvisibleuser;
        /// <summary>
        /// 最高在线用户数
        /// </summary>
		public string highestonlineusercount;
        /// <summary>
        /// 最高在线用户数发生时间
        /// </summary>
		public string highestonlineusertime;
        /// <summary>
        /// 开始行数
        /// </summary>
        private int startrow = 0;
        /// <summary>
        /// 结束行数
        /// </summary>
        private int endrow = 0;

        #endregion

	   
		protected override void ShowPage()
		{
            pagetitle = "在线列表";
			DataTable allonlineuserlist = OnlineUsers.GetOnlineUserList(onlineusercount, out totalonlineguest, out totalonlineuser, out totalonlineinvisibleuser);;
			onlineusernumber = onlineusercount;

			//获取总页数
            pagecount = onlineusernumber % 16 == 0 ? onlineusernumber / 16 : onlineusernumber / 16 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;

			//修正请求页数中可能的错误
			if (pageid <= 1)
			{
				pageid = 1;
				startrow = 0;
				endrow = 15;
			}
			else
			{
				pageid = pageid > pagecount ? pagecount : pageid;
                startrow = (pageid - 1) * 16;
                endrow = pageid * 16;
			}

            if (Discuz.Config.EntLibConfigs.GetConfig() != null && Discuz.Config.EntLibConfigs.GetConfig().Cacheonlineuser.Enable)
            {
                foreach (DataRow dr in allonlineuserlist.Rows)
                {
                    try
                    {
                        DataRow newrow = onlineuserlist.NewRow();
                        newrow["username"] = dr["username"].ToString();
                        newrow["userid"] = TypeConverter.ObjectToInt(dr["userid"]);
                        newrow["invisible"] = TypeConverter.ObjectToInt(dr["invisible"]);
                        newrow["lastupdatetime"] = TypeConverter.ObjectToDateTime(dr["lastupdatetime"]);
                        string actionid = dr["action"].ToString().Trim();
                        if (!Utils.StrIsNullOrEmpty(actionid))
                            newrow["action"] = UserAction.GetActionDescriptionByID(TypeConverter.StrToInt(actionid));

                        newrow["forumid"] = TypeConverter.ObjectToInt(dr["forumid"]);
                        newrow["forumname"] = dr["forumname"].ToString();
                        newrow["topicid"] = TypeConverter.ObjectToInt(dr["titleid"]);
                        newrow["title"] = dr["title"].ToString();

                        onlineuserlist.Rows.Add(newrow);
                        onlineuserlist.AcceptChanges();
                    }
                    catch { ; }
                }
            }
            else
            {
                startrow = startrow >= onlineusernumber ? onlineusernumber - 1 : startrow;
                endrow = endrow >= onlineusernumber ? onlineusernumber - 1 : endrow;

                for (; startrow <= endrow; startrow++)
                {
                    try
                    {
                        DataRow newrow = onlineuserlist.NewRow();

                        newrow["username"] = allonlineuserlist.Rows[startrow]["username"];
                        newrow["userid"] = TypeConverter.ObjectToInt(allonlineuserlist.Rows[startrow]["userid"]);
                        newrow["invisible"] = TypeConverter.ObjectToInt(allonlineuserlist.Rows[startrow]["invisible"]);
                        newrow["lastupdatetime"] = Convert.ToDateTime(allonlineuserlist.Rows[startrow]["lastupdatetime"]);
                        string actionid = allonlineuserlist.Rows[startrow]["action"].ToString().Trim();
                        if (!Utils.StrIsNullOrEmpty(actionid))
                            newrow["action"] = UserAction.GetActionDescriptionByID(TypeConverter.StrToInt(actionid));

                        newrow["forumid"] = TypeConverter.ObjectToInt(allonlineuserlist.Rows[startrow]["forumid"]);
                        newrow["forumname"] = allonlineuserlist.Rows[startrow]["forumname"];
                        newrow["topicid"] = TypeConverter.ObjectToInt(allonlineuserlist.Rows[startrow]["titleid"]);
                        newrow["title"] = allonlineuserlist.Rows[startrow]["title"];

                        onlineuserlist.Rows.Add(newrow);
                        onlineuserlist.AcceptChanges();
                    }
                    catch { ; }
                }
            }

			//得到页码链接
			pagenumbers = DNTRequest.GetString("search") == "" ? Utils.GetPageNumbers(pageid, pagecount, "onlineuser.aspx", 8) : 
                   Utils.GetPageNumbers(pageid, pagecount, "onlineuser.aspx", 8);

			totalonline = onlineusercount;
			highestonlineusercount = Statistics.GetStatisticsRowItem("highestonlineusercount");
			highestonlineusertime = Statistics.GetStatisticsRowItem("highestonlineusertime");
	   	}


		public static DataTable CreateUserTable()
		{
			DataTable dt = new DataTable("onlineuser");
		
			dt.Columns.Add("userid", System.Type.GetType("System.Int32"));
			dt.Columns["userid"].AllowDBNull = false;

			dt.Columns.Add("invisible", System.Type.GetType("System.Int32"));
			dt.Columns["invisible"].AllowDBNull = false;

			dt.Columns.Add("username", System.Type.GetType("System.String"));
			dt.Columns["username"].AllowDBNull = false;
			dt.Columns["username"].MaxLength = 20;
			dt.Columns["username"].DefaultValue = "";
			
			dt.Columns.Add("lastupdatetime", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("forumid", System.Type.GetType("System.Int32"));
            dt.Columns.Add("topicid", System.Type.GetType("System.Int32"));

			dt.Columns.Add("action", System.Type.GetType("System.String"));
			dt.Columns["action"].MaxLength = 40;
			dt.Columns["action"].DefaultValue = "";

			dt.Columns.Add("forumname", System.Type.GetType("System.String"));
			dt.Columns["forumname"].MaxLength = 50;
			dt.Columns["forumname"].DefaultValue = "";

			dt.Columns.Add("title", System.Type.GetType("System.String"));
			dt.Columns["title"].MaxLength = 80;
			dt.Columns["title"].DefaultValue = "";

            return dt;
		}  
	}
}
