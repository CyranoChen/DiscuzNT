using System;
using System.Data;
using System.Threading;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Forum;
using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using DropDownList = Discuz.Control.DropDownList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ���鷢�Ͷ���Ϣ
    /// </summary>
    public partial class sendsmtogroup : AdminPage
    {
        public string groupidlist = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.username != "")
                {
                    msgfrom.Text = this.username;
                    postdatetime.Text = DateTime.Now.ToShortDateString();
                }
            }
        }

        private void BatchSendSM_Click(object sender, EventArgs e)
        {
            #region ��������Ϣ����

            if (this.CheckCookie())
            {
                groupidlist = Usergroups.GetSelectString(",");

                if (groupidlist == "")
                {
                    base.RegisterStartupScript("", "<script>alert('������ѡȡ��ص��û���,�ٵ���ύ��ť');</script>");
                    return;
                }

#if EntLib
                if (RabbitMQConfigs.GetConfig() != null && RabbitMQConfigs.GetConfig().SendShortMsg.Enable)//������errlog������־��¼����ʱ
                {
                    PrivateMessageInfo pm = new PrivateMessageInfo()
                    {
                        Msgfrom = username.Replace("'", "''"),
                        Msgfromid = userid,
                        Folder = int.Parse(folder.SelectedValue),
                        Subject = subject.Text,
                        Postdatetime = Discuz.Common.Utils.GetDateTime(),//��ȡ������Ϣ��ϵͳʱ��
                        Message = message.Text,
                        New = 1//���Ϊδ��
                    };
                    Discuz.EntLib.ServiceBus.SendShortMsgClientHelper.GetSendShortMsgClient().AsyncSendShortMsgByUserGroup(groupidlist, pm);
                    return;
                }
#endif
                ClientScript.RegisterStartupScript(this.GetType(), "Page", "<script>submit_Click();</script>");               
            }
            
            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.BatchSendSM.Click += new EventHandler(this.BatchSendSM_Click);
            message.is_replace = true;

            DataTable dt = UserGroups.GetUserGroupWithOutGuestTitle();
            foreach (DataRow dr in dt.Rows)
            {
                dr["grouptitle"] = "<img src=../images/usergroup.gif border=0  style=\"position:relative;top:2 ;height:18 \">" + dr["grouptitle"];
            }
            Usergroups.AddTableData(dt);
        }

        #endregion
    }
}