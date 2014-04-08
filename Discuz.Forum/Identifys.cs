using Discuz.Cache;

namespace Discuz.Forum
{
    public class Identifys
    {
        /// <summary>
        /// 添加鉴定
        /// </summary>
        /// <param name="name">鉴定名称</param>
        /// <param name="fileName">图片名称</param>
        public static bool AddIdentify(string name, string fileName)
        {
            DNTCache.GetCacheService().RemoveObject("/Forum/TopicIdentifys");
            DNTCache.GetCacheService().RemoveObject("/Forum/TopicIndentifysJsArray");
            return Data.Identifys.AddIdentify(name, fileName);
        }

        /// <summary>
        /// 获取全部鉴定图片
        /// </summary>
        /// <returns></returns>
        public static System.Data.DataTable GetAllIdentify()
        {
            return Data.Identifys.GetAllIdentify();
        }

        /// <summary>
        /// 删除图片鉴定
        /// </summary>
        /// <param name="idlist"></param>
        public static void DeleteIdentify(string idlist)
        {
            if (Discuz.Common.Utils.IsNumericList(idlist))
            {
                Data.Identifys.DeleteIdentify(idlist);
                DNTCache.GetCacheService().RemoveObject("/Forum/TopicIdentifys");
                DNTCache.GetCacheService().RemoveObject("/Forum/TopicIndentifysJsArray");
            }
        }

        /// <summary>
        /// 更新鉴定
        /// </summary>
        /// <param name="id">鉴定ID</param>
        /// <param name="name">鉴定名称</param>
        /// <returns></returns>
        public static bool UpdateIdentifyById(int id, string name)
        {
            return id > 0 ? Data.Identifys.UpdateIdentifyById(id, name) : false;
        }
    }
}
