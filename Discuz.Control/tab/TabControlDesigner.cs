namespace Discuz.Control
{
    using System;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.ComponentModel.Design;

    /// <summary>
    /// 属性页设置器
    /// </summary>
    public class TabControlDesigner : ControlDesigner
    {
        /// <summary>
        /// 获取设计时HTML
        /// </summary>
        /// <returns></returns>
        protected override string GetEmptyDesignTimeHtml()
        {
            return CreatePlaceHolderDesignTimeHtml("右击选择创建新的属性页");
        }
        
        private DesignerVerbCollection _verbs;

        /// <summary>
        /// 定义菜单项动作
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    _verbs = new DesignerVerbCollection(new DesignerVerb[] { new DesignerVerb("创建新的属性页...", new EventHandler(this.OnBuildTabStrip)) });
                }

                return _verbs;
            }
        }


        private void OnBuildTabStrip(object sender, EventArgs e)
        {
            TabEditor oEditor = new TabEditor();
            oEditor.EditComponent(this.Component);
        }

        /// <summary>
        /// 获取设计时HTML
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {

            try
            {
                TabControl oTabStrip = ((TabControl)Component);

                if (oTabStrip.Items == null || oTabStrip.Items.Count == 0)
                {
                    return GetEmptyDesignTimeHtml();
                }

                System.Text.StringBuilder oSB = new System.Text.StringBuilder();
                StringWriter oStringWriter = new StringWriter(oSB);
                HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

                oTabStrip.RenderDownLevelContent(oWriter);
                oWriter.Flush();
                oStringWriter.Flush();

                return oSB.ToString();
            }
            catch (Exception ex)
            {
                return CreatePlaceHolderDesignTimeHtml("生成设计时代码错误:\n\n" + ex.ToString());
            }
        }
        
    }
}

