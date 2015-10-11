using System;
using System.Text;
using System.Data;

using Discuz.Common;

namespace Discuz.Data
{
    /// <summary>
    /// 表情符数据操作类
    /// </summary>
    public class Smilies
    {
        /// <summary>
        /// 得到表情符数据
        /// </summary>
        /// <returns>表情符数据</returns>
        public static IDataReader GetSmiliesList()
        {
            return DatabaseProvider.GetInstance().GetSmiliesList();
        }

        /// <summary>
        /// 得到表情符数据
        /// </summary>
        /// <returns>表情符数据</returns>
        public static DataTable GetSmilies()
        {
            return DatabaseProvider.GetInstance().GetSmilies();
        }

        /// <summary>
        /// 得到表情符数据,包括表情分类
        /// </summary>
        /// <returns>表情符表</returns>
        public static DataTable GetSmiliesListDataTable()
        {
            return DatabaseProvider.GetInstance().GetSmiliesListDataTable();
        }

        /// <summary>
        /// 得到不带分类的表情符数据
        /// </summary>
        /// <returns>表情符表</returns>
        public static SmiliesInfo[] GetSmiliesListWithoutType()
        {
            return LoadSmiliesInfo(DatabaseProvider.GetInstance().GetSmiliesListWithoutType());
        }

        /// <summary>
        /// 装载表情信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static SmiliesInfo[] LoadSmiliesInfo(DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1)
                return null;
            SmiliesInfo[] smilieslistinfo = new SmiliesInfo[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                smilieslistinfo[i] = new SmiliesInfo();
                smilieslistinfo[i].Id = TypeConverter.ObjectToInt(dt.Rows[i]["id"], 0);
                smilieslistinfo[i].Code = dt.Rows[i]["Code"].ToString();
                smilieslistinfo[i].Displayorder = TypeConverter.ObjectToInt(dt.Rows[i]["Displayorder"], 0);
                smilieslistinfo[i].Type = TypeConverter.ObjectToInt(dt.Rows[i]["Type"], 0);
                smilieslistinfo[i].Url = dt.Rows[i]["Url"].ToString();
            }
            return smilieslistinfo;
        }

        /// <summary>
        /// 获得表情分类列表
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetSmilieTypes()
        //{
        //    return DatabaseProvider.GetInstance().GetSmilieTypes();
        //}

        public static SmiliesInfo[] GetSmiliesTypesInfo()
        {
            return LoadSmiliesInfo(GetSmiliesTypes());
        }

        /// <summary>
        /// 获取特定分类下的表情列表
        /// </summary>
        /// <param name="typeid">分类Id</param>
        /// <returns></returns>
        public static DataTable GetSmiliesInfoByType(int typeid)
        {
            return DatabaseProvider.GetInstance().GetSmiliesInfoByType(typeid);
        }

        /// <summary>
        /// 删除表情
        /// </summary>
        /// <param name="id">表情Id</param>
        public static void DeleteSmilies(string id)
        {
            DatabaseProvider.GetInstance().DeleteSmilies(id.ToString());
        }

        /// <summary>
        /// 获得表情分类列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSmiliesTypes()
        {
            return DatabaseProvider.GetInstance().GetSmilieTypes();
        }

        /// <summary>
        /// 创建表情
        /// </summary>
        /// <param name="id">表情Id</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="type">分类</param>
        /// <param name="code">快捷编码</param>
        /// <param name="url">图片地址</param>
        public static void CreateSmilies(int id, int displayorder, int type, string code, string url)
        {
            DatabaseProvider.GetInstance().AddSmiles(id, displayorder, type, code, url);
        }

        /// <summary>
        /// 获取表情最大Id
        /// </summary>
        /// <returns></returns>
        public static int GetMaxSmiliesId()
        {
            return DatabaseProvider.GetInstance().GetMaxSmiliesId();
        }

        /// <summary>
        /// 更新表情
        /// </summary>
        /// <param name="id">表情ID</param>
        /// <param name="displayOrder">排序</param>
        /// <param name="type">类型</param>
        /// <param name="code">代码</param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static int UpdateSmilies(int id, int displayOrder, int type, string code) 
        {
            return  DatabaseProvider.GetInstance().UpdateSmilies(id, displayOrder, type, code);
        }

        /// <summary>
        /// 更新表情
        /// </summary>
        /// <param name="id">表情ID</param>
        /// <param name="displayOrder">排序</param>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static int UpdateSmilies(string code, int displayOrder, int id)
        {
            return DatabaseProvider.GetInstance().UpdateSmiliesPart(code,displayOrder,id);
        }

        /// <summary>
        /// 按类型删除表情
        /// </summary>
        /// <param name="type">类型</param>
        public static void DeleteSmilyByType(int type)
        {
            DatabaseProvider.GetInstance().DeleteSmilyByType(type);
        }
    }
}
