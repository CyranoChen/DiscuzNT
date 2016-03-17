using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using System.Web.UI;
using Discuz.Common;
using System.Web;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Plugin.Spread
{
    public class MainPage : Page
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public MainPage()
        {
            int uid = DNTRequest.GetInt("uid", -1);
            string transferurl = Config.SpreadConfigs.GetConfig().TransferUrl;

            if (uid < 1)
            {
                HttpContext.Current.Response.Redirect(transferurl == string.Empty ? BaseConfigs.GetForumPath : transferurl, true);
                return;
            }

            
            if (Utils.GetCookie("spread") != "")
                HttpContext.Current.Response.Redirect(transferurl == string.Empty ? BaseConfigs.GetForumPath : transferurl, true);
            //过滤用户，不得重复加分
            ShortUserInfo user = Users.GetShortUserInfo(uid);
            if (user == null || user.Uid < 1)
            {
                HttpContext.Current.Response.Redirect(transferurl == string.Empty ? BaseConfigs.GetForumPath : transferurl, true);
                return;
            }
            if (DNTRequest.GetIP() == user.Lastip)
            {
                HttpContext.Current.Response.Redirect(transferurl == string.Empty ? BaseConfigs.GetForumPath : transferurl, true);
            }
            
            
            Utils.WriteCookie("spread", uid.ToString(), 24*60);
            

            string credits = Config.SpreadConfigs.GetConfig().SpreadCredits;
            //设置用户的积分
            ///首先读取版块内自定义积分
            ///版设置了自定义积分则使用，否则使用论坛默认积分
            float[] values = null;
            if (!credits.Equals(""))
            {
                int index = 0;
                float tempval = 0;
                values = new float[8];
                foreach (string ext in Utils.SplitString(credits, ","))
                {

                    //if (index == 0)
                    //{
                    //    if (!ext.Equals("True"))
                    //    {
                    //        values = null;
                    //        break;
                    //    }
                    //    index++;
                    //    continue;
                    //}
                    tempval = Utils.StrToFloat(ext, 0);
                    values[index] = tempval;
                    index++;
                    if (index > 8)
                    {
                        break;
                    }
                }
            }
            if (values != null)
            {

                Forum.UserCredits.UpdateUserExtCredits(uid.ToString(), values);
            }
            
            HttpContext.Current.Response.Redirect(transferurl == string.Empty ? BaseConfigs.GetForumPath : transferurl, true);
        }
    }
}
