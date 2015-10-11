using System;
using System.Data;
using System.Data.Common;
using System.Text;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 广告操作类
    /// </summary>
    public class Advertisements
    {
        /// <summary>
        /// 按查询字符串得到广告列表
        /// </summary>
        /// <param name="selectstr">查询字符串</param>
        /// <returns>广告列表</returns>
        private static AdShowInfo[] GetAdsTable(string selectstr)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/Advertisements") as DataTable;

            if (dt == null)
            {
                dt = Discuz.Data.Advertisenments.GetAdsTable();
                cache.AddObject("/Forum/Advertisements", dt);
            }

            DataRow[] drs = dt.Select(selectstr);
            AdShowInfo[] adarray = new AdShowInfo[drs.Length];

            for (int i = 0; i < drs.Length; i++)
            {
                adarray[i] = new AdShowInfo();
                adarray[i].Advid = Utils.StrToInt(drs[i]["advid"].ToString(), 0);
                adarray[i].Displayorder = Utils.StrToInt(drs[i]["displayorder"].ToString(), 0);
                adarray[i].Code = drs[i]["code"].ToString().Trim();
                adarray[i].Parameters = drs[i]["parameters"].ToString().Trim();
            }
            return adarray;
        }


        /// <summary>
        /// 根据参数生成查询字符串
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="fid">版块id</param>
        /// <param name="adtype">广告类型</param>
        /// <returns>查询字符串</returns>
        public static string GetSelectStr(string pagename, int fid, AdType adtype)
        {
            string typestr = Convert.ToInt16(adtype).ToString();

            string selectstr = "";

            if (!Utils.StrIsNullOrEmpty(pagename) && pagename == "indexad")
                selectstr = ",首页,";
            else
            {
                if (fid > 0)
                    selectstr += "," + fid + ",";
            }

            if (selectstr == "")
                if (Convert.ToInt16(adtype) > 10)
                    selectstr = "type='" + typestr + "'";
                else
                    selectstr = "type='" + typestr + "'  AND [targets] Like '%全部%'";
            else
                selectstr = "type='" + typestr + "'  AND ([targets] Like '%" + selectstr + "%' OR [targets] Like '%全部%')";

            return selectstr;
        }


        /// <summary>
        /// 获得头部横幅广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetHeaderAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.HeaderAd));
        }


        /// <summary>
        /// 返回头部随机一条横幅广告	
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static string GetOneHeaderAd(string pagename, int forumid)
        {
            string result = "";

            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.HeaderAd));
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 返回尾部横幅广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetFooterAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.FooterAd));
        }

        /// <summary>
        /// 获取随机广告信息
        /// </summary>
        /// <param name="selectstr">查询字符串</param>
        /// <returns>随机广告信息</returns>
        private static string GetRandomAds(string selectstr)
        {
            string result = "";

            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 返回尾部随机一条横幅广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static string GetOneFooterAd(string pagename, int forumid)
        {
            return GetRandomAds(GetSelectStr(pagename, forumid, AdType.FooterAd));
        }

        /// <summary>
        /// 随机返回一条页内横幅广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static List<string> GetPageAd(string pagename, int forumid)
        {
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.PageAd));

            if (adshowArray.Length < 1)
                return new List<string>();

            List<string> list = new List<string>();
            foreach (AdShowInfo curadshow in adshowArray)
            {
                list.Add(curadshow.Code);
            }
            return list;
        }

        /// <summary>
        /// 返回页内文字广告列表
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetPageWordAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.PageWordAd));
        }


        /// <summary>
        /// 返回页内文字广告html字符串
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static string[] GetPageWordAd(string pagename, int forumid)
        {
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.PageWordAd));

            if (adshowArray.Length < 1)
                return new string[0];

            List<string> list = new List<string>();

            foreach (AdShowInfo curadshow in adshowArray)
            {
                list.Add(curadshow.Code);
            }
            return list.ToArray();
        }


        /// <summary>
        /// 返回帖内广告列表
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetInPostAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd));
        }


        /// <summary>
        /// 获取帖内广告个数
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告个数</returns>
        public static int GetInPostAdCount(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd)).Length;
        }

        /// <summary>
        /// 返回帖内广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <param name="templatepath">模板路径</param>
        /// <param name="count">总数</param>
        /// <returns>帖内广告内容</returns>
        public static string GetInPostAd(string pagename, int forumid, string templatepath, int count)
        {
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd));
            StringBuilder sb = new StringBuilder();

            if (adshowArray.Length > 0)
            {
                sb.Append("<div style=\"display: none;\" id=\"ad_none\">\r\n");
                //帖内下方的广告
                sb.Append(GetAdShowInfo(adshowArray, count, 0));
                //帖内上方的广告
                sb.Append(GetAdShowInfo(adshowArray, count, 1));
                //帖内右方的广告
                sb.Append(GetAdShowInfo(adshowArray, count, 2));

                sb.Append("</div><script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取显示广告信息
        /// </summary>
        /// <param name="adshowArray">显示的广告数据</param>
        /// <param name="count">总数</param>
        /// <param name="inPostAdType">帖内广告类型 0:帖内下方的广告   1:帖内上方的广告  2:帖内右方的广告</param>
        /// <returns>广告信息</returns>
        public static string GetAdShowInfo(AdShowInfo[] adshowArray, int count, int inPostAdType)
        {
            string adsMsg = "";
            Random random = new Random();

            for (int i = 1; i <= count; i++)
            {
                List<AdShowInfo> tmp = new List<AdShowInfo>();
                string[] parameter;

                foreach (AdShowInfo adshow in adshowArray)
                {
                    parameter = Utils.SplitString(adshow.Parameters.Trim(), "|", 9);
                    if (Utils.StrToInt(parameter[7], -1) == inPostAdType)
                    {
                        //parameter[8]: possibleflooridlist
                        if (Utils.InArray(i.ToString(), parameter[8], ",") || parameter[8] == "0")
                            tmp.Add(adshow);
                    }
                }

                if (tmp.Count > 0)
                {
                    switch (inPostAdType)
                    {
                        case 0: //帖内下方的广告
                            adsMsg += string.Format("<div class=\"ad_textlink1\" id=\"ad_thread1_{0}_none\">{1}</div>\r\n", i, tmp[random.Next(0, tmp.Count)].Code); break;
                        case 1: //帖内上方的广告
                            adsMsg += string.Format("<div class=\"ad_textlink2\" id=\"ad_thread2_{0}_none\">{1}</div>\r\n", i, tmp[random.Next(0, tmp.Count)].Code); break;
                        default: //帖内右方的广告
                            adsMsg += string.Format("<div class=\"ad_pip\" id=\"ad_thread3_{0}_none\">{1}</div>\r\n", i, tmp[random.Next(0, tmp.Count)].Code); break;
                    }
                }
            }
            return adsMsg;
        }



        /// <summary>
        /// 返回浮动广告列表
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>浮动广告内容</returns>
        public static AdShowInfo[] GetFloatAdList(string pagename, int forumid)
        {
            return GetAdsTable(GetSelectStr(pagename, forumid, AdType.FloatAd));
        }

        /// <summary>
        /// 返回浮动广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>浮动广告内容</returns>
        public static string GetFloatAd(string pagename, int forumid)
        {
            string[] parameter = new string[] { };
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.FloatAd));
            string result = GetAdsMsg(adshowArray, ref parameter);
            if (Utils.StrIsNullOrEmpty(result))
                return "";

            return "<script type='text/javascript'>theFloaters.addItem('floatAdv',10,'(document.body.clientHeight>document.documentElement.clientHeight ? document.documentElement.clientHeight :document.body.clientHeight)-" + (parameter[3] == "" ? "0" : parameter[3]) + "-40','" + result + "');</script>";
        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="adshowArray">广告信息数组</param>
        /// <param name="parameter">要返回的广告参数</param>
        /// <returns>广告信息</returns>
        private static string GetAdsMsg(AdShowInfo[] adshowArray, ref string[] parameter)
        {
            if (adshowArray.Length == 0)    //当没有浮动广告时直接返回空串
                return "";

            string result = "";
            int number = 0;
            if (adshowArray.Length > 1)    //如果浮动广告的条数大于一条，则随机显示一条
                number = new Random().Next(0, adshowArray.Length);

            if (Utils.StrIsNullOrEmpty(adshowArray[number].Parameters))
                return "";

            //初始化参数
            parameter = adshowArray[number].Parameters.Split('|');

            if (parameter[0].ToLower() == "flash")
                result = string.Format("<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"{0}\" height=\"{1}\"><param name=\"movie\" value=\"{2}\" /><param name=\"quality\" value=\"high\" /><param name=\"wmode\" value=\"opaque\">{3}<embed src=\"{2}\" wmode=\"opaque\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"{0}\" height=\"{1}\"></embed></object>",
                    parameter[2], parameter[3], parameter[1], adshowArray[number].Code);
            else
                result = adshowArray[number].Code;

            return result;
        }

        /// <summary>
        /// 返回对联广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>返回广告内容</returns>
        public static string GetDoubleAd(string pagename, int forumid)
        {
            string[] parameter = new string[] { };
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.DoubleAd));
            string result = GetAdsMsg(adshowArray, ref parameter);

            if (Utils.StrIsNullOrEmpty(result))
                return "";

            StringBuilder doublestr = new StringBuilder();

            doublestr.AppendFormat("<script type=\"text/javascript\">");
            doublestr.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv1','(document.body.clientWidth>document.documentElement.clientWidth ? document.documentElement.clientWidth :document.body.clientWidth )-" + (parameter[2] == "" ? "0" : parameter[2]) + "-10',10,'" + result + "<br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\\'hand\\\'\" onClick=\"closeBanner();\">');");
            doublestr.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv2',10,10,'" + result + "<br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\\'hand\\\'\" onClick=\"closeBanner();\">');");
            doublestr.AppendFormat("\n</script>");

            return doublestr.ToString();
        }


        /// <summary>
        /// 返回silverlight广告
        /// </summary>
        /// <param name="templatepath">模板路径</param>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>返回广告内容</returns>
        public static string GetMediaAd(string templatepath, string pagename, int forumid)
        {
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.MediaAd));

            return (adshowArray.Length > 0) ? string.Format(adshowArray[0].Code, templatepath, pagename, forumid) : "";
        }

        /// <summary>
        /// 获取视频广告的参数
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>获取视频广告的参数</returns>
        public static string[] GetMediaAdParams(string pagename, int forumid)
        {
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.MediaAd));

            if (adshowArray.Length > 0)
                return adshowArray[0].Parameters.Split('|');

            return new string[0];
        }

        /// <summary>
        /// 返回帖间通栏广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>返回帖间通栏广告</returns>
        public static string GetOnePostLeaderboardAD(string pagename, int forumid)
        {
            string result = "";

            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.PostLeaderboardAd));
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 返回分类间广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <param name="count">总数</param>
        /// <returns>返回分类间广告</returns>
        public static string GetInForumAd(string pagename, int forumid, List<IndexPageForumInfo> topforum, string templatepath)
        {
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.InForumAd));
            StringBuilder result = new StringBuilder();

            if (adshowArray.Length > 0)
            {
                Random ran = new Random();
                result.Append("<div style=\"display: none\" id=\"ad_none\">\r\n");
                for (int i = 0; i < topforum.Count; i++)
                {
                    if (i >= adshowArray.Length)
                        break;

                    result.AppendFormat("<div class=\"ad_column\" id=\"ad_intercat_{0}_none\">{1}</div>\r\n", topforum[i].Fid, adshowArray[i].Code);
                }
                result.Append("</div><script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取指定楼层的帖内广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <param name="templatepath">模板路径</param>
        /// <param name="floor">获取指定楼层的帖内广告</param>
        public static string GetInPostAdXMLByFloor(string pagename, int forumid, string templatepath, int floor)
        {
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.InPostAd));
            StringBuilder sb = new StringBuilder();

            if (adshowArray.Length > 0)
            {
                //帖内下方的广告
                sb.Append(GetAdShowInfoXMLByFloor(adshowArray, floor, 0));
                //帖内上方的广告
                sb.Append(GetAdShowInfoXMLByFloor(adshowArray, floor, 1));
                //帖内右方的广告
                sb.Append(GetAdShowInfoXMLByFloor(adshowArray, floor, 2));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取显示广告信息
        /// </summary>
        /// <param name="adshowArray">显示的广告数据</param>
        /// <param name="floor">获取指定楼层的帖内广告</param>
        /// <param name="inPostAdType">帖内广告类型 0:帖内下方的广告   1:帖内上方的广告  2:帖内右方的广告</param>
        /// <returns>广告信息</returns>
        public static string GetAdShowInfoXMLByFloor(AdShowInfo[] adshowArray, int floor, int inPostAdType)
        {
            string adsMsg = "";
            Random random = new Random();
            //取帖内下方广告
            List<AdShowInfo> tmp = new List<AdShowInfo>();

            foreach (AdShowInfo adshow in adshowArray)//可用的帖内下方广告
            {
                string[] parameter = Utils.SplitString(adshow.Parameters.ToString().Trim(), "|", 9);

                if (Utils.StrToInt(parameter[7], -1) == inPostAdType)
                {
                    //parameter[8]:possibleflooridlist
                    if (Utils.InArray(floor.ToString(), parameter[8], ",") || parameter[8] == "0")
                        tmp.Add(adshow);
                }
            }
            if (tmp.Count > 0)
            {
                AdShowInfo ad = tmp[random.Next(0, tmp.Count)];
                adsMsg = string.Format("<ad_thread{0}><![CDATA[{1}]]></ad_thread{0}>", inPostAdType, ad.Code);
            }
            return adsMsg;
        }

        /// <summary>
        /// 返回快速发帖广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">板块id</param>
        /// <returns>返回快速发帖广告</returns>
        public static string GetQuickEditorAD(string pagename, int forumid)
        {
            string result = "";
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.QuickEditorAd));

            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 快速编辑器背景广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">板块id</param>
        /// <returns>快速编辑器背景广告</returns>
        public static string[] GetQuickEditorBgAd(string pagename, int forumid)
        {
            string[] result = new string[2] { "", "" };
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr(pagename, forumid, AdType.QuickEditorBgAd));

            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result[1] = adshowArray[number].Parameters.Split('|')[1];
                result[0] = adshowArray[number].Parameters.Split('|')[4];
            }
            return result;
        }

        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="available">是否生效</param>
        /// <param name="type">广告类型</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="title">广告标题</param>
        /// <param name="targets">广告投放范围</param>
        /// <param name="parameters">展现方式</param>
        /// <param name="code">广告内容</param>
        /// <param name="startTime">生效时间</param>
        /// <param name="endTime">结束时间</param>
        public static void CreateAd(int available, string type, int displayorder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            targets = targets.IndexOf("全部") >= 0 ? ",全部," : "," + targets + ",";

            Discuz.Data.Advertisenments.CreateAd(available, type, displayorder, title, targets, parameters, code, startTime.IndexOf("1900") >= 0 ? "1900-1-1" : startTime, endTime.IndexOf("1900") >= 0 ? "2555-1-1" : endTime);

            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
        }

        /// <summary>
        /// 获取全部广告列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdvertisements(int type)
        {
            if (type < 0)
            {
                return Discuz.Data.Advertisenments.GetAdvertisements();
            }
            return Discuz.Data.Advertisenments.GetAdvertisements(type);
        }

        /// <summary>
        /// 删除广告列表            
        /// </summary>
        /// <param name="advIdList">广告列表Id</param>
        public static void DeleteAdvertisementList(string advIdList)
        {
            if (Utils.IsNumericList(advIdList))
            {
                Discuz.Data.Advertisenments.DeleteAdvertisementList(advIdList);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
            }
        }

        /// <summary>
        /// 更新广告可用状态
        /// </summary>
        /// <param name="aidList">广告Id</param>
        /// <param name="available"></param>
        /// <returns></returns>
        public static int UpdateAdvertisementAvailable(string aidList, int available)
        {
            if (Utils.IsNumericList(aidList) && (available == 0 || available == 1))
            {
                int result = Discuz.Data.Advertisenments.UpdateAdvertisementAvailable(aidList, available);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
                return result;
            }
            else
                return 0;
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="adId">广告Id</param>
        /// <param name="available">是否生效</param>
        /// <param name="type">广告类型</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="title">广告标题</param>
        /// <param name="targets">广告投放范围</param>
        /// <param name="parameters">展现方式</param>
        /// <param name="code">广告内容</param>
        /// <param name="startTime">生效时间</param>
        /// <param name="endTime">结束时间</param>
        public static void UpdateAdvertisement(int adId, int available, string type, int displayorder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            if (adId > 0)
            {
                startTime = startTime.IndexOf("1900") >= 0 ? "1900-1-1" : startTime;
                endTime = endTime.IndexOf("1900") >= 0 ? "2555-1-1" : endTime;
                targets = targets.IndexOf("全部") >= 0 ? ",全部," : "," + targets + ",";

                Data.Advertisenments.UpdateAdvertisement(adId, available, type, displayorder, title, targets, parameters, code, startTime, endTime);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
            }
        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="aid">广告Id</param>
        /// <returns></returns>
        public static DataTable GetAdvertisement(int aid)
        {
            return aid > 0 ? Data.Advertisenments.GetAdvertisement(aid) : new DataTable();
        }


        /// <summary>
        /// 返回聚合首页广告
        /// </summary>
        /// <param name="adType">广告类型</param>
        /// <returns>返回帖间通栏广告</returns>
        public static string GetWebSiteAd(AdType adType)
        {
            string result = "";
            AdShowInfo[] adshowArray = GetAdsTable(GetSelectStr("", 0, adType));
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }
    }
}
