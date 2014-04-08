using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class Identifys
    {
        /// <summary>
        /// 添加鉴定
        /// </summary>
        /// <param name="name">鉴定名称</param>
        /// <param name="fileName">图片名称</param>
        public static bool AddIdentify(string name, string  fileName)
        {
            return DatabaseProvider.GetInstance().AddIdentify(name, fileName);
        }

        /// <summary>
        /// 获取全部鉴定图片
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllIdentify()
        {
            return DatabaseProvider.GetInstance().GetAllIdentify();
        }

        /// <summary>
        /// 删除图片鉴定
        /// </summary>
        /// <param name="idlist"></param>
        public static void DeleteIdentify(string idlist)
        {
            DatabaseProvider.GetInstance().DeleteIdentify(idlist);
        }

        /// <summary>
        /// 更新鉴定
        /// </summary>
        /// <param name="id">鉴定ID</param>
        /// <param name="name">鉴定名称</param>
        /// <returns></returns>
        public static bool UpdateIdentifyById(int id, string name)
        {
            return DatabaseProvider.GetInstance().UpdateIdentifyById(id, name);
        }
    }
}
