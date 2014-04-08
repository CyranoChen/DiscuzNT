using System;

namespace Discuz.Entity
{
	/// <summary>
	/// CustomEditorButtonInfo 的摘要说明。
	/// </summary>
    [Serializable]
	public class CustomEditorButtonInfo
	{
		private int m_id;	//自动编号
		private int m_available;	//是否有效
		private string m_tag;	//Discuz!NT代码标签代码
		private string m_icon;	//图标地址
		private string m_replacement;	//替换内容
		private string m_example;	//范例(用于显示于帮助中)
		private string m_explanation;	//解释(用于显示于帮助中)
		private int m_params;	//参数个数
		private int m_nest;	//最大深度
		private string m_paramsdescript;	//参数对应的描述
		private string m_paramsdefvalue;	//参数对应的默认值

		///<summary>
		///自动编号
		///</summary>
		public int Id
		{
			get { return m_id;}
			set { m_id = value;}
		}
		///<summary>
		///是否有效
		///</summary>
		public int Available
		{
			get { return m_available;}
			set { m_available = value;}
		}
		///<summary>
		///Discuz!NT代码标签代码
		///</summary>
		public string Tag
		{
			get { return m_tag;}
			set { m_tag = value;}
		}
		///<summary>
		///图标地址
		///</summary>
		public string Icon
		{
			get { return m_icon;}
			set { m_icon = value;}
		}
		///<summary>
		///替换内容
		///</summary>
		public string Replacement
		{
			get { return m_replacement;}
			set { m_replacement = value;}
		}
		///<summary>
		///范例(用于显示于帮助中)
		///</summary>
		public string Example
		{
			get { return m_example;}
			set { m_example = value;}
		}
		///<summary>
		///解释(用于显示于帮助中)
		///</summary>
		public string Explanation
		{
			get { return m_explanation;}
			set { m_explanation = value;}
		}
		///<summary>
		///参数个数
		///</summary>
		public int Params
		{
			get { return m_params;}
			set { m_params = value;}
		}
		///<summary>
		///最大深度
		///</summary>
		public int Nest
		{
			get { return m_nest;}
			set { m_nest = value;}
		}
		///<summary>
		///参数对应的描述
		///</summary>
		public string Paramsdescript
		{
			get { return m_paramsdescript;}
			set { m_paramsdescript = value;}
		}
		///<summary>
		///参数对应的默认值
		///</summary>
		public string Paramsdefvalue
		{
			get { return m_paramsdefvalue;}
			set { m_paramsdefvalue = value;}
		}
	}
}
