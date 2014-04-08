using System;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Plugin.Mall;

namespace Discuz.Web.Admin
{
    public class usergrouppowersetting : System.Web.UI.UserControl
    {
        protected Discuz.Control.CheckBoxList usergroupright;
        protected Discuz.Control.RadioButtonList allowavatar;
        protected Discuz.Control.RadioButtonList allowsearch;
        protected Discuz.Control.RadioButtonList reasonpm;
        protected System.Web.UI.WebControls.Literal outscript;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (MallPluginProvider.GetInstance() == null)
                {
                    usergroupright.Items.RemoveAt(usergroupright.Items.Count - 1);  //最后一项总是为是否允许交易
                }
            }
        }

        public void Bind(UserGroupInfo usergroupinfo)
        {
            if (usergroupinfo.Allowsearch.ToString() == "0") allowsearch.Items[0].Selected = true;
            if (usergroupinfo.Allowsearch.ToString() == "1") allowsearch.Items[1].Selected = true;
            if (usergroupinfo.Allowsearch.ToString() == "2") allowsearch.Items[2].Selected = true;

            //if (usergroupinfo.Allowavatar >= 0) allowavatar.Items[usergroupinfo.Allowavatar].Selected = true;
            reasonpm.Items[usergroupinfo.Reasonpm].Selected = true;

            if (usergroupinfo.Allowvisit == 1) usergroupright.Items[0].Selected = true; //是否允许访问论坛
            if (usergroupinfo.Allowpost == 1) usergroupright.Items[1].Selected = true; //是否允许发帖
            if (usergroupinfo.Allowreply == 1) usergroupright.Items[2].Selected = true; //是否允许回复
            if (usergroupinfo.Allowpostpoll == 1) usergroupright.Items[3].Selected = true; //是否允许发起投票
            if (usergroupinfo.Allowvote == 1) usergroupright.Items[4].Selected = true; //是否允许参与投票
            if (usergroupinfo.Allowpostattach == 1) usergroupright.Items[5].Selected = true; //是否发布附件
            if (usergroupinfo.Allowgetattach == 1) usergroupright.Items[6].Selected = true; //是否允许下载附件
            if (usergroupinfo.Allowsetreadperm == 1) usergroupright.Items[7].Selected = true; //是否允许设置主题阅读积分权限
            if (usergroupinfo.Allowsetattachperm == 1) usergroupright.Items[8].Selected = true; //是否允许设置附件阅读积分限制
            if (usergroupinfo.Allowhidecode == 1) usergroupright.Items[9].Selected = true; //是否允许使用hide代码
            if (usergroupinfo.Allowcusbbcode == 1) usergroupright.Items[10].Selected = true; //是否允许使用Discuz!NT代码
            if (usergroupinfo.Allowsigbbcode == 1) usergroupright.Items[11].Selected = true; //签名是否支持Discuz!NT代码
            if (usergroupinfo.Allowsigimgcode == 1) usergroupright.Items[12].Selected = true; //签名是否支持图片代码
            if (usergroupinfo.Allowviewpro == 1) usergroupright.Items[13].Selected = true; //是否允许查看用户资料
            if (usergroupinfo.Disableperiodctrl == 1) usergroupright.Items[14].Selected = true; //是否不受时间段限制
            if (usergroupinfo.Allowdebate == 1) usergroupright.Items[15].Selected = true; //是否允许辩论
            if (usergroupinfo.Allowbonus == 1) usergroupright.Items[16].Selected = true; //是否允许悬赏
            if (usergroupinfo.Allowviewstats == 1) usergroupright.Items[17].Selected = true; //是否允许查看统计数据
            if (usergroupinfo.Allowdiggs == 1) usergroupright.Items[18].Selected = true; //是否允许辩论支持
            if (usergroupinfo.Allowhtmltitle == 1) usergroupright.Items[19].Selected = true;//是否允许html标题
            if (usergroupinfo.Allowhtml == 1) usergroupright.Items[20].Selected = true; //是否允许html
            if (usergroupinfo.ModNewTopics == 1) usergroupright.Items[21].Selected = true;//发主题是否需要审核
            if (usergroupinfo.ModNewPosts == 1) usergroupright.Items[22].Selected = true;//发回复是否需要审核
            if (usergroupinfo.Ignoreseccode == 1) usergroupright.Items[23].Selected = true;//是否允许忽略检测验证码
            if (MallPluginProvider.GetInstance() != null && usergroupinfo.Allowtrade == 1) usergroupright.Items[usergroupright.Items.Count - 1].Selected = true; //是否允许交易

            string strScript = "<script type='text/javascript'>\r\nfunction insertBonusPrice()\r\n{\r\n\t";
            strScript += "\r\n\tvar tdelement = document.getElementById('" + usergroupright.ClientID + "_16').parentNode;";
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;最低悬赏价格:<input type=\"text\" name=\"minbonusprice\" id=\"minbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"" + usergroupinfo.Minbonusprice + "\"" + (usergroupinfo.Allowbonus == 0 ? " disabled=\"disabled \"" : "") + " />'";
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;最高悬赏价格:<input type=\"text\" name=\"maxbonusprice\" id=\"maxbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"" + usergroupinfo.Maxbonusprice + "\"" + (usergroupinfo.Allowbonus == 0 ? " disabled=\"disabled \"" : "") + " />'";
            strScript += "\r\n}\r\ninsertBonusPrice();\r\n</script>\r\n";
            outscript.Text = strScript;
            usergroupright.Items[16].Attributes.Add("onclick", "bonusPriceSet(this.checked)");
        }

        public void Bind()
        {
            allowsearch.Items[0].Selected = true;
            //allowavatar.Items[0].Selected = true;
            reasonpm.Items[0].Selected = true;
            for(int i = 0 ; i < usergroupright.Items.Count ; i++)
            {
                usergroupright.Items[i].Selected = false;
            }
            string strScript = "<script type='text/javascript'>\r\nfunction insertBonusPrice()\r\n{\r\n\t";
            strScript += "\r\n\tvar tdelement = document.getElementById('" + usergroupright.ClientID + "_16').parentNode;";
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;最低悬赏价格:<input type=\"text\" name=\"minbonusprice\" id=\"minbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"10\" disabled=\"disabled\" />'";
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;最高悬赏价格:<input type=\"text\" name=\"maxbonusprice\" id=\"maxbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"20\" disabled=\"disabled\" />'";
            strScript += "\r\n}\r\ninsertBonusPrice();\r\n</script>\r\n";
            outscript.Text = strScript;
            usergroupright.Items[16].Attributes.Add("onclick", "bonusPriceSet(this.checked)");
        }

        public void GetSetting(ref UserGroupInfo usergroupinfo)
        {
            usergroupinfo.Allowsearch = Convert.ToInt32(allowsearch.SelectedValue);
            //usergroupinfo.Allowavatar = Convert.ToInt32(allowavatar.SelectedValue);
            usergroupinfo.Reasonpm = Convert.ToInt32(reasonpm.SelectedValue);

            usergroupinfo.Allowvisit = usergroupright.Items[0].Selected ? 1 : 0; //是否允许访问论坛
            usergroupinfo.Allowpost = usergroupright.Items[1].Selected ? 1 : 0; //是否允许发帖
            usergroupinfo.Allowreply = usergroupright.Items[2].Selected ? 1 : 0; //是否允许回复
            usergroupinfo.Allowpostpoll = usergroupright.Items[3].Selected ? 1 : 0; //是否允许发起投票
            usergroupinfo.Allowvote = usergroupright.Items[4].Selected ? 1 : 0; //是否允许参与投票
            usergroupinfo.Allowpostattach = usergroupright.Items[5].Selected ? 1 : 0; //是否发布附件
            usergroupinfo.Allowgetattach = usergroupright.Items[6].Selected ? 1 : 0; //是否允许下载附件
            usergroupinfo.Allowsetreadperm = usergroupright.Items[7].Selected ? 1 : 0; //是否允许设置主题阅读积分权限
            usergroupinfo.Allowsetattachperm = usergroupright.Items[8].Selected ? 1 : 0; //是否允许设置附件阅读积分限制
            usergroupinfo.Allowhidecode = usergroupright.Items[9].Selected ? 1 : 0; //是否允许使用hide代码
            usergroupinfo.Allowcusbbcode = usergroupright.Items[10].Selected ? 1 : 0; //是否允许使用Discuz!NT代码
            usergroupinfo.Allowsigbbcode = usergroupright.Items[11].Selected ? 1 : 0; //签名是否支持Discuz!NT代码
            usergroupinfo.Allowsigimgcode = usergroupright.Items[12].Selected ? 1 : 0; //签名是否支持图片代码
            usergroupinfo.Allowviewpro = usergroupright.Items[13].Selected ? 1 : 0; //是否允许查看用户资料
            usergroupinfo.Disableperiodctrl = usergroupright.Items[14].Selected ? 1 : 0; //是否不受时间段限制

            usergroupinfo.Allowdebate = usergroupright.Items[15].Selected ? 1 : 0; //是否允许辩论
            usergroupinfo.Allowbonus = usergroupright.Items[16].Selected ? 1 : 0; //是否允许悬赏
            //如果勾选允许悬赏
            if (usergroupright.Items[16].Selected)
            {
                usergroupinfo.Minbonusprice = DNTRequest.GetInt("minbonusprice", 0);
                usergroupinfo.Maxbonusprice = DNTRequest.GetInt("maxbonusprice", 0);
            }
            else
            {
                usergroupinfo.Minbonusprice = 0;
                usergroupinfo.Maxbonusprice = 0;
            }
            usergroupinfo.Allowviewstats = usergroupright.Items[17].Selected ? 1 : 0; //是否允许查看统计数据
            usergroupinfo.Allowdiggs = usergroupright.Items[18].Selected ? 1 : 0;   //是否允许辩论支持
            usergroupinfo.Allowhtmltitle = usergroupright.Items[19].Selected ? 1 : 0;//是否允许html标题
            usergroupinfo.Allowhtml = usergroupright.Items[20].Selected ? 1 : 0;    //是否允许html
            usergroupinfo.ModNewTopics = usergroupright.Items[21].Selected ? 1 : 0; //发主题需审核
            usergroupinfo.ModNewPosts = usergroupright.Items[22].Selected ? 1 : 0; //发回复需审核
            usergroupinfo.Ignoreseccode = usergroupright.Items[23].Selected ? 1 : 0;//是否允许忽略检测验证码
            if (MallPluginProvider.GetInstance() != null)
            {
                usergroupinfo.Allowtrade = usergroupright.Items[usergroupright.Items.Count - 1].Selected ? 1 : 0;   //是否允许交易
            }
        }
        
    }
}