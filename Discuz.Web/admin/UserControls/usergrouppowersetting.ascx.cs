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
                    usergroupright.Items.RemoveAt(usergroupright.Items.Count - 1);  //���һ������Ϊ�Ƿ�������
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

            if (usergroupinfo.Allowvisit == 1) usergroupright.Items[0].Selected = true; //�Ƿ����������̳
            if (usergroupinfo.Allowpost == 1) usergroupright.Items[1].Selected = true; //�Ƿ�������
            if (usergroupinfo.Allowreply == 1) usergroupright.Items[2].Selected = true; //�Ƿ�����ظ�
            if (usergroupinfo.Allowpostpoll == 1) usergroupright.Items[3].Selected = true; //�Ƿ�������ͶƱ
            if (usergroupinfo.Allowvote == 1) usergroupright.Items[4].Selected = true; //�Ƿ��������ͶƱ
            if (usergroupinfo.Allowpostattach == 1) usergroupright.Items[5].Selected = true; //�Ƿ񷢲�����
            if (usergroupinfo.Allowgetattach == 1) usergroupright.Items[6].Selected = true; //�Ƿ��������ظ���
            if (usergroupinfo.Allowsetreadperm == 1) usergroupright.Items[7].Selected = true; //�Ƿ��������������Ķ�����Ȩ��
            if (usergroupinfo.Allowsetattachperm == 1) usergroupright.Items[8].Selected = true; //�Ƿ��������ø����Ķ���������
            if (usergroupinfo.Allowhidecode == 1) usergroupright.Items[9].Selected = true; //�Ƿ�����ʹ��hide����
            if (usergroupinfo.Allowcusbbcode == 1) usergroupright.Items[10].Selected = true; //�Ƿ�����ʹ��Discuz!NT����
            if (usergroupinfo.Allowsigbbcode == 1) usergroupright.Items[11].Selected = true; //ǩ���Ƿ�֧��Discuz!NT����
            if (usergroupinfo.Allowsigimgcode == 1) usergroupright.Items[12].Selected = true; //ǩ���Ƿ�֧��ͼƬ����
            if (usergroupinfo.Allowviewpro == 1) usergroupright.Items[13].Selected = true; //�Ƿ�����鿴�û�����
            if (usergroupinfo.Disableperiodctrl == 1) usergroupright.Items[14].Selected = true; //�Ƿ���ʱ�������
            if (usergroupinfo.Allowdebate == 1) usergroupright.Items[15].Selected = true; //�Ƿ��������
            if (usergroupinfo.Allowbonus == 1) usergroupright.Items[16].Selected = true; //�Ƿ���������
            if (usergroupinfo.Allowviewstats == 1) usergroupright.Items[17].Selected = true; //�Ƿ�����鿴ͳ������
            if (usergroupinfo.Allowdiggs == 1) usergroupright.Items[18].Selected = true; //�Ƿ��������֧��
            if (usergroupinfo.Allowhtmltitle == 1) usergroupright.Items[19].Selected = true;//�Ƿ�����html����
            if (usergroupinfo.Allowhtml == 1) usergroupright.Items[20].Selected = true; //�Ƿ�����html
            if (usergroupinfo.ModNewTopics == 1) usergroupright.Items[21].Selected = true;//�������Ƿ���Ҫ���
            if (usergroupinfo.ModNewPosts == 1) usergroupright.Items[22].Selected = true;//���ظ��Ƿ���Ҫ���
            if (usergroupinfo.Ignoreseccode == 1) usergroupright.Items[23].Selected = true;//�Ƿ�������Լ����֤��
            if (MallPluginProvider.GetInstance() != null && usergroupinfo.Allowtrade == 1) usergroupright.Items[usergroupright.Items.Count - 1].Selected = true; //�Ƿ�������

            string strScript = "<script type='text/javascript'>\r\nfunction insertBonusPrice()\r\n{\r\n\t";
            strScript += "\r\n\tvar tdelement = document.getElementById('" + usergroupright.ClientID + "_16').parentNode;";
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;������ͼ۸�:<input type=\"text\" name=\"minbonusprice\" id=\"minbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"" + usergroupinfo.Minbonusprice + "\"" + (usergroupinfo.Allowbonus == 0 ? " disabled=\"disabled \"" : "") + " />'";
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;������ͼ۸�:<input type=\"text\" name=\"maxbonusprice\" id=\"maxbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"" + usergroupinfo.Maxbonusprice + "\"" + (usergroupinfo.Allowbonus == 0 ? " disabled=\"disabled \"" : "") + " />'";
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
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;������ͼ۸�:<input type=\"text\" name=\"minbonusprice\" id=\"minbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"10\" disabled=\"disabled\" />'";
            strScript += "\r\n\ttdelement.innerHTML += '&nbsp;������ͼ۸�:<input type=\"text\" name=\"maxbonusprice\" id=\"maxbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"20\" disabled=\"disabled\" />'";
            strScript += "\r\n}\r\ninsertBonusPrice();\r\n</script>\r\n";
            outscript.Text = strScript;
            usergroupright.Items[16].Attributes.Add("onclick", "bonusPriceSet(this.checked)");
        }

        public void GetSetting(ref UserGroupInfo usergroupinfo)
        {
            usergroupinfo.Allowsearch = Convert.ToInt32(allowsearch.SelectedValue);
            //usergroupinfo.Allowavatar = Convert.ToInt32(allowavatar.SelectedValue);
            usergroupinfo.Reasonpm = Convert.ToInt32(reasonpm.SelectedValue);

            usergroupinfo.Allowvisit = usergroupright.Items[0].Selected ? 1 : 0; //�Ƿ����������̳
            usergroupinfo.Allowpost = usergroupright.Items[1].Selected ? 1 : 0; //�Ƿ�������
            usergroupinfo.Allowreply = usergroupright.Items[2].Selected ? 1 : 0; //�Ƿ�����ظ�
            usergroupinfo.Allowpostpoll = usergroupright.Items[3].Selected ? 1 : 0; //�Ƿ�������ͶƱ
            usergroupinfo.Allowvote = usergroupright.Items[4].Selected ? 1 : 0; //�Ƿ��������ͶƱ
            usergroupinfo.Allowpostattach = usergroupright.Items[5].Selected ? 1 : 0; //�Ƿ񷢲�����
            usergroupinfo.Allowgetattach = usergroupright.Items[6].Selected ? 1 : 0; //�Ƿ��������ظ���
            usergroupinfo.Allowsetreadperm = usergroupright.Items[7].Selected ? 1 : 0; //�Ƿ��������������Ķ�����Ȩ��
            usergroupinfo.Allowsetattachperm = usergroupright.Items[8].Selected ? 1 : 0; //�Ƿ��������ø����Ķ���������
            usergroupinfo.Allowhidecode = usergroupright.Items[9].Selected ? 1 : 0; //�Ƿ�����ʹ��hide����
            usergroupinfo.Allowcusbbcode = usergroupright.Items[10].Selected ? 1 : 0; //�Ƿ�����ʹ��Discuz!NT����
            usergroupinfo.Allowsigbbcode = usergroupright.Items[11].Selected ? 1 : 0; //ǩ���Ƿ�֧��Discuz!NT����
            usergroupinfo.Allowsigimgcode = usergroupright.Items[12].Selected ? 1 : 0; //ǩ���Ƿ�֧��ͼƬ����
            usergroupinfo.Allowviewpro = usergroupright.Items[13].Selected ? 1 : 0; //�Ƿ�����鿴�û�����
            usergroupinfo.Disableperiodctrl = usergroupright.Items[14].Selected ? 1 : 0; //�Ƿ���ʱ�������

            usergroupinfo.Allowdebate = usergroupright.Items[15].Selected ? 1 : 0; //�Ƿ��������
            usergroupinfo.Allowbonus = usergroupright.Items[16].Selected ? 1 : 0; //�Ƿ���������
            //�����ѡ��������
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
            usergroupinfo.Allowviewstats = usergroupright.Items[17].Selected ? 1 : 0; //�Ƿ�����鿴ͳ������
            usergroupinfo.Allowdiggs = usergroupright.Items[18].Selected ? 1 : 0;   //�Ƿ��������֧��
            usergroupinfo.Allowhtmltitle = usergroupright.Items[19].Selected ? 1 : 0;//�Ƿ�����html����
            usergroupinfo.Allowhtml = usergroupright.Items[20].Selected ? 1 : 0;    //�Ƿ�����html
            usergroupinfo.ModNewTopics = usergroupright.Items[21].Selected ? 1 : 0; //�����������
            usergroupinfo.ModNewPosts = usergroupright.Items[22].Selected ? 1 : 0; //���ظ������
            usergroupinfo.Ignoreseccode = usergroupright.Items[23].Selected ? 1 : 0;//�Ƿ�������Լ����֤��
            if (MallPluginProvider.GetInstance() != null)
            {
                usergroupinfo.Allowtrade = usergroupright.Items[usergroupright.Items.Count - 1].Selected ? 1 : 0;   //�Ƿ�������
            }
        }
        
    }
}