using System;
using System.IO;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Install
{
    public class finish : System.Web.UI.Page
    {
        public string productname = "Discuz!NT 3.1beta";

        protected void Page_Load(object sender, EventArgs e)
        {
            string souPath = Utils.GetMapPath(string.Format("{0}upgrade/commonupgradeini.config", BaseConfigs.GetForumPath)); 
            string destPath = Utils.GetMapPath(string.Format("{0}config/commonupgradeini.config", BaseConfigs.GetForumPath));
            if (File.Exists(souPath))
                File.Copy(souPath,destPath,true);
        }
    }
}
