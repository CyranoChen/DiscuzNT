using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Control
{
	/// <summary>
	/// CheckBoxList 控件。
	/// </summary>
	[DefaultProperty("Text"), ToolboxData("<{0}:CheckBoxList runat=server></{0}:CheckBoxList>")]
    public class CheckBoxList : System.Web.UI.WebControls.CheckBoxList, Discuz.Control.IWebControl, IPostBackDataHandler
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckBoxList(): base()
		{
			//this.BorderStyle=BorderStyle.Dotted; 
			//this.BorderWidth=1; 
			//this.Font.Size=10; 
			this.RepeatColumns=2;
			this.Width=Unit.Percentage(100);
			//this.Font.Size=FontUnit.Smaller;
			this.RepeatDirection=RepeatDirection.Vertical;
			this.RepeatLayout = RepeatLayout.Table;
            this.CssClass = "buttonlist";
            
		}

		/// <summary>
        /// 添加表数据
		/// </summary>
		/// <param name="dt">要绑定的表</param>
		public void AddTableData(DataTable dt)
		{
			this.Items.Clear();
			foreach( DataRow r in dt.Rows )
			{
				this.Items.Add(new ListItem(r[1].ToString(),r[0].ToString()));
			}	
			this.DataBind();			
		}

        public void AddTableData(DataTable dt, string textName, string valueName)
        {
            this.Items.Clear();
            foreach (DataRow r in dt.Rows)
            {
                this.Items.Add(new ListItem(r[textName].ToString(),r[valueName].ToString()));
            }
            this.DataBind();
        }


        /// <summary>
        /// 添加表数据
        /// </summary>
        /// <param name="sqlstring">查询字符串</param>
        /// <param name="selectid">选取项</param>
		public void AddTableData(string sqlstring,string selectid)
		{
			selectid=","+selectid+",";
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
     
			this.Items.Clear();
			for(int i=0;i<dt.Rows.Count;i++)
			{
				this.Items.Add(new ListItem(dt.Rows[i][1].ToString(),dt.Rows[i][0].ToString()));

                if (selectid.IndexOf("," + dt.Rows[i][0].ToString() + ",") >= 0)
                {
                    this.Items[i].Selected = true;
                }

			}	
			this.DataBind();			
		}


        /// <summary>
        /// 添加表数据
        /// </summary>
        /// <param name="dt">要绑定的表</param>
        /// <param name="selectid">选取项</param>
		public void AddTableData(DataTable dt,string selectid)
		{
			selectid=","+selectid+",";
				     
			this.Items.Clear();
			for(int i=0;i<dt.Rows.Count;i++)
			{
				this.Items.Add(new ListItem(dt.Rows[i][1].ToString(),dt.Rows[i][0].ToString()));
				
				if(selectid.IndexOf(","+dt.Rows[i][0].ToString()+",")>=0)   this.Items[i].Selected=true;

			}	
			this.DataBind();			
		}


		/// <summary>
        /// 通过ID绑定选项
		/// </summary>
		/// <param name="selectid">批定ID</param>
		public void SetSelectByID(string selectid)
		{
			selectid=","+selectid+",";
				     
			for(int i=0;i<this.Items.Count;i++)
			{
				
				if(selectid.IndexOf(","+this.Items[i].Value+",")>=0)   this.Items[i].Selected=true;

			}	
			this.DataBind();			
		}


        /// <summary>
        /// 得到所选项的字符串(格式: 1,2,3)
        /// </summary>
        /// <returns></returns>
		public string GetSelectString()
		{
			return GetSelectString(",");
		}

        /// <summary>
        /// 得到按指定分割符的选项字符串
        /// </summary>
        /// <param name="split">指定分割符</param>
        /// <returns></returns>
		public string GetSelectString(string split)
		{
            split = split.Trim();
            string result = "";
            foreach (ListItem li in this.Items)
            {
                if (li.Selected)
                {
                    result = result + li.Value + split;
                }
            }
            return result.TrimEnd(split.ToCharArray());
		}


        /// <summary>
        /// 得到按指定分割符的选项字符串
        /// </summary>
        /// <param name="split">指定分割符</param>
        /// <param name="items">列表选项(含未选中的选项)</param>
        /// <returns></returns>
		public string GetSelectString(string split,System.Web.UI.WebControls.ListItemCollection items)
		{
            split = split.Trim();
            string result = "";
            foreach (ListItem li in items)
            {
                if (li.Selected)
                {
                    result = result + li.Value + split;
                }
            }
            return result.TrimEnd(split.ToCharArray());
        }


        /// <summary>
        /// 当某选项被选中后,获取焦点的控件ID(如提交按钮等)
        /// </summary>
        [Bindable(true),Category("Appearance"),DefaultValue("")] 
		public string SetFocusButtonID
		{
			get
			{
				object o = ViewState[this.ClientID+"_SetFocusButtonID"];
				return (o==null)?"":o.ToString(); 
			}
			set
			{
				ViewState[this.ClientID+"_SetFocusButtonID"] = value;
				if(value!="")
				{
					this.Attributes.Add("onChange","document.getElementById('"+value+"').focus();");
				}
			}
		}


        private string _hintTitle = "";

        /// <summary>
        /// 提示框标题
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HintTitle
        {
            get { return _hintTitle; }
            set { _hintTitle = value; }
        }


        private string _hintInfo = "";

        /// <summary>
        /// 提示框内容
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HintInfo
        {
            get { return _hintInfo; }
            set { _hintInfo = value; }
        }

        private int _hintLeftOffSet = 0;

        /// <summary>
        /// 提示框左侧偏移量
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int HintLeftOffSet
        {
            get { return _hintLeftOffSet; }
            set { _hintLeftOffSet = value; }
        }

        private int _hintTopOffSet = 0;

        /// <summary>
        /// 提示框顶部偏移量
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int HintTopOffSet
        {
            get { return _hintTopOffSet; }
            set { _hintTopOffSet = value; }
        }

        private string _hintShowType = "up";//或"down"

        /// <summary>
        /// 提示框风格,up(上方显示)或down(下方显示)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("up")]
        public string HintShowType
        {
            get { return _hintShowType; }
            set { _hintShowType = value; }
        }

        private int _hintHeight = 50;

        /// <summary>
        /// 提示框高度
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(130)]
        public int HintHeight
        {
            get { return _hintHeight; }
            set { _hintHeight = value; }
        }

        /// <summary> 
        /// 输出html,在浏览器中显示控件
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        protected override void Render(HtmlTextWriter output)
        {
            if (this.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }

            base.Render(output);

            if (this.HintInfo != "")
            {
                output.WriteEndTag("span");
            }

        }

	}
}
