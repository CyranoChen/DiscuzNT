using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using DataGrid = Discuz.Control.DataGrid;
using DropDownList = Discuz.Control.DropDownList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Plugin.Payment.Alipay;
using Discuz.Plugin.Payment;
using System.Web;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 积分设置
    /// </summary>
    
    public partial class scoreset : AdminPage
    {
        public DataSet dsSrc = new DataSet();
        public GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
        public string[] scoreNames = Scoresets.GetValidScoreName();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DNTRequest.GetString("accout")))
            {
                TestAccout(DNTRequest.GetString("accout"));
            }
            if (!Page.IsPostBack)
            {

                #region 加载积分设置公式及其它相关信息

                for (int i = 1; i < 9; i++)
                {
                    if (scoreNames[i] == string.Empty)
                        continue;
                    creditstrans.Items.Add(new ListItem("extcredits" + i + "(" + scoreNames[i] + ")", i.ToString()));
                    topicattachcreditstrans.Items.Add(new ListItem("extcredits" + i + "(" + scoreNames[i] + ")", i.ToString()));
                    bonuscreditstrans.Items.Add(new ListItem("extcredits" + i + "(" + scoreNames[i] + ")", i.ToString()));
                }


                dsSrc.ReadXml(Server.MapPath("../../config/scoreset.config"));
                formula.Text = dsSrc.Tables["formula"].Rows[0]["formulacontext"].ToString();
                if (dsSrc.Tables["formula"].Rows[0]["creditstrans"].ToString() == "")
                {
                    creditstrans.SelectedIndex = 0;
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "credits", "creditsTransStatus(0);");
                }
                else
                {
                    try
                    {
                        creditstrans.SelectedValue = dsSrc.Tables["formula"].Rows[0]["creditstrans"].ToString();
                    }
                    catch { }
                }
                //if (creditstrans.SelectedIndex == 0)
                //{
                //    creditstransLayer.Attributes.Add("style", "display:none");
                //}
                creditstrans.Attributes.Add("onchange", "creditsTransStatus(this.value);");

                if (dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"].ToString() == "")
                {
                    topicattachcreditstrans.SelectedIndex = 0;
                }
                else
                {
                    try
                    {
                        topicattachcreditstrans.SelectedValue = dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"].ToString();
                    }
                    catch { }
                }

                if (dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"].ToString() == "")
                {
                    bonuscreditstrans.SelectedIndex = 0;
                }
                else
                {
                    try
                    {
                        bonuscreditstrans.SelectedValue = dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"].ToString();
                    }
                    catch { }
                }


                creditstax.Text = dsSrc.Tables["formula"].Rows[0]["creditstax"].ToString();
                transfermincredits.Text = dsSrc.Tables["formula"].Rows[0]["transfermincredits"].ToString();
                exchangemincredits.Text = dsSrc.Tables["formula"].Rows[0]["exchangemincredits"].ToString();
                maxincperthread.Text = dsSrc.Tables["formula"].Rows[0]["maxincperthread"].ToString();
                maxchargespan.Text = dsSrc.Tables["formula"].Rows[0]["maxchargespan"].ToString();
                losslessdel.Text = configInfo.Losslessdel.ToString();
                #endregion

                BindData();


            }
        }

        public void BindData()
        {
            #region 积分设置列表数据加载

            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "<img src='../images/icons/icon31.jpg'>积分设置";
            DataGrid1.DataSource = dsSrc.Tables[0];
            DataGrid1.DataBind();

            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 设置数据绑定的长度

            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                    break;
                case ListItemType.AlternatingItem:
                    break;
                case ListItemType.Header:
                    e.Item.Cells[0].ColumnSpan = 1; //合并单元格 
                    e.Item.Cells[1].Visible = false;
                    break;
                case ListItemType.EditItem:
                    {
                        for (int i = 0; i < DataGrid1.Columns.Count; i++) //只调整被编辑的列 
                        {
                            if (e.Item.ItemType == ListItemType.EditItem)
                            {
                                if (i >= 3)
                                {
                                    System.Web.UI.WebControls.TextBox txt = (System.Web.UI.WebControls.TextBox)e.Item.Cells[i].Controls[0];
                                    txt.Width = 60;
                                }
                            }
                        }
                        break;
                    }
                default:
                    break;
            }

            #endregion
        }

        public DataTable AbsScoreSet1(DataTable dt)
        {
            #region 对表中数据进行绝对值转换

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt16(dr["id"].ToString()) > 2)
                {
                    //当是减积分操作时，系统自动取反。这样做主要是为了从config文件中取出的值无论正负将可参与前台积分运算
                    if ((dr["id"].ToString() == "11") || (dr["id"].ToString() == "12") || (dr["id"].ToString() == "13"))
                    {
                        dr["extcredits1"] = (-1) * Double.Parse(dr["extcredits1"].ToString());
                        dr["extcredits2"] = (-1) * Double.Parse(dr["extcredits2"].ToString());
                        dr["extcredits3"] = (-1) * Double.Parse(dr["extcredits3"].ToString());
                        dr["extcredits4"] = (-1) * Double.Parse(dr["extcredits4"].ToString());
                        dr["extcredits5"] = (-1) * Double.Parse(dr["extcredits5"].ToString());
                        dr["extcredits6"] = (-1) * Double.Parse(dr["extcredits6"].ToString());
                        dr["extcredits7"] = (-1) * Double.Parse(dr["extcredits7"].ToString());
                        dr["extcredits8"] = (-1) * Double.Parse(dr["extcredits8"].ToString());
                    }
                }
            }
            return dt;

            #endregion
        }

        public void SetUserGroupRaterange(int scoreid)
        {
            #region 设置用户组积分范围

            bool isupdate = true;

            foreach (DataRow dr in Scoresets.GetScoreSet().Rows)
            {
                if ((dr["id"].ToString() != "1") && (dr["id"].ToString() != "2"))
                {
                    if (dr[scoreid + 1].ToString().Trim() != "0")
                    {
                        isupdate = false;
                        break;
                    }
                }
            }

            if (isupdate)
            {
                foreach (DataRow dr in UserGroups.GetRateRange(scoreid).Rows)
                {
                    UserGroups.UpdateRateRange(dr["raterange"].ToString().Replace(scoreid + ",True,", scoreid + ",False,"), Utils.StrToInt(dr["groupid"], 0));
                }
            }

            #endregion
        }

        public void DataGrid_Update(Object sender, DataGridCommandEventArgs E)
        {
            #region 更新相应的积分选项

            string id = DataGrid1.DataKeys[(int)E.Item.ItemIndex].ToString();
            string item3 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[3].Controls[0]).Text.Trim();
            string item4 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[4].Controls[0]).Text.Trim();
            string item5 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[5].Controls[0]).Text.Trim();
            string item6 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[6].Controls[0]).Text.Trim();
            string item7 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[7].Controls[0]).Text.Trim();
            string item8 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[8].Controls[0]).Text.Trim();
            string item9 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[9].Controls[0]).Text.Trim();
            string item10 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[10].Controls[0]).Text.Trim();

            int rowindex = Convert.ToInt16(id);
            bool flag = true;

            //当内容是只能为文字情况下
            if (rowindex <= 2)
            {
                if (rowindex == 1)
                {
                    if (item3 == "") SetUserGroupRaterange(1);
                    if (item4 == "") SetUserGroupRaterange(2);
                    if (item5 == "") SetUserGroupRaterange(3);
                    if (item6 == "") SetUserGroupRaterange(4);
                    if (item7 == "") SetUserGroupRaterange(5);
                    if (item8 == "") SetUserGroupRaterange(6);
                    if (item9 == "") SetUserGroupRaterange(7);
                    if (item10 == "") SetUserGroupRaterange(8);
                }

                //是否为空或数字类型
                if (item3 != "" && (Utils.IsNumeric(item3.Replace("-", "")))) flag = false;
                if (item4 != "" && (Utils.IsNumeric(item4.Replace("-", "")))) flag = false;
                if (item5 != "" && (Utils.IsNumeric(item5.Replace("-", "")))) flag = false;
                if (item6 != "" && (Utils.IsNumeric(item6.Replace("-", "")))) flag = false;
                if (item7 != "" && (Utils.IsNumeric(item7.Replace("-", "")))) flag = false;
                if (item8 != "" && (Utils.IsNumeric(item8.Replace("-", "")))) flag = false;
                if (item9 != "" && (Utils.IsNumeric(item9.Replace("-", "")))) flag = false;
                if (item10 != "" && (Utils.IsNumeric(item10.Replace("-", "")))) flag = false;

                if (!flag)
                {
                    base.RegisterStartupScript( "DataGrid1",
                                               "<script>alert('当前项中数据不能为数字');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
            }
            else
            {
                if (item3 == "") flag = false;
                if (item4 == "") flag = false;
                if (item5 == "") flag = false;
                if (item6 == "") flag = false;
                if (item7 == "") flag = false;
                if (item8 == "") flag = false;
                if (item9 == "") flag = false;
                if (item10 == "") flag = false;

                if (!flag)
                {
                    base.RegisterStartupScript( "DataGrid1",
                                               "<script>alert('当前项中数据不能为空.');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }

                flag = true;

                //是否为空或非数字型
                if (item3 != "" && (!Utils.IsNumeric(item3.Replace("-", "")))) flag = false;
                if (item4 != "" && (!Utils.IsNumeric(item4.Replace("-", "")))) flag = false;
                if (item5 != "" && (!Utils.IsNumeric(item5.Replace("-", "")))) flag = false;
                if (item6 != "" && (!Utils.IsNumeric(item6.Replace("-", "")))) flag = false;
                if (item7 != "" && (!Utils.IsNumeric(item7.Replace("-", "")))) flag = false;
                if (item8 != "" && (!Utils.IsNumeric(item8.Replace("-", "")))) flag = false;
                if (item9 != "" && (!Utils.IsNumeric(item9.Replace("-", "")))) flag = false;
                if (item10 != "" && (!Utils.IsNumeric(item10.Replace("-", "")))) flag = false;

                if (!flag)
                {
                    base.RegisterStartupScript( "DataGrid1",
                                               "<script>alert('当前项中数据只能为数字.');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }

                flag = true;

                //数字范围是否在-999 至 999之间
                if ((Convert.ToDouble(item3) > 999) || (Convert.ToDouble(item3) < -999)) flag = false;
                if ((Convert.ToDouble(item4) > 999) || (Convert.ToDouble(item4) < -999)) flag = false;
                if ((Convert.ToDouble(item5) > 999) || (Convert.ToDouble(item5) < -999)) flag = false;
                if ((Convert.ToDouble(item6) > 999) || (Convert.ToDouble(item6) < -999)) flag = false;
                if ((Convert.ToDouble(item7) > 999) || (Convert.ToDouble(item7) < -999)) flag = false;
                if ((Convert.ToDouble(item8) > 999) || (Convert.ToDouble(item8) < -999)) flag = false;
                if ((Convert.ToDouble(item9) > 999) || (Convert.ToDouble(item9) < -999)) flag = false;
                if ((Convert.ToDouble(item10) > 999) || (Convert.ToDouble(item10) < -999)) flag = false;

                if (!flag)
                {
                    base.RegisterStartupScript( "DataGrid1",
                                               "<script>alert('各项积分增减允许的范围为-999～+999.');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
            }

            dsSrc = new DataSet();

            dsSrc.ReadXml(Server.MapPath("../../config/scoreset.config"));

            foreach (DataRow dr in dsSrc.Tables["record"].Rows)
            {
                int currentid = Convert.ToInt16(dr["id"].ToString());
                if (id == currentid.ToString())
                {
                    if (rowindex <= 2)
                    {
                        dr["extcredits1"] = item3;
                        dr["extcredits2"] = item4;
                        dr["extcredits3"] = item5;
                        dr["extcredits4"] = item6;
                        dr["extcredits5"] = item7;
                        dr["extcredits6"] = item8;
                        dr["extcredits7"] = item9;
                        dr["extcredits8"] = item10;
                    }
                    else
                    {
                        dr["extcredits1"] = Math.Round(Double.Parse(item3), 2);
                        dr["extcredits2"] = Math.Round(Double.Parse(item4), 2);
                        dr["extcredits3"] = Math.Round(Double.Parse(item5), 2);
                        dr["extcredits4"] = Math.Round(Double.Parse(item6), 2);
                        dr["extcredits5"] = Math.Round(Double.Parse(item7), 2);
                        dr["extcredits6"] = Math.Round(Double.Parse(item8), 2);
                        dr["extcredits7"] = Math.Round(Double.Parse(item9), 2);
                        dr["extcredits8"] = Math.Round(Double.Parse(item10), 2);
                    }
                }

                if (currentid > 2)
                {
                    dr["extcredits1"] = Math.Round(Double.Parse(dr["extcredits1"].ToString()), 2);
                    dr["extcredits2"] = Math.Round(Double.Parse(dr["extcredits2"].ToString()), 2);
                    dr["extcredits3"] = Math.Round(Double.Parse(dr["extcredits3"].ToString()), 2);
                    dr["extcredits4"] = Math.Round(Double.Parse(dr["extcredits4"].ToString()), 2);
                    dr["extcredits5"] = Math.Round(Double.Parse(dr["extcredits5"].ToString()), 2);
                    dr["extcredits6"] = Math.Round(Double.Parse(dr["extcredits6"].ToString()), 2);
                    dr["extcredits7"] = Math.Round(Double.Parse(dr["extcredits7"].ToString()), 2);
                    dr["extcredits8"] = Math.Round(Double.Parse(dr["extcredits8"].ToString()), 2);

                    //当是减积分操作时，系统自动取反。这样做主要是为了从config文件中取出的值无论正负将可参与前台积分运算
                    //if ((currentid == 11) || (currentid == 12) || (currentid == 13))
                    //{
                    //    dr["extcredits1"] = (-1) * Double.Parse(dr["extcredits1"].ToString());
                    //    dr["extcredits2"] = (-1) * Double.Parse(dr["extcredits2"].ToString());
                    //    dr["extcredits3"] = (-1) * Double.Parse(dr["extcredits3"].ToString());
                    //    dr["extcredits4"] = (-1) * Double.Parse(dr["extcredits4"].ToString());
                    //    dr["extcredits5"] = (-1) * Double.Parse(dr["extcredits5"].ToString());
                    //    dr["extcredits6"] = (-1) * Double.Parse(dr["extcredits6"].ToString());
                    //    dr["extcredits7"] = (-1) * Double.Parse(dr["extcredits7"].ToString());
                    //    dr["extcredits8"] = (-1) * Double.Parse(dr["extcredits8"].ToString());
                    //}
                }
            }


            try
            {

                dsSrc.WriteXml(Server.MapPath("../../config/scoreset.config"));
                dsSrc.Reset();
                dsSrc.Dispose();

                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.RemoveObject("/Forum/ScorePaySet");
                cache.RemoveObject("/Forum/ScoreSet");
                cache.RemoveObject("/Forum/ValidScoreName");
                cache.RemoveObject("/Forum/ValidScoreUnit");
                cache.RemoveObject("/Forum/RateScoreSet");
                cache.RemoveObject("/Forum/IsSetDownLoadAttachScore");

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip,
                                              "积分设置", "");

                DataGrid1.EditItemIndex = -1;

                dsSrc = new DataSet();
                dsSrc.ReadXml(Server.MapPath("../../config/scoreset.config"));
                //DataGrid1.DataSource = AbsScoreSet(dsSrc.Tables[0]);
                DataGrid1.DataSource = dsSrc.Tables[0];
                DataGrid1.DataBind();
            }
            catch
            {
                base.RegisterStartupScript( "DataGrid1",
                                           "<script>alert('无法更新数据库.');window.location.href='global_scoreset.aspx';</script>");
                return;
            }

            if (rowindex > 2)
            {
                Regex r = new Regex(@"^\d+(\.\d{1,2})?$");
                if (!r.IsMatch(item3.Replace("-", "")) || !r.IsMatch(item4.Replace("-", "")) ||
                    !r.IsMatch(item5.Replace("-", "")) || !r.IsMatch(item6.Replace("-", "")) ||
                    !r.IsMatch(item7.Replace("-", "")) || !r.IsMatch(item8.Replace("-", "")) ||
                    !r.IsMatch(item9.Replace("-", "")) || !r.IsMatch(item10.Replace("-", "")))
                {
                    base.RegisterStartupScript( "DataGrid1",
                                               "<script>alert('当前数据项最多只能为小位点后两位,系统将四舍五入该数据.');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
            }
            #endregion
        }

        private void Save_Click(object sender, EventArgs e)
        {
            #region 保存积分设置信息

            if (this.CheckCookie())
            {
                if ((Convert.ToDouble(creditstax.Text.Trim()) > 1) || (Convert.ToDouble(creditstax.Text.Trim()) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('积分交易税必须是0--1之间的小数');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }

                if (Convert.ToDouble(transfermincredits.Text.Trim()) < 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('转账最低余额必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }

                if (Convert.ToDouble(exchangemincredits.Text.Trim()) < 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('兑换最低余额必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }

                if (Convert.ToDouble(maxincperthread.Text.Trim()) < 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('单主题最高收入必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }

                if (Convert.ToDouble(maxchargespan.Text.Trim()) < 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('单主题最高出售时限必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }

                if (formula.Text.Trim() == "" || !AdminForums.CreateUpdateUserCreditsProcedure(formula.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script>alert('总积分计算公式为空或不正确');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                if (Convert.ToInt32(losslessdel.Text) > 9999 || Convert.ToInt32(losslessdel.Text) < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('删帖不减积分时间期限只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                    return;
                }

                dsSrc.ReadXml(Server.MapPath("../../config/scoreset.config"));
                dsSrc.Tables["formula"].Rows[0]["formulacontext"] = formula.Text.Trim();
                dsSrc.Tables["formula"].Rows[0]["creditstrans"] = creditstrans.SelectedValue;
                if(creditstrans.SelectedValue == "0")
                {
                    dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"] = creditstrans.SelectedValue;
                    dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"] = creditstrans.SelectedValue;
                }
                else
                {
                    dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"] = topicattachcreditstrans.SelectedValue;
                    dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"] = bonuscreditstrans.SelectedValue;
                }
                dsSrc.Tables["formula"].Rows[0]["creditstax"] = Convert.ToDouble(creditstax.Text);
                dsSrc.Tables["formula"].Rows[0]["transfermincredits"] = Convert.ToDouble(transfermincredits.Text);
                dsSrc.Tables["formula"].Rows[0]["exchangemincredits"] = Convert.ToDouble(exchangemincredits.Text);
                dsSrc.Tables["formula"].Rows[0]["maxincperthread"] = Convert.ToDouble(maxincperthread.Text);
                dsSrc.Tables["formula"].Rows[0]["maxchargespan"] = Convert.ToDouble(maxchargespan.Text);
                dsSrc.WriteXml(Server.MapPath("../../config/scoreset.config"));


                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.RemoveObject("/Forum/Scoreset");
                cache.RemoveObject("/Forum/Scoreset/CreditsTrans");
                cache.RemoveObject("/Forum/Scoreset//Forum/Scoreset/TopicAttachCreditsTrans");
                cache.RemoveObject("/Forum/Scoreset/BonusCreditsTrans");
                cache.RemoveObject("/Forum/Scoreset/CreditsTax");
                cache.RemoveObject("/Forum/Scoreset/TransferMinCredits");
                cache.RemoveObject("/Forum/Scoreset/ExchangeMinCredits");
                cache.RemoveObject("/Forum/Scoreset/MaxIncPerThread");
                cache.RemoveObject("/Forum/Scoreset/MaxChargeSpan");
                cache.RemoveObject("/Forum/IsSetDownLoadAttachScore");
                cache.RemoveObject("/Forum/ValidScoreUnit");
                cache.RemoveObject("/Forum/RateScoreSet");

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "修改积分设置", "修改积分设置");
             
                configInfo.Alipayaccout = DNTRequest.GetFormString("alipayaccount");
                configInfo.Cashtocreditrate = DNTRequest.GetFormInt("cashtocreditsrate", 0);

                int mincreditstobuy = DNTRequest.GetFormInt("mincreditstobuy", 0);
                //如果现金/积分兑换比率为0，则表示不开启积分充值功能
                if (configInfo.Cashtocreditrate > 0)
                {
                    //为了保证生成的订单价格最低价格为0.1元，则需要根据现金和积分兑换比率来动态调整积分最少购买数量的值
                    while ((decimal)mincreditstobuy / (decimal)configInfo.Cashtocreditrate < 0.10M)
                    {
                        mincreditstobuy++;
                    }
                }

                configInfo.Mincreditstobuy = mincreditstobuy;
                configInfo.Maxcreditstobuy = DNTRequest.GetFormInt("maxcreditstobuy", 0);
                configInfo.Userbuycreditscountperday = DNTRequest.GetFormInt("userbuycreditscountperday", 0);
                configInfo.Alipaypartnercheckkey = DNTRequest.GetFormString("alipaypartnercheckkey");
                configInfo.Alipaypartnerid = DNTRequest.GetFormString("alipaypartnerid");
                configInfo.Usealipaycustompartnerid = DNTRequest.GetFormInt("usealipaycustompartnerid", 1);
                configInfo.Usealipayinstantpay = DNTRequest.GetFormInt("usealipayinstantpay", 0);
                configInfo.Losslessdel = Convert.ToInt16(losslessdel.Text);
                GeneralConfigs.SaveConfig(configInfo);
                GeneralConfigs.ResetConfig();

                if (RefreshUserScore.SelectedValue.IndexOf("1") == 0)
                {
                    //运行ajax批量更新用户积分功能
                    ClientScript.RegisterStartupScript(this.GetType(), "Page", "<script>submit_Click();</script>");
                    return;
                    //Users.UpdateUserCredits(formula.Text, 0);
                }

                base.RegisterStartupScript( "PAGE",  "window.location.href='global_scoreset.aspx';");
            }

            #endregion
        }

        protected void DataGrid_Delete(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.DataKeys[E.Item.ItemIndex].ToString();
        }

        protected void DataGrid_Edit(Object sender, DataGridCommandEventArgs E)
        {
            #region 编辑积分
            DataGrid1.EditItemIndex = E.Item.ItemIndex;
            dsSrc.Clear();
            dsSrc.ReadXml(Server.MapPath("../../config/scoreset.config"));
            //DataGrid1.DataSource = AbsScoreSet(dsSrc.Tables[0]);
            DataGrid1.DataSource = dsSrc.Tables[0];
            DataGrid1.DataBind();
            #endregion
        }

        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs E)
        {
            #region 取消编辑
            DataGrid1.EditItemIndex = -1;
            //DataGrid1.DataSource = AbsScoreSet(Scoresets.GetScoreSet());
            DataGrid1.DataSource = Scoresets.GetScoreSet();
            DataGrid1.DataBind();
            #endregion
        }

        public string GetImgLink(string img)
        {
            if (img != "")
            {
                return "<img src=../../images/groupicons/" + img + ">";
            }
            return "";
        }
        public void TestAccout(string accout)
        {
            int openPartner = DNTRequest.GetInt("openpartner", 0);
            string partnerId = DNTRequest.GetString("partnerid");
            string partnerKey = DNTRequest.GetString("partnerKey");

            DigitalTrade virtualTrade = new DigitalTrade();
            virtualTrade.Subject = "测试支付宝充值功能";

            if (Utils.IsValidEmail(accout))//如果传递的帐号类型是email
                virtualTrade.Seller_Email = accout;
            else
                virtualTrade.Seller_Id = accout;

            virtualTrade.Return_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            virtualTrade.Notify_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            virtualTrade.Quantity = 1;
            virtualTrade.Price = 0.1M;
            virtualTrade.Payment_Type = 1;
            virtualTrade.PayMethod = "bankPay";

            string payUrl = "";

            if (openPartner == 1)
            {
                virtualTrade.Partner = partnerId;
                virtualTrade.Sign = partnerKey;
                payUrl = StandardAliPayment.GetService().CreateDigitalGoodsTradeUrl(virtualTrade);
            }
            else
            {
                payUrl = AliPayment.GetService().CreateDigitalGoodsTradeUrl(virtualTrade);
            }

            HttpContext.Current.Response.Redirect(payUrl);
        }
        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
            this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(DataGrid_Cancel);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.Save.Click += new EventHandler(this.Save_Click);

            formula.IsReplaceInvertedComma = false;

            DataGrid1.LoadEditColumn();
            DataGrid1.AllowSorting = false;
            DataGrid1.AllowPaging = false;
            DataGrid1.ShowFooter = false;
        }

        #endregion
    }
}