using System;

namespace Discuz.Entity
{
    /// <summary>
    /// SpaceTemplateInfo 的摘要说明。
    /// </summary>
    public class SpaceConfigInfo
    {
        public SpaceConfigInfo()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 当前用户的ID字段
        /// </summary>
        private int _spaceID;
        public int SpaceID
        {
            get
            {
                return _spaceID;
            }
            set
            {
                _spaceID = value;
            }
        }



        /// <summary>
        /// 当前用户的ID字段
        /// </summary>
        private int _userID;
        public int UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }


        /// <summary>
        /// 空间名称, 作为标题显示在您的空间的标题栏中
        /// </summary>
        private string _spaceTitle;
        public string Spacetitle
        {
            get
            {
                return _spaceTitle;
            }
            set
            {
                _spaceTitle = value;
            }
        }

        /// <summary>
        /// 空间描述, 作为副标题显示在您的空间的标题栏中。
        /// </summary>
        private string __description;
        public string Description
        {
            get
            {
                return __description;
            }
            set
            {
                __description = value;
            }
        }


        /// <summary>
        /// 日志显示模式 ,摘要 全文 只显示标题
        /// </summary>
        private int __blogDispMode;
        public int BlogDispMode
        {
            get
            {
                return __blogDispMode;
            }
            set
            {
                __blogDispMode = value;
            }
        }

        /// <summary>
        /// 每页显示日志篇数	
        /// </summary>
        private int _bpp;
        public int Bpp
        {
            get
            {
                return _bpp;
            }
            set
            {
                _bpp = value;
            }
        }


        /// <summary>
        /// 默认评论权限,	允许所有人 禁止所有人 只有登录用户
        /// </summary>
        private int _commentPref;
        public int Commentpref
        {
            get
            {
                return _commentPref;
            }
            set
            {
                _commentPref = value;
            }
        }

        /// <summary>
        /// 留言设置： 	允许所有人 禁止所有人 只有登录用户
        /// </summary>
        private int _messagePref;
        public int MessagePref
        {
            get
            {
                return _messagePref;
            }
            set
            {
                _messagePref = value;
            }
        }

        /// <summary>
        /// 重定向(跳转)名称
        /// </summary>
        private string _rewritename;
        public string Rewritename
        {
            get
            {
                return _rewritename;
            }
            set
            {
                _rewritename = value.Trim();
            }
        }

        /// <summary>
        /// 主题
        /// </summary>
        private int _themeID;
        public int ThemeID
        {
            get
            {
                return _themeID;
            }
            set
            {
                _themeID = value;
            }
        }



        /// <summary>
        /// 主题路径
        /// </summary>
        private string _themePath;
        public string ThemePath
        {
            get
            {
                return _themePath;
            }
            set
            {
                _themePath = value;
            }
        }


        /// <summary>
        /// 发BLOG数
        /// </summary>
        private int _postCount;
        public int PostCount
        {
            get
            {
                return _postCount;
            }
            set
            {
                _postCount = value;
            }
        }



        /// <summary>
        /// 评论数
        /// </summary>
        private int _commentCount;
        public int CommentCount
        {
            get
            {
                return _commentCount;
            }
            set
            {
                _commentCount = value;
            }
        }

        /// <summary>
        /// 访问量
        /// </summary>
        private int _visitedTimes;
        public int VisitedTimes
        {
            get
            {
                return _visitedTimes;
            }
            set
            {
                _visitedTimes = value;
            }
        }


        /// <summary>
        /// 创建日期
        /// </summary>
        private DateTime _createDateTime;
        public DateTime CreateDateTime
        {
            get
            {
                return _createDateTime;
            }
            set
            {
                _createDateTime = value;
            }
        }

        /// <summary>
        /// 更新日期
        /// </summary>
        private DateTime _updateDateTime;
        public DateTime UpdateDateTime
        {
            get
            {
                return _updateDateTime;
            }
            set
            {
                _updateDateTime = value;
            }
        }

        /// <summary>
        /// 默认tab
        /// </summary>
        private int _defaultTab;
        public int DefaultTab
        {
            get
            {
                return _defaultTab;
            }
            set
            {
                _defaultTab = value;
            }
        }

