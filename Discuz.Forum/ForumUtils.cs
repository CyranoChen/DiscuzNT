using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing.Drawing2D;

using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;
using Discuz.Common;
using Discuz.Plugin.Preview;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 论坛工具类
    /// </summary>
    public class ForumUtils
    {
        /// <summary>
        /// 验证码生成的取值范围
        /// </summary>
        private static string[] verifycodeRange = { "1","2","3","4","5","6","7","8","9",
                                                    "a","b","c","d","e","f","g",
                                                    "h",    "j","k",    "m","n",
                                                        "p","q",    "r","s","t",
                                                    "u","v","w",    "x","y"
                                                  };
        /// <summary>
        /// 生成验证码所使用的随机数发生器
        /// </summary>
        private static Random verifycodeRandom = new Random();

        /// <summary>
        /// 禁用文字正则式对象
        /// </summary>
        private static Regex r_word;

        private static RegexOptions options = RegexOptions.IgnoreCase;

        public static Regex[] r = new Regex[5];

        /// <summary>
        /// memcached 信息配置类
        /// </summary>
        private static MemCachedConfigInfo mcci = MemCachedConfigs.GetConfig();
        /// <summary>
        /// redis 信息配置类
        /// </summary>
        private static RedisConfigInfo rci = RedisConfigs.GetConfig();


        /// <summary>
        /// 为干扰码声明正则数组
        /// </summary>
        static ForumUtils()
        {
            r[0] = new Regex(@"(\r\n)", options);
            r[1] = new Regex(@"(\n)", options);
            r[2] = new Regex(@"(\r)", options);
            r[3] = new Regex(@"(<br( *)(/?)>)", options);
            r[4] = new Regex(@"(</p>)", options);

            string vcode = GeneralConfigs.GetConfig().Verifycode;
            if (vcode.Trim() != string.Empty)
            {
                char[] charArray = vcode.ToCharArray();
                List<string> vcodeList = new List<string>();
                foreach (char c in charArray)
                {
                    vcodeList.Add(c.ToString());
                }
                verifycodeRange = vcodeList.ToArray();
            }
        }


        /// <summary>
        /// 返回论坛用户密码cookie明文
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string GetCookiePassword(string key)
        {
            return DES.Decode(GetCookie("password"), key).Trim();
        }

        /// <summary>
        /// 返回论坛用户密码cookie明文
        /// </summary>
        /// <param name="password">密码密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string GetCookiePassword(string password, string key)
        {
            return DES.Decode(password, key);
        }


        /// <summary>
        /// 返回密码密文
        /// </summary>
        /// <param name="password">密码明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string SetCookiePassword(string password, string key)
        {
            return DES.Encode(password, key);
        }


        /// <summary>
        /// 返回用户安全问题答案的存储数据
        /// </summary>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns></returns>
        public static string GetUserSecques(int questionid, string answer)
        {
            if (questionid > 0)
                return Utils.MD5(answer + Utils.MD5(questionid.ToString())).Substring(15, 8);

            return "";
        }


        #region Cookies

        /// <summary>
        /// 写论坛cookie值
        /// </summary>
        /// <param name="strName">项</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["dnt"];
            if (cookie == null)
            {
                cookie = new HttpCookie("dnt");
                cookie.Values[strName] = Utils.UrlEncode(strValue);
            }
            else
            {
                cookie.Values[strName] = Utils.UrlEncode(strValue);
                if (HttpContext.Current.Request.Cookies["dnt"]["expires"] != null)
                {
                    int expires = Utils.StrToInt(HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString(), 0);
                    if (expires > 0)
                    {
                        cookie.Expires = DateTime.Now.AddMinutes(Utils.StrToInt(HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString(), 0));
                    }
                }
            }

            string cookieDomain = GeneralConfigs.GetConfig().CookieDomain.Trim();
            if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }


        /// <summary>
        /// 写论坛登录用户的cookie
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="passwordkey">用户密码Key</param>
        /// <param name="templateid">用户当前要使用的界面风格</param>
        /// <param name="invisible">用户当前的登录模式(正常或隐身)</param>
        public static void WriteUserCookie(int uid, int expires, string passwordkey, int templateid, int invisible)
        {
            UserInfo userinfo = Discuz.Data.Users.GetUserInfo(uid);
            WriteUserCookie(userinfo, expires, passwordkey, templateid, invisible);
        }

        /// <summary>
        /// 写论坛登录用户的cookie
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="passwordkey">用户密码Key</param>
        /// <param name="templateid">用户当前要使用的界面风格</param>
        /// <param name="invisible">用户当前的登录模式(正常或隐身)</param>
        public static void WriteUserCookie(UserInfo userinfo, int expires, string passwordkey, int templateid, int invisible)
        {
            if (userinfo == null)
                return;

            HttpCookie cookie = new HttpCookie("dnt");
            cookie.Values["userid"] = userinfo.Uid.ToString();
            cookie.Values["password"] = Utils.UrlEncode(SetCookiePassword(userinfo.Password, passwordkey));
            if (Templates.GetTemplateItem(templateid) == null)
            {
                templateid = 0;
                foreach (string strTemplateid in Utils.SplitString(Templates.GetValidTemplateIDList(), ","))
                {
                    if (strTemplateid.Equals(userinfo.Templateid.ToString()))
                    {
                        templateid = userinfo.Templateid;
                        break;
                    }
                }
            }

            //cookie.Values["avatar"] = Utils.UrlEncode(userinfo.Avatar.ToString());
            cookie.Values["tpp"] = userinfo.Tpp.ToString();
            cookie.Values["ppp"] = userinfo.Ppp.ToString();
            cookie.Values["pmsound"] = userinfo.Pmsound.ToString();
            if (invisible != 0 || invisible != 1)
            {
                invisible = userinfo.Invisible;
            }
            cookie.Values["invisible"] = invisible.ToString();

            cookie.Values["referer"] = "index.aspx";
            cookie.Values["sigstatus"] = userinfo.Sigstatus.ToString();
            cookie.Values["expires"] = expires.ToString();
            cookie.Values["userinfotips"] = Utils.GetCookie("dnt", "userinfotips");
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            string cookieDomain = GeneralConfigs.GetConfig().CookieDomain.Trim();
            if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
            {
                cookie.Domain = cookieDomain;
            }

            HttpContext.Current.Response.AppendCookie(cookie);
            if (templateid > 0)
            {
                Utils.WriteCookie(Utils.GetTemplateCookieName(), templateid.ToString(), 999999);
            }
        }

        /// <summary>
        /// 写论坛登录用户的cookie
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="passwordkey">用户密码Key</param>
        public static void WriteUserCookie(int uid, int expires, string passwordkey)
        {
            WriteUserCookie(uid, expires, passwordkey, 0, -1);
        }

        /// <summary>
        /// 写论坛登录用户的cookie
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="passwordkey">用户密码Key</param>
        public static void WriteUserCookie(UserInfo userinfo, int expires, string passwordkey)
        {
            WriteUserCookie(userinfo, expires, passwordkey, 0, -1);
        }

        /// <summary>
        /// 获得论坛cookie值
        /// </summary>
        /// <param name="strName">项</param>
        /// <returns>值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies["dnt"] != null && HttpContext.Current.Request.Cookies["dnt"][strName] != null)
                return Utils.UrlDecode(HttpContext.Current.Request.Cookies["dnt"][strName].ToString());

            return "";
        }


        /// <summary>
        /// 清除论坛登录用户的cookie
        /// </summary>
        public static void ClearUserCookie()
        {
            ClearUserCookie("dnt");
        }

        public static void ClearUserCookie(string cookieName)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Clear();
            cookie.Expires = DateTime.Now.AddYears(-1);
            string cookieDomain = GeneralConfigs.GetConfig().CookieDomain.Trim();
            if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = cookieDomain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>验证码</returns>
        public static string CreateAuthStr(int len)
        {
            int number;
            StringBuilder checkCode = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < len; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    checkCode.Append((char)('0' + (char)(number % 10)));
                else
                    checkCode.Append((char)('A' + (char)(number % 26)));
            }
            return checkCode.ToString();
        }

        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="OnlyNum">是否仅为数字</param>
        /// <returns>string</returns>
        public static string CreateAuthStr(int len, bool OnlyNum)
        {
            int number;
            StringBuilder checkCode = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                if (!OnlyNum)
                    number = verifycodeRandom.Next(0, verifycodeRange.Length);
                else
                    number = verifycodeRandom.Next(0, 10);

                checkCode.Append(verifycodeRange[number]);
            }
            return checkCode.ToString();
        }

        /// <summary>
        /// 创建主题缓存标志文件
        /// </summary>
        /// <returns>bool</returns>
        public static bool CreateTopicCacheInfoFile()
        {
            string infofilepath = Utils.GetMapPath(GetShowTopicCacheDir() + "/cacheinfo.config");
            try
            {
                using (FileStream fs = new FileStream(infofilepath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes("<?xml version=\"1.0\"?>");
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否使用游客缓存页面
        /// </summary>
        /// <param name="pageid">当前分页id</param>
        /// <returns></returns>
        public static bool IsGuestCachePage(int pageid, string pagename)
        {
            //只有在第一页或应用memcached的情况下才可以使用主题游客缓存更多分页信息
            if (pagename == "showtopic")
            {
                if (GeneralConfigs.GetConfig().Guestcachepagetimeout > 0 && (pageid == 1 || (mcci != null && mcci.ApplyMemCached && (pageid > 0 && pageid <= mcci.CacheShowTopicPageNumber))))
                    return true;

                if (GeneralConfigs.GetConfig().Guestcachepagetimeout > 0 && (pageid == 1 || (rci != null && rci.ApplyRedis && (pageid > 0 && pageid <= rci.CacheShowTopicPageNumber))))
                    return true;
            }

            //只有在第一页或应用memcached的情况下才可以使用主题游客缓存更多分页信息
            if (pagename == "showforum")
            {
                if (mcci != null && mcci.ApplyMemCached && pageid > 0 && pageid <= mcci.CacheShowForumPageNumber && mcci.CacheShowForumCacheTime > 0)
                    return true;
                if (rci != null && rci.ApplyRedis && pageid > 0 && pageid <= rci.CacheShowForumPageNumber && rci.CacheShowForumCacheTime > 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 向HTTP输出指定主题id的主题缓存信息
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="timeout">缓存文件的有效时间</param>
        /// <returns>bool</returns>
        public static bool ResponseShowTopicCacheFile(int tid, int pageid)
        {
            //只有在第一页或应用memcached的情况下才可以使用主题游客缓存更多分页信息
            if (IsGuestCachePage(pageid, "showtopic"))
            {
                DNTCache cache = DNTCache.GetCacheService();
                string str = cache.RetrieveObject("/Forum/ShowTopicGuestCachePage/Tid_" + tid + "/Page_" + pageid + "/") as string;
                if (string.IsNullOrEmpty(str))
                    return false;
                System.Web.HttpContext.Current.Response.Write(str);
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 向HTTP输出指定版块id的主题缓存信息
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="timeout">缓存文件的有效时间</param>
        /// <returns>bool</returns>
        public static bool ResponseShowForumCacheFile(int fid, int pageid)
        {
            //只有在第一页或应用memcached的情况下才可以使用主题游客缓存更多分页信息
            if (IsGuestCachePage(pageid, "showforum"))
            {
                DNTCache cache = DNTCache.GetCacheService();
                string str = cache.RetrieveObject("/Forum/ShowForumGuestCachePage/Fid_" + fid + "/Page_" + pageid + "/") as string;
                if (string.IsNullOrEmpty(str))
                    return false;
                System.Web.HttpContext.Current.Response.Write(str);
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                return true;
            }
            return false;
        }


        /// <summary>
        /// 创建主题缓存文件
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="pagetext">缓存的字符串内容</param>
        /// <returns>bool</returns>
        public static bool CreateShowTopicCacheFile(int tid, int pageid, string pagetext)
        {
            //只有在第一页或应用memcached的情况下才可以使用主题游客缓存更多分页信息
            if (IsGuestCachePage(pageid, "showtopic"))
            {
                DNTCache cache = DNTCache.GetCacheService();
                if (!string.IsNullOrEmpty(pagetext))
                {
                    //pagetext = "\r\n<!-- Discuz!NT CachedPage (Created: " + Utils.GetDateTime() + ") -->\r\n" + pagetext;
                    pagetext += "\r\n<!-- Discuz!NT CachedPage (Created: " + Utils.GetDateTime() + ") -->\r\n";
                    cache.AddObject("/Forum/ShowTopicGuestCachePage/Tid_" + tid + "/Page_" + pageid + "/", pagetext, GeneralConfigs.GetConfig().Guestcachepagetimeout * 60);
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// 创建主题缓存文件
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="pagetext">缓存的字符串内容</param>
        /// <returns>bool</returns>
        public static bool CreateShowForumCacheFile(int fid, int pageid, string pagetext)
        {
            //只有在第一页或应用memcached的情况下才可以使用主题游客缓存更多分页信息
            if (IsGuestCachePage(pageid, "showforum"))
            {
                DNTCache cache = DNTCache.GetCacheService();
                if (!string.IsNullOrEmpty(pagetext))
                {
                    //此处与上面CreateShowTopicCacheFile方法的赋值方式不一样，只因为防止在IE（6，7版本）浏览器下被缓存的页面会出现样式问题导致页面变形
                    pagetext += "\r\n<!-- Discuz!NT CachedPage (Created: " + Utils.GetDateTime() + ") -->";
                    cache.AddObject("/Forum/ShowForumGuestCachePage/Fid_" + fid + "/Page_" + pageid + "/", pagetext, mcci.ApplyMemCached ? mcci.CacheShowForumCacheTime * 60 : rci.CacheShowForumCacheTime * 60);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 删除指定id的主题游客缓存
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>bool</returns>
        public static bool DeleteTopicCacheFile(int tid)
        {
            int cachenumber = 1;
            if (mcci != null && mcci.ApplyMemCached)
                cachenumber = mcci.CacheShowTopicPageNumber;
            else if (rci != null && rci.ApplyRedis)
                cachenumber = rci.CacheShowTopicPageNumber;

            for (int pageid = 1; pageid < cachenumber; pageid++)
            {
                DNTCache.GetCacheService().RemoveObject("/Forum/ShowTopicGuestCachePage/Tid_" + tid + "/Page_" + pageid + "/");
            }
            return true;
        }

        /// <summary>
        /// 删除指定id列表的主题游客缓存
        /// </summary>
        /// <param name="tidlist">主题id列表</param>
        /// <returns>int</returns>
        public static int DeleteTopicCacheFile(string tidlist)
        {
            int count = 0;
            string[] strNumber = Utils.SplitString(tidlist, ",");

            foreach (string tid in strNumber)
            {
                if (Utils.IsNumeric(tid) && DeleteTopicCacheFile(Int32.Parse(tid)))
                    count++;
            }
            return count;
        }


        /// <summary>
        /// 返回"查看主题"的页面缓存目录
        /// </summary>
        /// <returns>缓存目录</returns>
        public static string GetShowTopicCacheDir()
        {
            return GetCacheDir("showtopic");
        }


        /// <summary>
        /// 返回指定目录的页面缓存目录
        /// </summary>
        /// <param name="path">目录</param>
        /// <returns>缓存目录</returns>
        public static string GetCacheDir(string path)
        {
            path = path.Trim();
            StringBuilder dir = new StringBuilder();
            dir.Append(BaseConfigs.GetForumPath);
            dir.Append("cache/");
            dir.Append(path);
            string cachedir = dir.ToString();
            if (!Directory.Exists(Utils.GetMapPath(cachedir)))
            {
                Utils.CreateDir(Utils.GetMapPath(cachedir));
            }
            return cachedir;
        }

        /// <summary>
        /// 保存上传头像
        /// </summary>
        /// <param name="MaxAllowFileSize">最大允许的头像文件尺寸(单位:KB)</param>
        /// <returns>保存文件的相对路径</returns>
        public static string SaveRequestAvatarFile(int userid, int MaxAllowFileSize)
        {
            string filename = Path.GetFileName(HttpContext.Current.Request.Files[0].FileName);
            string fileextname = Path.GetExtension(filename).ToLower();
            string filetype = HttpContext.Current.Request.Files[0].ContentType.ToLower();

            // 判断 文件扩展名/文件大小/文件类型 是否符合要求
            if (Utils.InArray(fileextname, ".jpg,.gif,.png") && filetype.StartsWith("image"))
            {
                StringBuilder savedir = new StringBuilder(BaseConfigs.GetForumPath + "avatars/upload/");

                int t1 = (int)((double)userid / (double)10000);
                savedir.Append(t1);
                savedir.Append("/");
                int t2 = (int)((double)userid / (double)200);
                savedir.Append(t2);
                savedir.Append("/");
                if (!Directory.Exists(Utils.GetMapPath(savedir.ToString())))
                    Utils.CreateDir(Utils.GetMapPath(savedir.ToString()));

                string newfilename = savedir.ToString() + userid.ToString() + fileextname;

                if (HttpContext.Current.Request.Files[0].ContentLength <= MaxAllowFileSize)
                {
                    File.Delete(Utils.GetMapPath(savedir.ToString()) + userid.ToString() + ".jpg");
                    File.Delete(Utils.GetMapPath(savedir.ToString()) + userid.ToString() + ".gif");
                    File.Delete(Utils.GetMapPath(savedir.ToString()) + userid.ToString() + ".png");

                    HttpContext.Current.Request.Files[0].SaveAs(Utils.GetMapPath(newfilename));
                    return newfilename;
                }
            }
            return "";
        }

        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkFilename">水印文件名</param>
        /// <param name="watermarkStatus">图片水印位置</param>
        public static void AddImageSignPic(Image img, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Image watermark = new Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
                return;

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                transparency = (watermarkTransparency / 10.0F);


            float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
                img.Save(filename, ici, encoderParams);
            else
                img.Save(filename);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }


        /// <summary>
        /// 增加图片文字水印
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkStatus">图片水印位置</param>
        public static void AddImageSignText(Image img, string filename, string watermarkText, int watermarkStatus, int quality, string fontname, int fontsize)
        {
            Graphics g = Graphics.FromImage(img);
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (float)img.Width * (float).01;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 2:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (float)img.Height * (float).01;
                    break;
                case 3:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 4:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
            }

            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
                img.Save(filename, ici, encoderParams);
            else
                img.Save(filename);

            g.Dispose();
            img.Dispose();
        }

        /// <summary>
        /// 判断是否有上传的文件
        /// </summary>
        /// <returns>是否有上传的文件</returns>
        public static bool IsPostFile()
        {
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                if (HttpContext.Current.Request.Files[i].FileName != "")
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 保存上传的文件
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <param name="MaxAllowFileCount">最大允许的上传文件个数</param>
        /// <param name="MaxSizePerDay">每天允许的附件大小总数</param>
        /// <param name="MaxFileSize">单个最大允许的文件字节数</param>/// 
        /// <param name="TodayUploadedSize">今天已经上传的附件字节总数</param>
        /// <param name="AllowFileType">允许的文件类型, 以string[]形式提供</param>
        /// <param name="config">附件保存方式 0=按年/月/日存入不同目录 1=按年/月/日/论坛存入不同目录 2=按论坛存入不同目录 3=按文件类型存入不同目录</param>
        /// <param name="watermarkstatus">图片水印位置</param>
        /// <param name="filekey">File控件的Key(即Name属性)</param>
        /// <returns>文件信息结构</returns>
        public static AttachmentInfo[] SaveRequestFiles(int forumid, int MaxAllowFileCount, int MaxSizePerDay, int MaxFileSize, int TodayUploadedSize, string AllowFileType, int watermarkstatus, GeneralConfigInfo config, string filekey, bool isImage)
        {
            string[] tmp = Utils.SplitString(AllowFileType, "|");
            string[] allowFileExtName = new string[tmp.Length];
            int[] maxSize = new int[tmp.Length];


            for (int i = 0; i < tmp.Length; i++)
            {
                allowFileExtName[i] = Utils.CutString(tmp[i], 0, tmp[i].LastIndexOf(","));
                maxSize[i] = Utils.StrToInt(Utils.CutString(tmp[i], tmp[i].LastIndexOf(",") + 1), 0);
            }

            int saveFileCount = 0;
            int fCount = HttpContext.Current.Request.Files.Count;

            for (int i = 0; i < fCount; i++)
            {
                if (!HttpContext.Current.Request.Files[i].FileName.Equals("") && HttpContext.Current.Request.Files.AllKeys[i].Equals(filekey))
                {
                    saveFileCount++;
                }
            }

            AttachmentInfo[] attachmentInfo = saveFileCount > 0 ? new AttachmentInfo[saveFileCount] : null;
            if (saveFileCount > MaxAllowFileCount)
                return attachmentInfo;

            saveFileCount = 0;

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < fCount; i++)
            {
                if (!HttpContext.Current.Request.Files[i].FileName.Equals("") && HttpContext.Current.Request.Files.AllKeys[i].Equals(filekey))
                {
                    string fileName = Path.GetFileName(HttpContext.Current.Request.Files[i].FileName);
                    string fileExtName = Utils.CutString(fileName, fileName.LastIndexOf(".") + 1).ToLower();
                    string fileType = HttpContext.Current.Request.Files[i].ContentType.ToLower();
                    int fileSize = HttpContext.Current.Request.Files[i].ContentLength;
                    string newFileName = "";

                    //flash批量上传时无法获取contenttype
                    if (fileType == "application/octet-stream")
                        fileType = GetContentType(fileExtName);

                    attachmentInfo[saveFileCount] = new AttachmentInfo();
                    attachmentInfo[saveFileCount].Sys_noupload = "";

                    // 判断 文件扩展名/文件大小/文件类型 是否符合要求
                    if (!(Utils.IsImgFilename(fileName) && !fileType.StartsWith("image")) && ValidateImage(fileType, HttpContext.Current.Request.Files[i].InputStream))
                    {
                        int extnameid = Utils.GetInArrayID(fileExtName, allowFileExtName);
                        if (extnameid >= 0 && (fileSize <= maxSize[extnameid]) && (MaxFileSize >= fileSize /*|| MaxAllSize == 0*/) &&
                            (MaxSizePerDay - TodayUploadedSize >= fileSize))
                        {
                            TodayUploadedSize = TodayUploadedSize + fileSize;
                            string UploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/");

                            string saveDir = GetAttachmentPath(forumid, config, fileExtName);

                            newFileName = string.Format("{0}{1}{2}.{3}",
                                (Environment.TickCount & int.MaxValue).ToString(),
                                i.ToString(),
                                random.Next(1000, 9999).ToString(),
                                fileExtName);
                            //(Environment.TickCount & int.MaxValue).ToString() + i.ToString() + random.Next(1000, 9999).ToString() + "." + fileextname;

                            //临时文件名称变量. 用于当启动远程附件之后,先上传到本地临时文件夹的路径信息
                            string tempFileName = "";
                            //当支持FTP上传附件且不保留本地附件时
                            if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                            {
                                // 如果指定目录不存在则建立临时路径
                                if (!Directory.Exists(UploadDir + "temp\\"))
                                    Utils.CreateDir(UploadDir + "temp\\");

                                tempFileName = "temp\\" + newFileName;
                            }
                            // 如果指定目录不存在则建立
                            else if (!Directory.Exists(UploadDir + saveDir))
                                Utils.CreateDir(UploadDir + saveDir);

                            newFileName = saveDir + newFileName;

                            try
                            {
                                // 如果是bmp jpg png图片类型
                                if ((fileExtName == "bmp" || fileExtName == "jpg" || fileExtName == "jpeg" || fileExtName == "png") && fileType.StartsWith("image"))
                                {
                                    Image img = Image.FromStream(HttpContext.Current.Request.Files[i].InputStream);

                                    if (config.Attachimgmaxwidth > 0 && img.Width > config.Attachimgmaxwidth)
                                        attachmentInfo[saveFileCount].Sys_noupload = "图片宽度为" + img.Width + ", 系统允许的最大宽度为" + config.Attachimgmaxwidth;
                                    if (config.Attachimgmaxheight > 0 && img.Height > config.Attachimgmaxheight)
                                        attachmentInfo[saveFileCount].Sys_noupload = "图片高度为" + img.Width + ", 系统允许的最大高度为" + config.Attachimgmaxheight;

                                    attachmentInfo[saveFileCount].Width = img.Width;
                                    attachmentInfo[saveFileCount].Height = img.Height;

                                    if (attachmentInfo[saveFileCount].Sys_noupload == "")
                                    {
                                        if (watermarkstatus == 0)
                                        {
                                            //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                            if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                                HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                            else
                                                HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);

                                            attachmentInfo[saveFileCount].Filesize = fileSize;
                                        }
                                        else
                                        {
                                            if (config.Watermarktype == 1 && File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic)))
                                            {
                                                //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                                if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                                    AddImageSignPic(img, UploadDir + tempFileName, Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic), config.Watermarkstatus, config.Attachimgquality, config.Watermarktransparency);
                                                else
                                                    AddImageSignPic(img, UploadDir + newFileName, Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic), config.Watermarkstatus, config.Attachimgquality, config.Watermarktransparency);
                                            }
                                            else
                                            {
                                                string watermarkText;
                                                watermarkText = config.Watermarktext.Replace("{1}", config.Forumtitle);
                                                watermarkText = watermarkText.Replace("{2}", "http://" + DNTRequest.GetCurrentFullHost() + "/");
                                                watermarkText = watermarkText.Replace("{3}", Utils.GetDate());
                                                watermarkText = watermarkText.Replace("{4}", Utils.GetTime());

                                                //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                                if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                                    AddImageSignText(img, UploadDir + tempFileName, watermarkText, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                                                else
                                                    AddImageSignText(img, UploadDir + newFileName, watermarkText, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                                            }

                                            //当支持FTP上传附件且不保留本地附件模式时,则读取临时目录下的文件信息
                                            if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                                attachmentInfo[saveFileCount].Filesize = new FileInfo(UploadDir + tempFileName).Length;
                                            else
                                                attachmentInfo[saveFileCount].Filesize = new FileInfo(UploadDir + newFileName).Length;
                                        }
                                    }
                                }
                                else
                                {
                                    attachmentInfo[saveFileCount].Filesize = fileSize;

                                    //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                    if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }
                            catch
                            {
                                //当上传目录和临时文件夹都没有上传的文件时
                                if (!(Utils.FileExists(UploadDir + tempFileName)) && (!(Utils.FileExists(UploadDir + newFileName))))
                                {
                                    attachmentInfo[saveFileCount].Filesize = fileSize;

                                    //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                    if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }

                            try
                            {
                                //加载文件预览类指定方法
                                IPreview preview = PreviewProvider.GetInstance(fileExtName.Trim());
                                if (preview != null)
                                {
                                    preview.UseFTP = (FTPs.GetForumAttachInfo.Allowupload == 1) ? true : false;

                                    //当支持FTP上传附件且不保留本地附件模式时
                                    if (FTPs.GetForumAttachInfo.Allowupload == 1 && FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                        preview.OnSaved(UploadDir + tempFileName);
                                    else
                                        preview.OnSaved(UploadDir + newFileName);
                                }
                            }
                            catch
                            { }

                            //当支持FTP上传附件时,使用FTP上传远程附件
                            if (FTPs.GetForumAttachInfo.Allowupload == 1)
                            {
                                FTPs ftps = new FTPs();

                                //当不保留本地附件模式时,在上传完成之后删除本地tempfilename文件
                                if (FTPs.GetForumAttachInfo.Reservelocalattach == 0)
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + tempFileName, FTPs.FTPUploadEnum.ForumAttach);
                                else
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + newFileName, FTPs.FTPUploadEnum.ForumAttach);
                            }

                            if (EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Cacheattachfiles.Enable && EntLibConfigs.GetConfig().Cacheattachfiles.Attachpostid > 0)
                                Discuz.Cache.Data.DBCacheService.GetAttachFilesService().UploadFile(UploadDir, newFileName);
                        }
                        else
                        {
                            if (extnameid < 0)
                                attachmentInfo[saveFileCount].Sys_noupload = "文件格式无效";
                            else if (MaxSizePerDay - TodayUploadedSize < fileSize)
                                attachmentInfo[saveFileCount].Sys_noupload = "文件大于今天允许上传的字节数";
                            else if (fileSize > maxSize[extnameid])
                                attachmentInfo[saveFileCount].Sys_noupload = "文件大于该类型附件允许的字节数";
                            else
                                attachmentInfo[saveFileCount].Sys_noupload = "文件大于单个文件允许上传的字节数";
                        }
                    }
                    else
                    {
                        attachmentInfo[saveFileCount].Sys_noupload = "文件格式无效";
                    }
                    //当支持FTP上传附件时
                    if (FTPs.GetForumAttachInfo.Allowupload == 1)
                        attachmentInfo[saveFileCount].Filename = FTPs.GetForumAttachInfo.Remoteurl + "/" + newFileName.Replace("\\", "/");
                    else
                        attachmentInfo[saveFileCount].Filename = newFileName;

                    attachmentInfo[saveFileCount].Description = "";
                    attachmentInfo[saveFileCount].Filetype = fileType;
                    attachmentInfo[saveFileCount].Attachment = fileName;
                    attachmentInfo[saveFileCount].Downloads = 0;
                    attachmentInfo[saveFileCount].Postdatetime = DateTime.Now.ToString();
                    attachmentInfo[saveFileCount].Sys_index = i;
                    attachmentInfo[saveFileCount].Isimage = isImage ? 1 : 0;
                    saveFileCount++;
                }
            }
            return attachmentInfo;
        }

        private static bool ValidateImage(string fileType, Stream inputStream)
        {
            if (inputStream.Length == 0)
                return false;
            if (!fileType.StartsWith("image"))
                return true;
            try
            {
                Image image = Image.FromStream(inputStream);
                image.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }


        #region 获取相应扩展名的ContentType类型
        private static string GetContentType(string fileextname)
        {
            switch (fileextname)
            {
                #region 常用文件类型
                case "jpeg": return "image/jpeg";
                case "jpg": return "image/jpeg";
                case "js": return "application/x-javascript";
                case "jsp": return "text/html";
                case "gif": return "image/gif";
                case "htm": return "text/html";
                case "html": return "text/html";
                case "asf": return "video/x-ms-asf";
                case "avi": return "video/avi";
                case "bmp": return "application/x-bmp";
                case "asp": return "text/asp";
                case "wma": return "audio/x-ms-wma";
                case "wav": return "audio/wav";
                case "wmv": return "video/x-ms-wmv";
                case "ra": return "audio/vnd.rn-realaudio";
                case "ram": return "audio/x-pn-realaudio";
                case "rm": return "application/vnd.rn-realmedia";
                case "rmvb": return "application/vnd.rn-realmedia-vbr";
                case "xhtml": return "text/html";
                case "png": return "image/png";
                case "ppt": return "application/x-ppt";
                case "tif": return "image/tiff";
                case "tiff": return "image/tiff";
                case "xls": return "application/x-xls";
                case "xlw": return "application/x-xlw";
                case "xml": return "text/xml";
                case "xpl": return "audio/scpls";
                case "swf": return "application/x-shockwave-flash";
                case "torrent": return "application/x-bittorrent";
                case "dll": return "application/x-msdownload";
                case "asa": return "text/asa";
                case "asx": return "video/x-ms-asf";
                case "au": return "audio/basic";
                case "css": return "text/css";
                case "doc": return "application/msword";
                case "exe": return "application/x-msdownload";
                case "mp1": return "audio/mp1";
                case "mp2": return "audio/mp2";
                case "mp2v": return "video/mpeg";
                case "mp3": return "audio/mp3";
                case "mp4": return "video/mpeg4";
                case "mpa": return "video/x-mpg";
                case "mpd": return "application/vnd.ms-project";
                case "mpe": return "video/x-mpeg";
                case "mpeg": return "video/mpg";
                case "mpg": return "video/mpg";
                case "mpga": return "audio/rn-mpeg";
                case "mpp": return "application/vnd.ms-project";
                case "mps": return "video/x-mpeg";
                case "mpt": return "application/vnd.ms-project";
                case "mpv": return "video/mpg";
                case "mpv2": return "video/mpeg";
                case "wml": return "text/vnd.wap.wml";
                case "wsdl": return "text/xml";
                case "xsd": return "text/xml";
                case "xsl": return "text/xml";
                case "xslt": return "text/xml";
                case "htc": return "text/x-component";
                case "mdb": return "application/msaccess";
                case "zip": return "application/zip";
                case "rar": return "application/x-rar-compressed";
                #endregion

                case "*": return "application/octet-stream";
                case "001": return "application/x-001";
                case "301": return "application/x-301";
                case "323": return "text/h323";
                case "906": return "application/x-906";
                case "907": return "drawing/907";
                case "a11": return "application/x-a11";
                case "acp": return "audio/x-mei-aac";
                case "ai": return "application/postscript";
                case "aif": return "audio/aiff";
                case "aifc": return "audio/aiff";
                case "aiff": return "audio/aiff";
                case "anv": return "application/x-anv";
                case "awf": return "application/vnd.adobe.workflow";
                case "biz": return "text/xml";
                case "bot": return "application/x-bot";
                case "c4t": return "application/x-c4t";
                case "c90": return "application/x-c90";
                case "cal": return "application/x-cals";
                case "cat": return "application/vnd.ms-pki.seccat";
                case "cdf": return "application/x-netcdf";
                case "cdr": return "application/x-cdr";
                case "cel": return "application/x-cel";
                case "cer": return "application/x-x509-ca-cert";
                case "cg4": return "application/x-g4";
                case "cgm": return "application/x-cgm";
                case "cit": return "application/x-cit";
                case "class": return "java/*";
                case "cml": return "text/xml";
                case "cmp": return "application/x-cmp";
                case "cmx": return "application/x-cmx";
                case "cot": return "application/x-cot";
                case "crl": return "application/pkix-crl";
                case "crt": return "application/x-x509-ca-cert";
                case "csi": return "application/x-csi";
                case "cut": return "application/x-cut";
                case "dbf": return "application/x-dbf";
                case "dbm": return "application/x-dbm";
                case "dbx": return "application/x-dbx";
                case "dcd": return "text/xml";
                case "dcx": return "application/x-dcx";
                case "der": return "application/x-x509-ca-cert";
                case "dgn": return "application/x-dgn";
                case "dib": return "application/x-dib";
                case "dot": return "application/msword";
                case "drw": return "application/x-drw";
                case "dtd": return "text/xml";
                case "dwf": return "application/x-dwf";
                case "dwg": return "application/x-dwg";
                case "dxb": return "application/x-dxb";
                case "dxf": return "application/x-dxf";
                case "edn": return "application/vnd.adobe.edn";
                case "emf": return "application/x-emf";
                case "eml": return "message/rfc822";
                case "ent": return "text/xml";
                case "epi": return "application/x-epi";
                case "eps": return "application/x-ps";
                case "etd": return "application/x-ebx";
                case "fax": return "image/fax";
                case "fdf": return "application/vnd.fdf";
                case "fif": return "application/fractals";
                case "fo": return "text/xml";
                case "frm": return "application/x-frm";
                case "g4": return "application/x-g4";
                case "gbr": return "application/x-gbr";
                case "gcd": return "application/x-gcd";

                case "gl2": return "application/x-gl2";
                case "gp4": return "application/x-gp4";
                case "hgl": return "application/x-hgl";
                case "hmr": return "application/x-hmr";
                case "hpg": return "application/x-hpgl";
                case "hpl": return "application/x-hpl";
                case "hqx": return "application/mac-binhex40";
                case "hrf": return "application/x-hrf";
                case "hta": return "application/hta";
                case "htt": return "text/webviewhtml";
                case "htx": return "text/html";
                case "icb": return "application/x-icb";
                case "ico": return "application/x-ico";
                case "iff": return "application/x-iff";
                case "ig4": return "application/x-g4";
                case "igs": return "application/x-igs";
                case "iii": return "application/x-iphone";
                case "img": return "application/x-img";
                case "ins": return "application/x-internet-signup";
                case "isp": return "application/x-internet-signup";
                case "IVF": return "video/x-ivf";
                case "java": return "java/*";
                case "jfif": return "image/jpeg";
                case "jpe": return "application/x-jpe";
                case "la1": return "audio/x-liquid-file";
                case "lar": return "application/x-laplayer-reg";
                case "latex": return "application/x-latex";
                case "lavs": return "audio/x-liquid-secure";
                case "lbm": return "application/x-lbm";
                case "lmsff": return "audio/x-la-lms";
                case "ls": return "application/x-javascript";
                case "ltr": return "application/x-ltr";
                case "m1v": return "video/x-mpeg";
                case "m2v": return "video/x-mpeg";
                case "m3u": return "audio/mpegurl";
                case "m4e": return "video/mpeg4";
                case "mac": return "application/x-mac";
                case "man": return "application/x-troff-man";
                case "math": return "text/xml";
                case "mfp": return "application/x-shockwave-flash";
                case "mht": return "message/rfc822";
                case "mhtml": return "message/rfc822";
                case "mi": return "application/x-mi";
                case "mid": return "audio/mid";
                case "midi": return "audio/mid";
                case "mil": return "application/x-mil";
                case "mml": return "text/xml";
                case "mnd": return "audio/x-musicnet-download";
                case "mns": return "audio/x-musicnet-stream";
                case "mocha": return "application/x-javascript";
                case "movie": return "video/x-sgi-movie";
                case "mpw": return "application/vnd.ms-project";
                case "mpx": return "application/vnd.ms-project";
                case "mtx": return "text/xml";
                case "mxp": return "application/x-mmxp";
                case "net": return "image/pnetvue";
                case "nrf": return "application/x-nrf";
                case "nws": return "message/rfc822";
                case "odc": return "text/x-ms-odc";
                case "out": return "application/x-out";
                case "p10": return "application/pkcs10";
                case "p12": return "application/x-pkcs12";
                case "p7b": return "application/x-pkcs7-certificates";
                case "p7c": return "application/pkcs7-mime";
                case "p7m": return "application/pkcs7-mime";
                case "p7r": return "application/x-pkcs7-certreqresp";
                case "p7s": return "application/pkcs7-signature";
                case "pc5": return "application/x-pc5";
                case "pci": return "application/x-pci";
                case "pcl": return "application/x-pcl";
                case "pcx": return "application/x-pcx";
                case "pdf": return "application/pdf";
                case "pdx": return "application/vnd.adobe.pdx";
                case "pfx": return "application/x-pkcs12";
                case "pgl": return "application/x-pgl";
                case "pic": return "application/x-pic";
                case "pko": return "application/vnd.ms-pki.pko";
                case "pl": return "application/x-perl";
                case "plg": return "text/html";
                case "pls": return "audio/scpls";
                case "plt": return "application/x-plt";
                case "pot": return "application/vnd.ms-powerpoint";
                case "ppa": return "application/vnd.ms-powerpoint";
                case "ppm": return "application/x-ppm";
                case "pps": return "application/vnd.ms-powerpoint";
                case "pr": return "application/x-pr";
                case "prf": return "application/pics-rules";
                case "prn": return "application/x-prn";
                case "prt": return "application/x-prt";
                case "ps": return "application/x-ps";
                case "ptn": return "application/x-ptn";
                case "pwz": return "application/vnd.ms-powerpoint";
                case "r3t": return "text/vnd.rn-realtext3d";
                case "ras": return "application/x-ras";
                case "rat": return "application/rat-file";
                case "rdf": return "text/xml";
                case "rec": return "application/vnd.rn-recording";
                case "red": return "application/x-red";
                case "rgb": return "application/x-rgb";
                case "rjs": return "application/vnd.rn-realsystem-rjs";
                case "rjt": return "application/vnd.rn-realsystem-rjt";
                case "rlc": return "application/x-rlc";
                case "rle": return "application/x-rle";
                case "rmf": return "application/vnd.adobe.rmf";
                case "rmi": return "audio/mid";
                case "rmj": return "application/vnd.rn-realsystem-rmj";
                case "rmm": return "audio/x-pn-realaudio";
                case "rmp": return "application/vnd.rn-rn_music_package";
                case "rms": return "application/vnd.rn-realmedia-secure";
                case "rmx": return "application/vnd.rn-realsystem-rmx";
                case "rnx": return "application/vnd.rn-realplayer";
                case "rp": return "image/vnd.rn-realpix";
                case "rpm": return "audio/x-pn-realaudio-plugin";
                case "rsml": return "application/vnd.rn-rsml";
                case "rt": return "text/vnd.rn-realtext";
                case "rtf": return "application/msword";
                case "rv": return "video/vnd.rn-realvideo";
                case "sam": return "application/x-sam";
                case "sat": return "application/x-sat";
                case "sdp": return "application/sdp";
                case "sdw": return "application/x-sdw";
                case "sit": return "application/x-stuffit";
                case "slb": return "application/x-slb";
                case "sld": return "application/x-sld";
                case "slk": return "drawing/x-slk";
                case "smi": return "application/smil";
                case "smil": return "application/smil";
                case "smk": return "application/x-smk";
                case "snd": return "audio/basic";
                case "sol": return "text/plain";
                case "sor": return "text/plain";
                case "spc": return "application/x-pkcs7-certificates";
                case "spl": return "application/futuresplash";
                case "spp": return "text/xml";
                case "ssm": return "application/streamingmedia";
                case "sst": return "application/vnd.ms-pki.certstore";
                case "stl": return "application/vnd.ms-pki.stl";
                case "stm": return "text/html";
                case "sty": return "application/x-sty";
                case "svg": return "text/xml";
                case "tdf": return "application/x-tdf";
                case "tg4": return "application/x-tg4";
                case "tga": return "application/x-tga";
                case "tld": return "text/xml";
                case "top": return "drawing/x-top";
                case "tsd": return "text/xml";
                case "txt": return "text/plain";
                case "uin": return "application/x-icq";
                case "uls": return "text/iuls";
                case "vcf": return "text/x-vcard";
                case "vda": return "application/x-vda";
                case "vdx": return "application/vnd.visio";
                case "vml": return "text/xml";
                case "vpg": return "application/x-vpeg005";
                case "vsd": return "application/vnd.visio";
                case "vss": return "application/vnd.visio";
                case "vst": return "application/vnd.visio";
                case "vsw": return "application/vnd.visio";
                case "vsx": return "application/vnd.visio";
                case "vtx": return "application/vnd.visio";
                case "vxml": return "text/xml";
                case "wax": return "audio/x-ms-wax";
                case "wb1": return "application/x-wb1";
                case "wb2": return "application/x-wb2";
                case "wb3": return "application/x-wb3";
                case "wbmp": return "image/vnd.wap.wbmp";
                case "wiz": return "application/msword";
                case "wk3": return "application/x-wk3";
                case "wk4": return "application/x-wk4";
                case "wkq": return "application/x-wkq";
                case "wks": return "application/x-wks";
                case "wm": return "video/x-ms-wm";
                case "wmd": return "application/x-ms-wmd";
                case "wmf": return "application/x-wmf";
                case "wmx": return "video/x-ms-wmx";
                case "wmz": return "application/x-ms-wmz";
                case "wp6": return "application/x-wp6";
                case "wpd": return "application/x-wpd";
                case "wpg": return "application/x-wpg";
                case "wpl": return "application/vnd.ms-wpl";
                case "wq1": return "application/x-wq1";
                case "wr1": return "application/x-wr1";
                case "wri": return "application/x-wri";
                case "wrk": return "application/x-wrk";
                case "ws": return "application/x-ws";
                case "ws2": return "application/x-ws";
                case "wsc": return "text/scriptlet";
                case "wvx": return "video/x-ms-wvx";
                case "xdp": return "application/vnd.adobe.xdp";
                case "xdr": return "text/xml";
                case "xfd": return "application/vnd.adobe.xfd";
                case "xfdf": return "application/vnd.adobe.xfdf";
                case "xq": return "text/xml";
                case "xql": return "text/xml";
                case "xquery": return "text/xml";
                case "xwd": return "application/x-xwd";
                case "x_b": return "application/x-x_b";
                case "x_t": return "application/x-x_t";
            }
            return "application/octet-stream";
        }
        #endregion

        /// <summary>
        /// 获得附件存放目录
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="config"></param>
        /// <param name="fileExtName"></param>
        /// <returns></returns>
        private static string GetAttachmentPath(int forumid, GeneralConfigInfo config, string fileExtName)
        {
            StringBuilder saveDir = new StringBuilder("");
            //附件保存方式 0=按年/月/日存入不同目录 1=按年/月/日/论坛存入不同目录 2=按论坛存入不同目录 3=按文件类型存入不同目录
            if (config.Attachsave == 1)
            {
                saveDir.Append(DateTime.Now.ToString("yyyy"));
                saveDir.Append(Path.DirectorySeparatorChar);
                saveDir.Append(DateTime.Now.ToString("MM"));
                saveDir.Append(Path.DirectorySeparatorChar);
                saveDir.Append(DateTime.Now.ToString("dd"));
                saveDir.Append(Path.DirectorySeparatorChar);
                saveDir.Append(forumid.ToString());
                saveDir.Append(Path.DirectorySeparatorChar);
            }
            else if (config.Attachsave == 2)
            {
                saveDir.Append(forumid);
                saveDir.Append(Path.DirectorySeparatorChar);
            }
            else if (config.Attachsave == 3)
            {
                saveDir.Append(fileExtName);
                saveDir.Append(Path.DirectorySeparatorChar);
            }
            else
            {
                saveDir.Append(DateTime.Now.ToString("yyyy"));
                saveDir.Append(Path.DirectorySeparatorChar);
                saveDir.Append(DateTime.Now.ToString("MM"));
                saveDir.Append(Path.DirectorySeparatorChar);
                saveDir.Append(DateTime.Now.ToString("dd"));
                saveDir.Append(Path.DirectorySeparatorChar);
            }
            return saveDir.ToString();
        }

        /// <summary>
        /// 返回访问过的论坛的列表html
        /// </summary>
        /// <param name="count">最大显示条数</param>
        /// <returns>列表html</returns>
        public static string GetVisitedForumsOptions(int count)
        {
            if (GetCookie("visitedforums").Trim() == "")
                return "";

            StringBuilder sb = new StringBuilder();
            string[] strfid = Utils.SplitString(GetCookie("visitedforums"), ",");
            for (int fidIndex = 0; fidIndex < strfid.Length; fidIndex++)
            {
                ForumInfo foruminfo = Forums.GetForumInfo(Utils.StrToInt(strfid[fidIndex], 0));
                if (foruminfo != null)
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", strfid[fidIndex], foruminfo.Name);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 增加已访问版块id到历史记录cookie
        /// </summary>
        /// <param name="fid">要加入的版块id</param>
        public static void UpdateVisitedForumsOptions(int fid)
        {
            if (GetCookie("visitedforums").Trim() == "")
                WriteCookie("visitedforums", fid.ToString());
            else
            {
                bool fidExists = false;
                string[] strfid = Utils.SplitString(GetCookie("visitedforums"), ",");
                for (int fidIndex = 0; fidIndex < strfid.Length; fidIndex++)
                {
                    if (strfid[fidIndex] == fid.ToString())
                        fidExists = true;
                }
                if (!fidExists)
                    WriteCookie("visitedforums", fid + "," + GetCookie("visitedforums"));
            }
            return;
        }

        /// <summary>
        /// 替换原始字符串中的脏字词语
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <returns>替换后的结果</returns>
        public static string BanWordFilter(string text)
        {
            StringBuilder sb = new StringBuilder(text);
            string[,] str = Caches.GetBanWordList();
            int count = str.Length / 2;
            for (int i = 0; i < count; i++)
            {
                if (str[i, 1] != "{BANNED}" && str[i, 1] != "{MOD}")
                {
                    sb = new StringBuilder().Append(
                                  Regex.Replace(sb.ToString(), str[i, 0],
                                        str[i, 1],
                                        Utils.GetRegexCompiledOptions()));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 判断字符串是否包含脏字词语
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <returns>如果包含则返回true, 否则反悔false</returns>
        public static bool InBanWordArray(string text)
        {
            string[,] str = Caches.GetBanWordList();

            for (int i = 0; i < str.Length / 2; i++)
            {
                r_word = new Regex(str[i, 0], Utils.GetRegexCompiledOptions());
                foreach (Match m in r_word.Matches(text))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 指定的字符串中是否含有禁止词汇
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool HasBannedWord(string text)
        {
            string[,] str = Caches.GetBanWordList();
            text = RemoveSpecialChars(text, GeneralConfigs.GetConfig().Antispamreplacement);

            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str[i, 1] == "{BANNED}")
                {
                    r_word = new Regex(str[i, 0], Utils.GetRegexCompiledOptions());
                    foreach (Match m in r_word.Matches(text))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得被屏蔽掉的关键词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetBannedWord(string text)
        {
            string[,] str = Caches.GetBanWordList();
            text = RemoveSpecialChars(text, GeneralConfigs.GetConfig().Antispamreplacement);

            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str[i, 1] == "{BANNED}")
                {
                    r_word = new Regex(str[i, 0], Utils.GetRegexCompiledOptions());
                    foreach (Match m in r_word.Matches(text))
                        return m.Groups[0].ToString();
                }
            }
            return string.Empty;

        }

        /// <summary>
        /// 指定的字符串中是否含有需要审核词汇
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>bool</returns>
        public static bool HasAuditWord(string text)
        {
            string[,] str = Caches.GetBanWordList();
            text = RemoveSpecialChars(text, GeneralConfigs.GetConfig().Antispamreplacement);

            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str[i, 1] == "{MOD}")
                {
                    r_word = new Regex(str[i, 0], Utils.GetRegexCompiledOptions());
                    foreach (Match m in r_word.Matches(text))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回星星图片html
        /// </summary>
        /// <param name="starcount">星星总数</param>
        /// <param name="starthreshold">星星阀值</param>
        /// <returns>星星图片html</returns>
        public static string GetStarImg(int starcount, int starthreshold)
        {
            StringBuilder sb = new StringBuilder();
            int len = starcount / (starthreshold * starthreshold);
            for (int i = 0; i < len; i++)
            {
                sb.Append("<img src=\"star_level3.gif\" />");
            }
            starcount = starcount - len * starthreshold * starthreshold;

            len = starcount / starthreshold;
            for (int i = 0; i < len; i++)
            {
                sb.Append("<img src=\"star_level2.gif\" />");
            }
            starcount = starcount - len * starthreshold;

            for (int i = 0; i < starcount; i++)
            {
                sb.Append("<img src=\"star_level1.gif\" />");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 返回当前页面是否是跨站提交
        /// </summary>
        /// <returns>当前页面是否是跨站提交</returns>
        public static bool IsCrossSitePost()
        {
            // 如果不是提交则为true
            if (!DNTRequest.IsPost())
                return true;

            return IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost());
        }

        /// <summary>
        /// 判断是否是跨站提交
        /// </summary>
        /// <param name="urlReferrer">上个页面地址</param>
        /// <param name="host">论坛url</param>
        /// <returns>bool</returns>
        public static bool IsCrossSitePost(string urlReferrer, string host)
        {
            if (urlReferrer.Length < 7)
                return true;

            Uri u = new Uri(urlReferrer);

            return u.Host != host;
        }

        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <returns>string</returns>
        public static string GetAssemblyVersion()
        {
            return string.Format("{0}.{1}.{2}", Utils.AssemblyFileVersion.FileMajorPart, Utils.AssemblyFileVersion.FileMinorPart, Utils.AssemblyFileVersion.FileBuildPart);
        }

        /// <summary>
        /// 帖子中是否包含[hide]...[/hide]
        /// </summary>
        /// <param name="str">帖子内容</param>
        /// <returns>是否包含</returns>
        public static bool IsHidePost(string str)
        {
            Regex regex = new Regex(@"\[hide(=\d+)?\]");
            return (regex.IsMatch(str)) && (str.IndexOf("[/hide]") > 0);
        }

        /// <summary>
        /// 返回显示魔法表情flash层的xhtml字符串
        /// </summary>
        /// <param name="magic">魔法表情id</param>
        /// <returns>string</returns>
        public static string ShowTopicMagic(int magic)
        {
            if (magic <= 0)
                return "";

            return "\r\n<!-- DNT Magic (ID:" + magic.ToString() + ") -->\r\n<object width=\"400\" height=\"300\" id=\"dntmagic\" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0\" align=\"middle\" style=\"position:absolute;z-index:111;display:none;\"> \r\n<param name=\"allowScriptAccess\" value=\"sameDomain\" />\r\n<param name=\"movie\" value=\"magic/swf/" + magic.ToString() + ".swf\" />\r\n<param name=\"loop\" value=\"false\" />\r\n<param name=\"menu\" value=\"false\" />\r\n<param name=\"quality\" value=\"high\" />\r\n<param name=\"scale\" value=\"noscale\" />\r\n<param name=\"salign\" value=\"lt\" />\r\n<param name=\"wmode\" value=\"transparent\" /><param name=\"PLAY\" value=\"false\" /> \r\n<embed src=\"magic/swf/" + magic.ToString() + ".swf\" width=\"400\" height=\"300\" loop=\"false\" align=\"middle\" menu=\"false\" quality=\"high\" scale=\"noscale\" salign=\"lt\" wmode=\"transparent\" play=\"false\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" /> \r\n</object>\r\n<script language=\"javascript\">\r\nfunction $(id){\r\n\treturn document.getElementById(id);\r\n}\r\nfunction playFlash(){\r\n\tdivElement = $('dntmagic');\r\n\tdivElement.style.display = '';\r\n\tdivElement.style.left = (document.documentElement.clientWidth /2 - parseInt(divElement.offsetWidth)/2)+\"px\";\r\n\tdivElement.style.top = (document.documentElement.clientHeight /2 - parseInt(divElement.offsetHeight)/2 )+\"px\";\r\n\tsetTimeout(\"hiddenFlash()\", 5000);\r\n}\r\n\r\nfunction hiddenFlash() {\r\n\t$('dntmagic').style.display = 'none';\r\n}\r\nplayFlash();\r\n</script>\r\n<!-- DNT Magic End -->\r\n";
        }


        /// <summary>
        /// 给帖子内容加上干扰码
        /// </summary>
        /// <param name="message">帖子内容</param>
        /// <returns>加入干扰码后的帖子内容</returns>
        public static string AddJammer(string message)
        {
            Match m;
            string jammer = Caches.GetJammer();

            m = r[0].Match(message);
            if (m.Success)
                message = message.Replace(m.Groups[0].ToString(), jammer + "\r\n");

            m = r[1].Match(message);
            if (m.Success)
                message = message.Replace(m.Groups[0].ToString(), jammer + "\n");

            m = r[2].Match(message);
            if (m.Success)
                message = message.Replace(m.Groups[0].ToString(), jammer + "\r");

            m = r[3].Match(message);
            if (m.Success)
                message = message.Replace(m.Groups[0].ToString(), jammer + "<br />");

            m = r[4].Match(message);
            if (m.Success)
                message = message.Replace(m.Groups[0].ToString(), jammer + "</p>");

            return message + jammer;
        }

        /// <summary>
        /// 是否是过滤的用户名
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringarray"></param>
        /// <returns>bool</returns>
        public static bool IsBanUsername(string str, string stringarray)
        {
            if (Utils.StrIsNullOrEmpty(stringarray))
                return false;

            stringarray = Regex.Escape(stringarray).Replace(@"\*", @"[\s\S]*");
            Regex r;
            foreach (string strarray in Utils.SplitString(stringarray, "\\n"))
            {
                r = new Regex(string.Format("^{0}$", strarray), RegexOptions.IgnoreCase);
                if (r.IsMatch(str) && (!strarray.Trim().Equals("")))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 从cookie中获取转向页
        /// </summary>
        /// <returns>string</returns>
        public static string GetReUrl()
        {
            if (DNTRequest.GetString("reurl").Trim() != "")
            {
                Utils.WriteCookie("reurl", DNTRequest.GetString("reurl").Trim());
                return DNTRequest.GetString("reurl").Trim();
            }
            else
            {
                if (Utils.GetCookie("reurl") == "")
                    return "index.aspx";
                else
                    return Utils.GetCookie("reurl");
            }
        }

        /// <summary>
        /// 是否为有效域
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(string host)
        {
            if (host.IndexOf(".") == -1)
                return false;

            return new Regex(@"^\d+$").IsMatch(host.Replace(".", string.Empty)) ? false : true;
        }


        /// <summary>
        /// 更新路径url串中的扩展名
        /// </summary>
        /// <param name="pathlist">路径url串</param>
        /// <param name="extname">扩展名</param>
        /// <returns>string</returns>
        public static string UpdatePathListExtname(string pathlist, string extname)
        {
            return pathlist.Replace("{extname}", extname);
        }

        public static void CreateTextImage(string filename, string watermarkText, int quality, string fontname, int fontsize, Color fontcolor)
        {
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);

            Bitmap bmp = new Bitmap(100, 50);
            Graphics g = Graphics.FromImage(bmp);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);
            bmp = new Bitmap((int)crSize.Width, (int)crSize.Height);

            g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.Clear(Color.Transparent);
            g.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, crSize.Width, crSize.Height);

            float xpos = 0;
            float ypos = 0;

            g.DrawString(watermarkText, drawFont, new SolidBrush(fontcolor), xpos, ypos);

            bmp.Save(filename, ImageFormat.Png);
            g.Dispose();
            bmp.Dispose();
        }

        /// <summary>
        /// 把两个时间差，三天内的时间用今天，昨天，前天表示，后跟时间，无日期
        /// </summary>
        /// <param name="date">被比较的时间</param>
        /// <param name="currentDateTime">目标时间</param>
        /// <returns></returns>
        public static string ConvertDateTime(string date, DateTime currentDateTime)
        {
            if (Utils.StrIsNullOrEmpty(date))
                return "";

            DateTime time;
            if (!DateTime.TryParse(date, out time))
                return "";

            //如果关闭人性化时间
            if (GeneralConfigs.GetConfig().DateDiff == 0)
                return Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm");

            string result = "";
            if (currentDateTime.Year == time.Year && currentDateTime.Month == time.Month)//如果date和当前时间年份或者月份不一致，则直接返回"yyyy-MM-dd HH:mm"格式日期
            {
                if (DateDiff("hour", time, currentDateTime) <= 10)//如果date和当前时间间隔在10小时内(曾经是3小时)
                {
                    if (DateDiff("hour", time, currentDateTime) > 0)
                        return DateDiff("hour", time, currentDateTime) + "小时前";

                    if (DateDiff("minute", time, currentDateTime) > 0)
                        return DateDiff("minute", time, currentDateTime) + "分钟前";

                    if (DateDiff("second", time, currentDateTime) >= 0)
                        return DateDiff("second", time, currentDateTime) + "秒前";
                    else
                        return "刚才";//为了解决postdatetime时间精度不够导致发帖时间问题的兼容
                }
                else
                {
                    switch (currentDateTime.Day - time.Day)
                    {
                        case 0:
                            result = "今天 " + time.ToString("HH") + ":" + time.ToString("mm");
                            break;
                        case 1:
                            result = "昨天 " + time.ToString("HH") + ":" + time.ToString("mm");
                            break;
                        case 2:
                            result = "前天 " + time.ToString("HH") + ":" + time.ToString("mm");
                            break;
                        default:
                            result = time.ToString("yyyy-MM-dd HH:mm");
                            break;
                    }
                }
            }
            else
                result = time.ToString("yyyy-MM-dd HH:mm");
            return result;
        }


        /// <summary>
        /// 两个时间的差值，可以为秒，小时，天，分钟
        /// </summary>
        /// <param name="Interval">需要得到的时差方式</param>
        /// <param name="StartDate">起始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <returns></returns>
        public static long DateDiff(string Interval, DateTime StartDate, DateTime EndDate)
        {

            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case "second":
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case "minute":
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case "hour":
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case "day":
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case "week":
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case "month":
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case "quarter":
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case "year":
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }

        /// <summary>
        /// 把三天内的时间用今天，昨天，前天表示，后跟时间，无日期
        /// </summary>
        /// <param name="strdate">被转换的时间</param>
        /// <returns></returns>
        public static string ConvertDateTime(string date)
        {
            return ConvertDateTime(date, DateTime.Now);
        }

        /// <summary>
        /// 将扩展积分类别和数量转换为字符串显示
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string ConvertCreditAndAmountToWord(int credit, int amount)
        {
            if (credit < 1 || credit > 8)
                return "0";
            string[] extcreditnames = Scoresets.GetValidScoreName();
            string[] extcreditunits = Scoresets.GetValidScoreUnit();
            return string.Format("{0}:{1}{2}", extcreditnames[credit], amount, extcreditunits[credit]);
        }

        /// <summary>
        /// 移除特殊字符
        /// </summary>
        /// <param name="content"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string RemoveSpecialChars(string content, string keyCharString)
        {
            char[] c = keyCharString.ToCharArray();

            for (int i = 0; i < c.Length; i++)
            {
                content = content.Replace(Convert.ToString(c[i]), string.Empty);
            }

            return content.Replace(" ", "");
        }

        /// <summary>
        /// 写用户积分信息及用户组别COOKIE
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="groupTitle">用户组名</param>
        public static void WriteUserCreditsCookie(ShortUserInfo userInfo, string groupTitle)
        {
            if (userInfo == null)
                return;
            string[] validScoreName = Discuz.Forum.Scoresets.GetValidScoreName();
            string[] scoreUnit = Discuz.Forum.Scoresets.GetValidScoreUnit();
            string scoreInfo = "积分:" + userInfo.Credits + ",";
            scoreInfo += "用户组:" + groupTitle + ",";
            for (int i = 0; i < validScoreName.Length; i++)
            {
                if (!Utils.StrIsNullOrEmpty(validScoreName[i]))
                {
                    switch ("Extcredits" + i)
                    {
                        case "Extcredits1":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits1 + scoreUnit[1] + ","; break;
                        case "Extcredits2":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits2 + scoreUnit[2] + ","; break;
                        case "Extcredits3":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits3 + scoreUnit[3] + ","; break;
                        case "Extcredits4":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits4 + scoreUnit[4] + ","; break;
                        case "Extcredits5":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits5 + scoreUnit[5] + ","; break;
                        case "Extcredits6":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits6 + scoreUnit[6] + ","; break;
                        case "Extcredits7":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits7 + scoreUnit[7] + ","; break;
                        case "Extcredits8":
                            scoreInfo += validScoreName[i] + ": " + userInfo.Extcredits8 + scoreUnit[8] + ","; break;
                    }
                }
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies["dntusertips"];
            if (cookie == null)
            {
                cookie = new HttpCookie("dntusertips");
            }
            cookie.Values["userinfotips"] = Utils.UrlEncode(scoreInfo.TrimEnd(','));
            cookie.Expires = DateTime.Now.AddMinutes(5);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获取cookie中的用户积分和用户组信息
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="groupTitle"></param>
        /// <returns></returns>
        public static string GetUserCreditsCookie(int uId, string groupTitle)
        {
            if (uId == -1)
                return "";
            string userCreditsTips = Utils.UrlDecode(Utils.GetCookie("dntusertips", "userinfotips"));

            if (!string.IsNullOrEmpty(userCreditsTips))
                return userCreditsTips;

            WriteUserCreditsCookie(Users.GetShortUserInfo(uId), groupTitle);
            return Utils.UrlDecode(Utils.GetCookie("dntusertips", "userinfotips"));
        }

        /// <summary>
        /// 设置访问过版块的cookie
        /// </summary>
        /// <param name="fid">要保存的Fid</param>
        public static void SetVisitedForumsCookie(string fid)
        {
            string visitedForums = Utils.GetCookie("visitedforums");

            if (visitedForums == "")    //Cookie不存在
                visitedForums = fid;
            else
            {
                int pos = string.Concat(",", visitedForums, ",").IndexOf(string.Concat(",", fid, ","));

                //未在列表中
                if (pos == -1) visitedForums = fid + "," + visitedForums;

                //位置出现在第一字符之后
                if (pos > 0) visitedForums = fid + "," + visitedForums.Replace(string.Concat(",", fid), "");   //将版块重新附加到开头               

                if (visitedForums.Split(',').Length > 10)   //如果超过10个版块，将多余的版块从列表去掉
                    visitedForums = visitedForums.Substring(0, visitedForums.LastIndexOf(","));
            }
            Utils.WriteCookie("visitedforums", visitedForums, 43200);
        }

    }// end class
}