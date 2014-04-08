using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace Discuz.Control
{
    /// <summary>
    /// WebControl接口
    /// </summary>
    public interface IWebControl
    {
        /// <summary>
        /// 提示框标题
        /// </summary>
        string HintTitle { set;get;}

        /// <summary>
        /// 提示框内容
        /// </summary>
        string HintInfo { set;get;}

        /// <summary>
        /// 提示框左侧偏移量
        /// </summary>
        int HintLeftOffSet { set;get;}

        /// <summary>
        /// 提示框顶部偏移量
        /// </summary>
        int HintTopOffSet { set;get;}

        /// <summary>
        /// 提示框高度
        /// </summary>
        int HintHeight { set;get;}
    }

    /// <summary>
    /// DiscuzNT WebControl 基类
    /// </summary>
    public class WebControl : System.Web.UI.WebControls.WebControl, IWebControl
    {

        #region 变量声明
        private string _hintTitle = "";

        private string _hintInfo = "";

        private int _hintLeftOffSet = 0;

        private int _hintTopOffSet = 0;

        private string _hintShowType = "up";//或"down"

        private int _hintHeight = 50;
        #endregion


        /// <summary>
        /// 提示框标题
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HintTitle
        {
            get { return _hintTitle; }
            set { _hintTitle = value; }
        }

  
        /// <summary>
        /// 提示框内容
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HintInfo
        {
            get { return _hintInfo; }
            set { _hintInfo = value; }
        }

 
        /// <summary>
        /// 提示框左侧偏移量
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int HintLeftOffSet
        {
            get { return _hintLeftOffSet; }
            set { _hintLeftOffSet = value; }
        }

  
        /// <summary>
        /// 提示框顶部偏移量
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int HintTopOffSet
        {
            get { return _hintTopOffSet; }
            set { _hintTopOffSet = value; }
        }
                         
    
        /// <summary>
        /// 提示框风格,up(上方显示)或down(下方显示)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("up")]
        public string HintShowType
        {
            get { return _hintShowType; }
            set { _hintShowType = value; }
        }

   
        /// <summary>
        /// 提示框高度
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(50)]
        public int HintHeight
        {
            get { return _hintHeight; }
            set { _hintHeight = value; }
        }
      
    }
}