        /// <summary>
        /// 是否开通： 0为开通 (1自已完闭, 2所有人关闭, 3所有人和管理员都关闭) 
        /// </summary>
        private SpaceStatusType _status;
        public SpaceStatusType Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
    }


    public class SpaceConfigInfoExt 
    {
        public SpaceConfigInfoExt()
        {
        }

        /// <summary>
        /// 当前用户的ID字段
        /// </summary>
        private int _spaceID;
        public int Spaceid
        {
            get
            {
                return _spaceID;
            }
            set
            {
                _spaceID = value;
            }
        }



        /// <summary>
        /// 当前用户的ID字段
        /// </summary>
        private int _userID;
        public int Userid
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }


        /// <summary>
        /// 空间名称, 作为标题显示在您的空间的标题栏中
        /// </summary>
        private string _spaceTitle;
        public string Spacetitle
        {
            get
            {
                return _spaceTitle;
            }
            set
            {
                _spaceTitle = value;
            }
        }

        /// <summary>
        /// 空间描述, 作为副标题显示在您的空间的标题栏中。
        /// </summary>
        private string __description;
        public string Description
        {
            get
            {
                return __description;
            }
            set
            {
                __description = value;
            }
        }


        /// <summary>
        /// 日志显示模式 ,摘要 全文 只显示标题
        /// </summary>
        private int __blogDispMode;
        public int Blogdispmode
        {
            get
            {
                return __blogDispMode;
            }
            set
            {
                __blogDispMode = value;
            }
        }

        /// <summary>
        /// 每页显示日志篇数	
        /// </summary>
        private int _bpp;
        public int Bpp
        {
            get
            {
                return _bpp;
            }
            set
            {
                _bpp = value;
            }
        }


        /// <summary>
        /// 默认评论权限,	允许所有人 禁止所有人 只有登录用户
        /// </summary>
        private int _commentPref;
        public int Commentpref
        {
            get
            {
                return _commentPref;
            }
            set
            {
                _commentPref = value;
            }
        }

        /// <summary>
        /// 留言设置： 	允许所有人 禁止所有人 只有登录用户
        /// </summary>
        private int _messagePref;
        public int Messagepref
        {
            get
            {
                return _messagePref;
            }
            set
            {
                _messagePref = value;
            }
        }

        /// <summary>
        /// 重定向(跳转)名称
        /// </summary>
        private string _rewritename;
        public string Rewritename
        {
            get
            {
                return _rewritename;
            }
            set
            {
                _rewritename = value;
            }
        }

        /// <summary>
        /// 主题
        /// </summary>
        private int _themeID;
        public int Themeid
        {
            get
            {
                return _themeID;
            }
            set
            {
                _themeID = value;
            }
        }



        /// <summary>
        /// 主题路径
        /// </summary>
        private string _themePath;
        public string Themepath
        {
            get
            {
                return _themePath;
            }
            set
            {
                _themePath = value;
            }
        }


        /// <summary>
        /// 发BLOG数
        /// </summary>
        private int _postCount;
        public int Postcount
        {
            get
            {
                return _postCount;
            }
            set
            {
                _postCount = value;
            }
        }



        /// <summary>
        /// 评论数
        /// </summary>
        private int _commentCount;
        public int Commentcount
        {
            get
            {
                return _commentCount;
            }
            set
            {
                _commentCount = value;
            }
        }

        /// <summary>
        /// 访问量
        /// </summary>
        private int _visitedTimes;
        public int Visitedtimes
        {
            get
            {
                return _visitedTimes;
            }
            set
            {
                _visitedTimes = value;
            }
        }


        /// <summary>
        /// 创建日期
        /// </summary>
        private DateTime _createDateTime;
        public DateTime Createdatetime
        {
            get
            {
                return _createDateTime;
            }
            set
            {
                _createDateTime = value;
            }
        }

        /// <summary>
        /// 更新日期
        /// </summary>
        private DateTime _updateDateTime;
        public DateTime Updatedatetime
        {
            get
            {
                return _updateDateTime;
            }
            set
            {
                _updateDateTime = value;
            }
        }

        /// <summary>
        /// 默认tab
        /// </summary>
        private int _defaultTab;
        public int Defaulttab
        {
            get
            {
                return _defaultTab;
            }
            set
            {
                _defaultTab = value;
            }
        }

        /// <summary>
        /// 是否开通： 0为开通 (1自已完闭, 2所有人关闭, 3所有人和管理员都关闭) 
        /// </summary>
        private SpaceStatusType _status;
        public SpaceStatusType Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        /// <summary>
        /// 空间头像
        /// </summary>
        private string _spacePic;
        public string Spacepic
        {
            get
            {
                return _spacePic;
            }
            set
            {
                _spacePic = value;
            }
        }

        //相册数
        private int _albumCount;
        public int Albumcount
        {
            get
            {
                return _albumCount;
            }
            set
            {
                _albumCount = value;
            }
        }


        //最新日志ID
        private int _postID;
        public int Postid
        {
            get
            {
                return _postID;
            }
            set
            {
                _postID = value;
            }
        }


        //最新日志标题
        private string  _postTitle;
        public string Posttitle
        {
            get
            {
                return _postTitle;
            }
            set
            {
                _postTitle = value;
            }
        }
       
    }
}
