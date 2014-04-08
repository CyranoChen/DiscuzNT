using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class BBCodes
    {
        /// <summary>
        /// 获取Discuz!NT代码
        /// </summary>
        public static DataTable GetBBCode()
        {
            return DatabaseProvider.GetInstance().GetBBCode();
        }

        /// <summary>
        /// 按Id获取Discuz!NT代码
        /// </summary>
        /// <param name="id">Discuz!NT代码Id</param>
        /// <returns></returns>
        public static DataTable GetBBCode(int id)
        {
            return DatabaseProvider.GetInstance().GetBBCode(id);
        }

        /// <summary>
        /// 更新Discuz!NT代码
        /// </summary>
        /// <param name="available">是否启用</param>
        /// <param name="tag">标签</param>
        /// <param name="icon">图标</param>
        /// <param name="replacement">替换内容</param>
        /// <param name="example">示例</param>
        /// <param name="explanation">说明</param>
        /// <param name="param">参数</param>
        /// <param name="nest">嵌套次数</param>
        /// <param name="paramsDescription">参数描述</param>
        /// <param name="paramsDefaultValue">参数默认值</param>
        /// <param name="id">Id</param>
        public static void UpdateBBCode(int available, string tag, string icon, string replacement, string example,
            string explanation, string param, string nest, string paramsDescription, string paramsDefaultValue, int id)
        {
            DatabaseProvider.GetInstance().UpdateBBCCode(available, tag, icon, replacement, example, explanation, param,
                nest, paramsDescription, paramsDefaultValue, id);
        }

        /// <summary>
        /// 删除Discuz!NT代码
        /// </summary>
        /// <param name="idList">Id列表</param>
        public static void DeleteBBCode(string idList)
        {
            DatabaseProvider.GetInstance().DeleteBBCode(idList);
        }

        /// <summary>
        /// 批量更新BBCode的可用性
        /// </summary>
        /// <param name="status">可用性状态</param>
        /// <param name="idList">BBCodeId列表</param>
        public static void BatchUpdateAvailable(int status, string idList)
        {
            DatabaseProvider.GetInstance().SetBBCodeAvailableStatus(idList, status);
        }

        /// <summary>
        /// 创建Discuz!NT代码
        /// </summary>
        /// <param name="available">是否启用</param>
        /// <param name="tag">标签</param>
        /// <param name="icon">图标</param>
        /// <param name="replacement">替换内容</param>
        /// <param name="example">示例</param>
        /// <param name="explanation">说明</param>
        /// <param name="param">参数</param>
        /// <param name="nest">嵌套次数</param>
        /// <param name="paramsDescription">参数描述</param>
        /// <param name="paramsDefaultValue">参数默认值</param>
        public static void CreateBBCCode(int available, string tag, string icon, string replacement, string example,
            string explanation, string param, string nest, string paramsDescript, string paramsDefvalue)
        {
            DatabaseProvider.GetInstance().AddBBCCode(available, tag, icon, replacement, example, explanation, 
                param, nest, paramsDescript, paramsDefvalue);
        }
    }
}
