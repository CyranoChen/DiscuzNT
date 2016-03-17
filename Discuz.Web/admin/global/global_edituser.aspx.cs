using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑用户
    /// </summary>
    public partial class edituser : AdminPage
    {
        public UserInfo userInfo = new UserInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!AllowEditUser(this.userid, DNTRequest.GetInt("uid", -1)))
                {
                    Response.Write("<script>alert('非创始人身份不能修改其它管理员的信息!');window.location.href='global_usergrid.aspx';</script>");
                    Response.End();
                    return;
                }
                IsEditUserName.Attributes.Add("onclick", "document.getElementById('" + userName.ClientID + "').disabled = !document.getElementById('" + IsEditUserName.ClientID + "').checked;");
            }
        }

        private bool AllowEditUser(int managerUId, int targetUId)
        {
            #region 是否可以编辑用户
            int managerGroupId = Users.GetUserInfo(managerUId).Groupid;
            if (Users.GetUserInfo(managerUId).Adminid == 0)
            {
                return false;
            }
            int targetGroupId = Users.GetUserInfo(targetUId).Groupid;
            int founderUId = BaseConfigs.GetBaseConfig().Founderuid;
            if (managerUId == targetUId)    //可自身修改
                return true;
            else if (managerUId == founderUId)  //创始人可修改
                return true;
            else if (managerGroupId == targetGroupId)   //管理组相同的不能修改
                return false;
            else
                return true;
            #endregion
        }

        public bool AllowEditUserInfo(int uid, bool redirect)
        {
            #region 是否允许编辑用户信息

            if ((BaseConfigs.GetBaseConfig().Founderuid == uid) && (uid == this.userid))
                return true;
            if (BaseConfigs.GetBaseConfig().Founderuid != uid) //当要编辑的用户信息不是创建人的信息时
                return true;
            if (redirect)
            {
                base.RegisterStartupScript("", "<script>alert('您要编辑信息是论坛创始人信息,请您以创始人身份登陆后台才能修改!');</script>");
            }
            return false;

            #endregion
        }

        public bool IsValidScoreName(int scoreid)
        {
            #region 是否是有效的积分名称

            bool isvalid = false;

            foreach (DataRow dr in Scoresets.GetScoreSet().Rows)
            {
                if ((dr["id"].ToString() != "1") && (dr["id"].ToString() != "2"))
                {
                    if (dr[scoreid + 1].ToString().Trim() != "0")
                    {
                        isvalid = true;
                        break;
                    }
                }
            }
            return isvalid;

            #endregion
        }

        public void LoadScoreInf(string fid, string fieldname)
        {
            #region 加载积分信息

            DataRow dr = Scoresets.GetScoreSet().Rows[0];
            if (dr[2].ToString().Trim() != "")
            {
                extcredits1name.Text = dr[2].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(1))
                {
                    extcredits1.Enabled = false;
                }
            }

            if (dr[3].ToString().Trim() != "")
            {
                extcredits2name.Text = dr[3].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(2))
                {
                    extcredits2.Enabled = false;
                }
            }


            if (dr[4].ToString().Trim() != "")
            {
                extcredits3name.Text = dr[4].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(3))
                {
                    extcredits3.Enabled = false;
                }
            }


            if (dr[5].ToString().Trim() != "")
            {
                extcredits4name.Text = dr[5].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(4))
                {
                    extcredits4.Enabled = false;
                }
            }


            if (dr[6].ToString().Trim() != "")
            {
                extcredits5name.Text = dr[6].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(5))
                {
                    extcredits5.Enabled = false;
                }
            }


            if (dr[7].ToString().Trim() != "")
            {
                extcredits6name.Text = dr[7].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(6))
                {
                    extcredits6.Enabled = false;
                }
            }


            if (dr[8].ToString().Trim() != "")
            {
                extcredits7name.Text = dr[8].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(7))
                {
                    extcredits7.Enabled = false;
                }
            }


            if (dr[9].ToString().Trim() != "")
            {
                extcredits8name.Text = dr[9].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(8))
                {
                    extcredits8.Enabled = false;
                }
            }

            lblScoreCalFormula.Text = Scoresets.GetScoreCalFormula();
            #endregion
        }

        public void LoadCurrentUserInfo(int uid)
        {
            #region 加载相关信息

            //userInfo = AdminUsers.GetUserInfo(uid);

            ViewState["username"] = userInfo.Username;
            userName.Text = userInfo.Username;

            //只有在当前用户为等待验证用户, 且系统论坛设置为1时才会显示重发EMAIL按钮
            if ((userInfo.Groupid == 8) && (config.Regverify == 1)) ReSendEmail.Visible = true;
            else ReSendEmail.Visible = false;

            nickname.Text = userInfo.Nickname;
            accessmasks.SelectedValue = userInfo.Accessmasks.ToString();
            bday.Text = userInfo.Bday.Trim();
            credits.Text = userInfo.Credits.ToString();
            digestposts.Text = userInfo.Digestposts.ToString();
            email.Text = userInfo.Email.Trim();
            gender.SelectedValue = userInfo.Gender.ToString();
            //groupexpiry.Text = userInfo.Groupexpiry.ToString();

            if (userInfo.Groupid.ToString() == "")
            {
                groupid.SelectedValue = "0";
            }
            else
            {
                if (groupid.Items.FindByValue(userInfo.Groupid.ToString()) != null)
                {
                    groupid.SelectedValue = userInfo.Groupid.ToString();
                }
                else
                {
                    groupid.SelectedValue = UserCredits.GetCreditsUserGroupId(userInfo.Credits).Groupid.ToString();
                }
            }

            if (uid == BaseConfigs.GetFounderUid)
            {
                groupid.Enabled = false;
            }

            if (userInfo.Groupid == 4)
            {
                StopTalk.Text = "取消禁言";
                StopTalk.HintInfo = "取消禁言将会把当前用户所在的 \\'系统禁言\\' 组进行系统调整成为非禁言组";
            }

            ViewState["Groupid"] = userInfo.Groupid.ToString();

            invisible.SelectedValue = userInfo.Invisible.ToString();
            joindate.Text = userInfo.Joindate.ToString();
            lastactivity.Text = userInfo.Lastactivity.ToString();
            lastip.Text = userInfo.Lastip.Trim();
            lastpost.Text = userInfo.Lastpost.ToString();
            lastvisit.Text = userInfo.Lastvisit;
            newpm.SelectedValue = userInfo.Newpm.ToString();
            switch (userInfo.Newsletter)
            {
                case ReceivePMSettingType.ReceiveNone:
                    SetNewsLetter(false, false, false);
                    break;
                case ReceivePMSettingType.ReceiveSystemPM:
                    SetNewsLetter(true, false, false);
                    break;
                case ReceivePMSettingType.ReceiveUserPM:
                    SetNewsLetter(false, true, false);
                    break;
                case ReceivePMSettingType.ReceiveAllPM:
                    SetNewsLetter(true, true, false);
                    break;
                case ReceivePMSettingType.ReceiveSystemPMWithHint:
                    SetNewsLetter(true, false, true);
                    break;
                case ReceivePMSettingType.ReceiveUserPMWithHint:
                    SetNewsLetter(false, true, true);
                    break;
                default:
                    SetNewsLetter(true, true, true);
                    break;
            }
            oltime.Text = userInfo.Oltime.ToString();
            pageviews.Text = userInfo.Pageviews.ToString();
            pmsound.Text = userInfo.Pmsound.ToString();
            posts.Text = userInfo.Posts.ToString();
            ppp.Text = userInfo.Ppp.ToString();
            regip.Text = userInfo.Regip.Trim();

            showemail.SelectedValue = userInfo.Showemail.ToString();
            sigstatus.SelectedValue = userInfo.Sigstatus.ToString();

            if ((userInfo.Templateid.ToString() != "") && (userInfo.Templateid.ToString() != "0"))
            {
                templateid.SelectedValue = userInfo.Templateid.ToString();
            }

            tpp.Text = userInfo.Tpp.ToString();

            extcredits1.Text = userInfo.Extcredits1.ToString();
            extcredits2.Text = userInfo.Extcredits2.ToString();
            extcredits3.Text = userInfo.Extcredits3.ToString();
            extcredits4.Text = userInfo.Extcredits4.ToString();
            extcredits5.Text = userInfo.Extcredits5.ToString();
            extcredits6.Text = userInfo.Extcredits6.ToString();
            extcredits7.Text = userInfo.Extcredits7.ToString();
            extcredits8.Text = userInfo.Extcredits8.ToString();


            //用户扩展信息
            website.Text = userInfo.Website;
            icq.Text = userInfo.Icq;
            qq.Text = userInfo.Qq;
            yahoo.Text = userInfo.Yahoo;
            msn.Text = userInfo.Msn;
            skype.Text = userInfo.Skype;
            location.Text = userInfo.Location;
            customstatus.Text = userInfo.Customstatus;
            //avatar.Text = userInfo.Avatar;
            //avatarheight.Text = userInfo.Avatarheight.ToString();
            //avatarwidth.Text = userInfo.Avatarwidth.ToString();
            bio.Text = userInfo.Bio;
            signature.Text = userInfo.Signature;
            realname.Text = userInfo.Realname;
            idcard.Text = userInfo.Idcard;
            mobile.Text = userInfo.Mobile;
            phone.Text = userInfo.Phone;

            givenusername.Text = userInfo.Username;

            if (userInfo.Medals.Trim() == "")
            {
                userInfo.Medals = "0";
            }

            string begivenmedals = "," + userInfo.Medals + ",";
            DataTable dt = Medals.GetAvailableMedal();

            if (dt != null)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = "isgiven";
                dc.DataType = Type.GetType("System.Boolean");
                dc.DefaultValue = false;
                dc.AllowDBNull = false;
                dt.Columns.Add(dc);

                foreach (DataRow dr in dt.Rows)
                {
                    if (begivenmedals.IndexOf("," + dr["medalid"].ToString() + ",") >= 0)
                    {
                        dr["isgiven"] = true;
                    }
                }
                medalslist.DataSource = dt;
                medalslist.DataBind();
            }

            #endregion
        }

        private void SetNewsLetter(bool item1, bool item2, bool item3)
        {
            newsletter.Items[0].Selected = item2;
            newsletter.Items[1].Selected = item3;

            if (!item2)
            {
                newsletter.Items[1].Selected = false;
                newsletter.Items[1].Enabled = false;
            }
        }

        private int GetNewsLetter()
        {
            int item2 = 0;
            int item3 = 0;

            if (newsletter.Items[0].Selected)
            {
                item2 = 2;
            }
            if (newsletter.Items[1].Selected)
            {
                item3 = 4;
            }

            return item2 | item3;
        }

        private void IsEditUserName_CheckedChanged(object sender, EventArgs e)
        {
            #region 是否可以编辑用户名
            if (IsEditUserName.Checked)
            {
                userName.Enabled = true;
            }
            else
            {
                userName.Enabled = false;
            }
            #endregion
        }

        public string BeGivenMedal(string isgiven, string medalid)
        {
            #region 勋章的显示方式

            if (isgiven == "True")
            {
                return "<INPUT id=\"medalid\"  type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\" checked>";
            }
            else
            {
                return "<INPUT id=\"medalid\"  type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\">";
            }

            #endregion
        }


        private void GivenMedal_Click(object sender, EventArgs e)
        {
            #region 给予勋章

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);
                GivenUserMedal(uid);

                if (DNTRequest.GetString("codition") == "")
                {
                    Session["codition"] = null;
                }
                else
                {
                    Session["codition"] = DNTRequest.GetString("codition").Replace("^", "'");
                }

                base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }

        private void GivenUserMedal(int uid)
        {
            Users.UpdateMedals(uid, DNTRequest.GetString("medalid"), userid, username, DNTRequest.GetIP(), reason.Text.Trim());
        }

        private void ResetUserDigestPost_Click(object sender, EventArgs e)
        {
            #region 重设用户精华帖

            if (this.CheckCookie())
            {
                AdminForumStats.ReSetUserDigestPosts(DNTRequest.GetInt("uid", -1), DNTRequest.GetInt("uid", -1));
                base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + userInfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }


        private void ResetUserPost_Click(object sender, EventArgs e)
        {
            #region 重设用户发帖

            if (this.CheckCookie())
            {
                AdminForumStats.ReSetUserPosts(DNTRequest.GetInt("uid", -1), DNTRequest.GetInt("uid", -1));
                base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + userInfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }


        private void ResetPassWord_Click(object sender, EventArgs e)
        {
            #region 重设用户密码

            if (this.CheckCookie())
            {
                if (!AllowEditUserInfo(DNTRequest.GetInt("uid", -1), true)) return;

                Response.Redirect("global_resetpassword.aspx?uid=" + DNTRequest.GetString("uid"));
            }

            #endregion
        }


        private void StopTalk_Click(object sender, EventArgs e)
        {
            #region 设置禁言

            if (this.CheckCookie())
            {
                userInfo = AdminUsers.GetUserInfo(DNTRequest.GetInt("uid", -1));

                if (!AllowEditUserInfo(DNTRequest.GetInt("uid", -1), true)) return;

                if (ViewState["Groupid"].ToString() != "4") //当用户不是系统禁言组时
                {
                    if (userInfo.Uid > 1) //判断是不是当前uid是不是系统初始化时生成的uid
                    {
                        if (AlbumPluginProvider.GetInstance() != null)
                            AlbumPluginProvider.GetInstance().Ban(userInfo.Uid);
                        if (SpacePluginProvider.GetInstance() != null)
                            SpacePluginProvider.GetInstance().Ban(userInfo.Uid);
                        Users.UpdateUserToStopTalkGroup(userInfo.Uid.ToString());
                        base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + userInfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,你要禁言的用户是系统初始化时的用户,因此不能操作!');window.location.href='global_edituser.aspx?uid=" + userInfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';</script>");
                    }
                }
                else
                {
                    if (UserCredits.GetCreditsUserGroupId(0) != null)
                    {
                        int tmpGroupID = UserCredits.GetCreditsUserGroupId(userInfo.Credits).Groupid;
                        Users.UpdateUserGroup(userInfo.Uid, tmpGroupID);
                        base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + userInfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,系统未能找到合适的用户组来调整当前用户所处的组!');window.location.href='global_edituser.aspx?uid=" + userInfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';</script>");
                    }
                }
                OnlineUsers.DeleteUserByUid(userInfo.Uid);
            }

            #endregion
        }


        private void DelPosts_Click(object sender, EventArgs e)
        {
            #region 删除用户帖

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);

                if (!AllowEditUserInfo(uid, true)) return;

                //清除用户所发的帖子
                Posts.ClearPosts(uid, 0);
                //foreach (DataRow dr in Posts.GetAllPostTableName().Rows)
                //{
                //    if (dr["id"].ToString() != "")
                //    {
                //        Posts.DeletePostByPosterid(int.Parse(dr["id"].ToString()), uid);
                //    }
                //}
                //Topics.DeleteTopicByPosterid(uid);
                //Users.ClearPosts(uid);
                base.RegisterStartupScript("", "<script>alert('请到 论坛维护->论坛数据维护->重建指定主题区间帖数 对出现因为该操作产生\"读取信息失败\"的主题进行修复 ')</script>");
                base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }

        private void ReSendEmail_Click(object sender, EventArgs e)
        {
            #region 发送EMAIL

            string authstr = ForumUtils.CreateAuthStr(20);
            Emails.DiscuzSmtpMail(userName.Text, email.Text, "", authstr);
            string uid = DNTRequest.GetString("uid");
            //DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [Authstr]='" + authstr + "' , [Authtime]='" + DateTime.Now.ToString() + "' ,[Authflag]=1  WHERE [uid]=" + uid);
            Users.UpdateEmailValidateInfo(authstr, DateTime.Now, int.Parse(uid));
            base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + uid + "&condition=" + DNTRequest.GetString("condition") + "';");

            #endregion
        }

        private void SaveUserInfo_Click(object sender, EventArgs e)
        {
            #region 保存用户信息

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);
                string errorInfo = "";

                if (!AllowEditUserInfo(uid, true)) return;

                if (userName.Text != ViewState["username"].ToString())
                {
                    if (AdminUsers.GetUserId(userName.Text) > 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('您所输入的用户名已被使用过, 请输入其他的用户名!');</script>");
                        return;
                    }
                }

                if (userName.Text == "")
                {
                    base.RegisterStartupScript("", "<script>alert('用户名不能为空!');</script>");
                    return;
                }

                if (groupid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何用户组!');</script>");
                    return;
                }

                userInfo = AdminUsers.GetUserInfo(uid);
                userInfo.Username = userName.Text;
                userInfo.Nickname = nickname.Text;
                userInfo.Accessmasks = Convert.ToInt32(accessmasks.SelectedValue);

                //当用户组发生变化时则相应更新用户的管理组字段
                if (userInfo.Groupid.ToString() != groupid.SelectedValue)
                {
                    userInfo.Adminid = UserGroups.GetUserGroupInfo(int.Parse(groupid.SelectedValue)).Radminid;
                }

                //userInfo.Avatarshowid = 0;

                if ((bday.Text == "0000-00-00") || (bday.Text == "0000-0-0") | (bday.Text.Trim() == ""))
                {
                    userInfo.Bday = "";
                }
                else
                {
                    if (!Utils.IsDateString(bday.Text.Trim()))
                    {
                        base.RegisterStartupScript("", "<script>alert('用户生日不是有效的日期型数据!');</script>");
                        return;
                    }
                    else
                    {
                        userInfo.Bday = bday.Text;
                    }
                }


                if (!Users.ValidateEmail(email.Text, uid))
                {
                    base.RegisterStartupScript("", "<script>alert('当前用户的邮箱地址已被使用过, 请输入其他的邮箱!');</script>");
                    return;
                }

                userInfo.Email = email.Text;
                userInfo.Gender = Convert.ToInt32(gender.SelectedValue);
                //userInfo.Groupexpiry = Convert.ToInt32(groupexpiry.Text);后台操作为永久禁言和永久禁访

                userInfo.Groupexpiry = 0;
                userInfo.Extgroupids = extgroupids.GetSelectString(",");

                if ((groupid.SelectedValue != "1") && (userInfo.Uid == BaseConfigs.GetFounderUid))
                {
                    base.RegisterStartupScript("", "<script>alert('创始人的所属用户组不能被修改为其它组!');window.location.href='global_edituser.aspx?uid=" + DNTRequest.GetString("uid") + "';</script>");
                    return;
                }

                userInfo.Groupid = Convert.ToInt32(groupid.SelectedValue);
                userInfo.Invisible = Convert.ToInt32(invisible.SelectedValue);
                userInfo.Joindate = joindate.Text;
                userInfo.Lastactivity = lastactivity.Text;
                userInfo.Lastip = lastip.Text;
                userInfo.Lastpost = lastpost.Text;
                userInfo.Lastvisit = lastvisit.Text;
                userInfo.Newpm = Convert.ToInt32(newpm.SelectedValue);
                userInfo.Newsletter = (ReceivePMSettingType)GetNewsLetter();
                userInfo.Oltime = Convert.ToInt32(oltime.Text);
                userInfo.Pageviews = Convert.ToInt32(pageviews.Text);
                userInfo.Pmsound = Convert.ToInt32(pmsound.Text);
                userInfo.Posts = Convert.ToInt32(posts.Text);
                userInfo.Ppp = Convert.ToInt32(ppp.Text);
                userInfo.Regip = regip.Text;
                userInfo.Digestposts = Convert.ToInt32(digestposts.Text);

                if (secques.SelectedValue == "1") userInfo.Secques = ""; //清空安全码

                userInfo.Showemail = Convert.ToInt32(showemail.SelectedValue);
                userInfo.Sigstatus = Convert.ToInt32(sigstatus.SelectedValue);
                userInfo.Templateid = Convert.ToInt32(templateid.SelectedValue);
                userInfo.Tpp = Convert.ToInt32(tpp.Text);


                if (Utils.IsNumeric(extcredits1.Text.Replace("-", "")))
                {
                    userInfo.Extcredits1 = float.Parse(extcredits1.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits2.Text.Replace("-", "")))
                {
                    userInfo.Extcredits2 = float.Parse(extcredits2.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits3.Text.Replace("-", "")))
                {
                    userInfo.Extcredits3 = float.Parse(extcredits3.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits4.Text.Replace("-", "")))
                {
                    userInfo.Extcredits4 = float.Parse(extcredits4.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits5.Text.Replace("-", "")))
                {
                    userInfo.Extcredits5 = float.Parse(extcredits5.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits6.Text.Replace("-", "")))
                {
                    userInfo.Extcredits6 = float.Parse(extcredits6.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits7.Text.Replace("-", "")))
                {
                    userInfo.Extcredits7 = float.Parse(extcredits7.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits8.Text.Replace("-", "")))
                {
                    userInfo.Extcredits8 = float.Parse(extcredits8.Text);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }

                
                //根据公式计算用户的总积分,并更新
                userInfo.Credits = UserCredits.GetUserCreditsByUserInfo(userInfo);
                //判断用户组是否为积分用户组。如果是的话，就用当前积分更新。
                if (UserGroups.IsCreditUserGroup(userInfo.Groupid))
                {
                    userInfo.Groupid = UserCredits.GetCreditsUserGroupId(userInfo.Credits).Groupid;
                }
                //用户扩展信息
                userInfo.Website = website.Text;
                userInfo.Icq = icq.Text;
                userInfo.Qq = qq.Text;
                userInfo.Yahoo = yahoo.Text;
                userInfo.Msn = msn.Text;
                userInfo.Skype = skype.Text;
                userInfo.Location = location.Text;
                userInfo.Customstatus = customstatus.Text;
                //userInfo.Avatar = avatar.Text;
                //userInfo.Avatarheight = Convert.ToInt32(avatarheight.Text);
                //userInfo.Avatarwidth = Convert.ToInt32(avatarwidth.Text);
                userInfo.Bio = bio.Text;
                if (signature.Text.Length > UserGroups.GetUserGroupInfo(userInfo.Groupid).Maxsigsize)
                {
                    errorInfo = "更新的签名长度超过 " + UserGroups.GetUserGroupInfo(userInfo.Groupid).Maxsigsize + " 字符的限制，未能更新。";
                }
                else
                {
                    userInfo.Signature = signature.Text;
                    //签名UBB转换HTML
                    PostpramsInfo _postpramsinfo = new PostpramsInfo();
                    _postpramsinfo.Showimages = UserGroups.GetUserGroupInfo(userInfo.Groupid).Allowsigimgcode;
                    _postpramsinfo.Sdetail = signature.Text;
                    userInfo.Sightml = UBB.UBBToHTML(_postpramsinfo);
                }

                userInfo.Realname = realname.Text;
                userInfo.Idcard = idcard.Text;
                userInfo.Mobile = mobile.Text;
                userInfo.Phone = phone.Text;
                userInfo.Medals = DNTRequest.GetString("medalid");

                if (IsEditUserName.Checked && userName.Text != ViewState["username"].ToString())
                {
                    AdminUsers.UserNameChange(userInfo, ViewState["username"].ToString());
                    //用户重命名同步
                    Discuz.Forum.Sync.RenameUser(userInfo.Uid, ViewState["username"].ToString(), userInfo.Username, "");
                }

                if (AdminUsers.UpdateUserAllInfo(userInfo))
                {
                    OnlineUsers.DeleteUserByUid(userInfo.Uid);    //移除该用户的在线信息，使之重建在线表信息
                    if (ViewState["Groupid"].ToString() != userInfo.Groupid.ToString())
                    {
                        if (userInfo.Groupid == 4)
                        {
                            if (AlbumPluginProvider.GetInstance() != null)
                            {
                                AlbumPluginProvider.GetInstance().Ban(userInfo.Uid);
                            }
                            if (SpacePluginProvider.GetInstance() != null)
                            {
                                SpacePluginProvider.GetInstance().Ban(userInfo.Uid);
                            }
                        }
                        else
                        {
                            if (AlbumPluginProvider.GetInstance() != null)
                            {
                                AlbumPluginProvider.GetInstance().UnBan(userInfo.Uid);
                            }
                            if (SpacePluginProvider.GetInstance() != null)
                            {
                                SpacePluginProvider.GetInstance().UnBan(userInfo.Uid);
                            }
                        }
                    }
                    //if (userName.Text != ViewState["username"].ToString())
                    //{
                    //    AdminUsers.UserNameChange(userInfo, ViewState["username"].ToString());
                    //}
                    //删除头像
                    if (delavart.Checked)
                        Avatars.DeleteAvatar(userInfo.Uid.ToString());
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台编辑用户", "用户名:" + userName.Text);
                    if (errorInfo == "")
                    {
                        base.RegisterStartupScript("PAGE", "window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript("PAGE", "alert('" + errorInfo + "');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                    }
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';</script>");
                }
            }

            #endregion
        }

        private void DelUserInfo_Click(object sender, EventArgs e)
        {
            #region 删除指定用户信息

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);

                if (!AllowEditUserInfo(uid, true)) return;

                if (AllowDeleteUser(this.userid, uid))
                {
                    bool delpost = !(deltype.SelectedValue.IndexOf("1") >= 0);
                    bool delpms = !(deltype.SelectedValue.IndexOf("2") >= 0);

                    if (SpacePluginProvider.GetInstance() != null)
                    {
                        SpacePluginProvider.GetInstance().Delete(uid);
                    }

                    if (AlbumPluginProvider.GetInstance() != null)
                    {
                        AlbumPluginProvider.GetInstance().Delete(uid);
                    }
                    if (AdminUsers.DelUserAllInf(uid, delpost, delpms))
                    {
                        //删除用户同步
                        Discuz.Forum.Sync.DeleteUsers(uid.ToString(), "");
                        //ManyouApplications.AddUserLog(uid, UserLogActionEnum.Delete);
                        //删除该用户头像
                        Avatars.DeleteAvatar(uid.ToString());
                        AdminUsers.UpdateForumsFieldModerators(userName.Text);

                        OnlineUsers.DeleteUserByUid(userInfo.Uid);    //移除该用户的在线信息，使之退出
                        AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户", "用户名:" + userName.Text);
                        base.RegisterStartupScript("PAGE", "window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';</script>");
                    }
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,你要删除的用户是创始人用户或是其它管理员,因此不能删除!');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';</script>");
                }
            }

            #endregion
        }

        private bool AllowDeleteUser(int managerUId, int byDeleterUId)
        {
            #region 判断将要删除的用户是否是创始人
            int managerGroupId = Users.GetUserInfo(managerUId).Groupid;
            int byDeleterGruopid = Users.GetUserInfo(byDeleterUId).Groupid;
            int founderUId = BaseConfigs.GetBaseConfig().Founderuid;
            if (byDeleterUId == founderUId) //判断被删除人是否为创始人
            {
                return false;
            }
            else if (managerUId != founderUId && managerGroupId == byDeleterGruopid)    //判断被删除人是否为相同组，即是否都是管理员，管理员不能相互删除
            {
                return false;
            }
            else
            {
                return true;
            }
            #endregion
        }

        private void CalculatorScore_Click(object sender, EventArgs e)
        {
            #region 计算积分
            if (this.CheckCookie())
            {
                credits.Text = UserCredits.GetUserCreditsByUid(DNTRequest.GetInt("uid", -1)).ToString();
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.StopTalk.Click += new EventHandler(this.StopTalk_Click);
            this.DelPosts.Click += new EventHandler(this.DelPosts_Click);
            this.SaveUserInfo.Click += new EventHandler(this.SaveUserInfo_Click);
            this.ResetPassWord.Click += new EventHandler(this.ResetPassWord_Click);
            this.IsEditUserName.CheckedChanged += new EventHandler(this.IsEditUserName_CheckedChanged);

            this.DelUserInfo.Click += new EventHandler(this.DelUserInfo_Click);
            this.ReSendEmail.Click += new EventHandler(this.ReSendEmail_Click);
            this.CalculatorScore.Click += new EventHandler(this.CalculatorScore_Click);
            this.ResetUserDigestPost.Click += new EventHandler(this.ResetUserDigestPost_Click);
            this.ResetUserPost.Click += new EventHandler(this.ResetUserPost_Click);

            this.GivenMedal.Click += new EventHandler(this.GivenMedal_Click);
            //UserCredits.UpdateUserCredits(DNTRequest.GetInt("uid", -1));
            userInfo = AdminUsers.GetUserInfo(DNTRequest.GetInt("uid", -1));

            UserGroupInfo tmpUserGroupInfo = UserCredits.GetCreditsUserGroupId(userInfo.Credits);
            groupid.Items.Add(new ListItem(UserGroups.GetUserGroupInfo(tmpUserGroupInfo.Groupid).Grouptitle, tmpUserGroupInfo.Groupid.ToString()));
            foreach (UserGroupInfo userGroupInfo in UserGroups.GetUserGroupList())
            {
                //if (userGroupInfo.System == 0 && userInfo.Groupid != userGroupInfo.Groupid || userGroupInfo.Groupid == 7)
                //    continue;

                if ((userGroupInfo.System == 0 && userGroupInfo.Radminid == 0) || userGroupInfo.Groupid == 7)
                    continue;
                groupid.Items.Add(new ListItem(userGroupInfo.Grouptitle, userGroupInfo.Groupid.ToString()));
                extgroupids.Items.Add(new ListItem(userGroupInfo.Grouptitle, userGroupInfo.Groupid.ToString()));
            }

            templateid.AddTableData(Templates.GetValidTemplateList(), "name", "templateid");
            templateid.Items[0].Text = "默认";
            TabControl1.InitTabPage();

            if (DNTRequest.GetString("uid") == "")
            {
                Response.Redirect("global_usergrid.aspx");
                return;
            }
            LoadCurrentUserInfo(DNTRequest.GetInt("uid", -1));
            LoadScoreInf(DNTRequest.GetString("uid"), DNTRequest.GetString("fieldname"));
        }

        #endregion

    }
}
