using System;
using System.Data;
using System.Text;

using Discuz.Entity;

namespace Discuz.Plugin.Mall
{
    public abstract class MallPluginBase
    {
        public const string GoodsHotTagJSONPCacheFileName = "cache\\tag\\hottags_mall_cache_jsonp.txt";

        /// <summary>
        /// 操作运行符枚举
        /// </summary>
        public enum OperaCode
        {
            Equal = 1, //等于
            NoEuqal = 2, //不等于
            Morethan = 3, //大于
            MorethanOrEqual = 4, //大于或等于
            Lessthan = 5,  //小于
            LessthanOrEqual = 6 //小于或等于
        }

        /// <summary>
        /// 写入商品热门标签缓存文件
        /// </summary>
        /// <param name="count">数量</param>
        public abstract void WriteHotTagsListForGoodsJSONPCacheFile(int count);

         /// <summary>
        /// 获取指定商品id和相关条件下的商品交易信息(json数据串)
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public abstract StringBuilder GetTradeLogJson(int goodsid, int pagesize, int pageindex, string orderby, int ascdesc);

        /// <summary>
        /// 获取指定商品的交易日志JSON数据
        /// </summary>
        /// <param name="goodsid">指定商品</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public abstract StringBuilder GetLeaveWordJson(int leavewordid);

        /// <summary>
        /// 获取指定商品的交易日志JSON数据
        /// </summary>
        /// <param name="goodsid">指定商品</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public abstract StringBuilder GetLeaveWordJson(int goodsid, int pagesize, int pageindex, string orderby, int ascdesc);
        
        /// <summary>
        /// 获取指定条件的商品评价数据(json格式)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="uidtype">用户id类型(1:卖家, 2:买家, 3:给他人)</param>
        /// <param name="ratetype">评价类型(1:好评, 2:中评, 3:差评)</param>
        /// <param name="filter">进行过滤的条件(oneweek:1周内, onemonth:1月内, sixmonth:半年内, sixmonthago:半年之前)</param>
        /// <returns></returns>
        public abstract string GetGoodsRatesJson(int uid, int uidtype, int ratetype, string filter);

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        public abstract string GetHotGoodsJsonData(int days, int categroyid, int count);

        /// <summary>
        /// 获取热门或新开的店铺信息
        /// </summary>
        /// <param name="shoptype">热门店铺(1:热门店铺, 2 :新开店铺)</param>
        /// <returns></returns>
        public abstract string GetShopInfoJson(int shoptype);

         /// <summary>
        /// 获取推荐商品字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="recommend">推荐信息</param>
        /// <returns>查询条件</returns>
        public abstract string GetGoodsRecommendCondition(int opcode, int recommend);

        /// <summary>
        /// 获取商品类型(全新,二手)字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="quality">数量</param>
        /// <returns>查询条件</returns>
        public abstract string GetGoodsQualityCondition(int opcode, int quality);

        /// <summary>
        /// 获取商品关闭字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="closed">关闭信息</param>
        /// <returns>查询条件</returns>
        public abstract string GetGoodsCloseCondition(int opcode, int closed);

        /// <summary>
        /// 获取商品到期日期条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="day">天数</param>
        /// <returns>查询条件</returns>
        public abstract string GetGoodsExpirationCondition(int opcode, int day);

         /// <summary>
        /// 获取商品开始日期条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="day">天数</param>
        /// <returns>查询条件</returns>
        public abstract string GetGoodsDateLineCondition(int opcode, int day);

        /// <summary>
        /// 获取剩余商品数条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="amount">数量</param>
        /// <returns>查询条件</returns>
        public abstract string GetGoodsRemainCondition(int opcode, int amount);

        /// <summary>
        /// 获取商品显示字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="displayorder">显示信息</param>
        /// <returns>查询条件</returns>
        public abstract string GetGoodsDisplayCondition(int opcode, int displayorder);

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public abstract GoodsinfoCollection GetHotGoods(int days, int categoryid, int count, string condition);

        /// <summary>
        /// 获取指定分类和条件下的商品列表集合
        /// </summary>
        /// <param name="categoryid">商品分类</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序, 1:降序)</param>
        /// <returns></returns>
        public abstract GoodsinfoCollection GetGoodsInfoList(int categoryid, int pagesize, int pageindex, string condition, string orderby, int ascdesc);

        /// <summary>
        /// 获取指定条件的商品信息
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public abstract GoodsinfoCollection GetGoodsInfoList(int pagesize, int pageindex, string condition, string orderby, int ascdesc);

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        public abstract string GetGoodsListJsonData(int categroyid, int order, int topnumber);

          /// <summary>
        /// 获取绑定版块的商品分类
        /// </summary>
        /// <returns></returns>
        public abstract string GetGoodsCategoryWithFid();
   
        /// <summary>
        /// 返回商品所在地数据
        /// </summary>
        /// <returns></returns>
        public abstract DataTable GetLocationsTable();

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="goodsid">商品Id</param>
        public abstract Goodsinfo GetGoodsInfo(int goodsid);

        /// <summary>
        /// 获取指定标签商品数量
        /// </summary>
        /// <param name="tagid">TAG id</param>
        /// <returns></returns>
        public abstract int GetGoodsCountWithSameTag(int tagid);

        /// <summary>
        /// 通过指定的论坛版块id获取相应的商品分类
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <returns></returns>
        public abstract int GetGoodsCategoryIdByFid(int forumid);

        /// <summary>
        /// 获取指定商品标签id的商品信息集合
        /// </summary>
        /// <param name="tagid"></param>
        /// <param name="pageid"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public abstract GoodsinfoCollection GetGoodsWithSameTag(int tagid, int pageid, int pagesize);

        /// <summary>
        /// 获取指定附件id的相关附件信息
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public abstract Goodsattachmentinfo GetGoodsAttachmentsByAid(int aid);

        /// <summary>
        /// 获取指定分类的fid(版块id)字段信息
        /// </summary>
        /// <param name="categoryid">指定的分类id</param>
        /// <returns>(fid)版块id</returns>
        public abstract int GetCategoriesFid(int categoryid);

        /// <summary>
        /// 清除商品分类绑定的版块
        /// </summary>
        /// <param name="fid"></param>
        public abstract void EmptyGoodsCategoryFid(int fid);

        /// <summary>
        /// 生成商品分类的json文件
        /// </summary>
        public abstract void StaticWriteJsonFile();
        
    }
}
