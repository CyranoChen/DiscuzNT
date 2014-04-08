using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.Common;

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    public class searchuser : System.Web.UI.UserControl
    {
        public string userListTable = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string searchinfo = DNTRequest.GetString("searchinf");
            if (searchinfo != "")
            {
                userListTable = Forum.Users.GetSearchUserList(searchinfo);
            }
            else
            { 
                userListTable = "您未输入任何搜索关键字"; 
            }
        }
    }
}