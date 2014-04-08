using System;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Discuz.Control
{
    /// <summary>
    /// 属性页编辑器,供设置时调用
    /// </summary>
    internal class TabEditor : WindowsFormsComponentEditor
    {
        /// <summary>
        /// 编辑组件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
        {
            TabControl oControl = (TabControl)component;
            IServiceProvider site = oControl.Site;
            IComponentChangeService changeService = null;

            DesignerTransaction transaction = null;
            bool changed = false;

            try
            {
                if (site != null)
                {
                    IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
                    transaction = designerHost.CreateTransaction("BuildTabStrip");

                    changeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
                    if (changeService != null)
                    {
                        try
                        {
                            changeService.OnComponentChanging(component, null);
                        }
                        catch (CheckoutException ex)
                        {
                            if (ex == CheckoutException.Canceled)
                                return false;
                            throw ex;
                        }
                    }
                }

                try
                {
                    TabEditorForm oEditorForm = new TabEditorForm(oControl);
                    if (oEditorForm.ShowDialog(owner) == DialogResult.OK)
                    {
                        changed = true;
                    }
                }
                finally
                {
                    if (changed && changeService != null)
                    {
                        changeService.OnComponentChanged(oControl, null, null, null);
                    }
                }
            }
            finally
            {
                if (transaction != null)
                {
                    if (changed)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Cancel();
                    }
                }
            }

            return changed;
        }
    }
}
