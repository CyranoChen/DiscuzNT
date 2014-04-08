using System;

namespace Discuz.Entity
{
    /// <summary>
    /// SpaceTemplateInfo ��ժҪ˵����
    /// </summary>
    public class SpaceConfigInfo
    {
        public SpaceConfigInfo()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        /// <summary>
        /// ��ǰ�û���ID�ֶ�
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
        /// ��ǰ�û���ID�ֶ�
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
        /// �ռ�����, ��Ϊ������ʾ�����Ŀռ�ı�������
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
        /// �ռ�����, ��Ϊ��������ʾ�����Ŀռ�ı������С�
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
        /// ��־��ʾģʽ ,ժҪ ȫ�� ֻ��ʾ����
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
        /// ÿҳ��ʾ��־ƪ��	
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
        /// Ĭ������Ȩ��,	���������� ��ֹ������ ֻ�е�¼�û�
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
        /// �������ã� 	���������� ��ֹ������ ֻ�е�¼�û�
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
        /// �ض���(��ת)����
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
        /// ����
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
        /// ����·��
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
        /// ��BLOG��
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
        /// ������
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
        /// ������
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
        /// ��������
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
        /// ��������
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
        /// Ĭ��tab
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
        /// �Ƿ�ͨ�� 0Ϊ��ͨ (1�������, 2�����˹ر�, 3�����˺͹���Ա���ر�) 
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
        /// ��ǰ�û���ID�ֶ�
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
        /// ��ǰ�û���ID�ֶ�
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
        /// �ռ�����, ��Ϊ������ʾ�����Ŀռ�ı�������
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
        /// �ռ�����, ��Ϊ��������ʾ�����Ŀռ�ı������С�
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
        /// ��־��ʾģʽ ,ժҪ ȫ�� ֻ��ʾ����
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
        /// ÿҳ��ʾ��־ƪ��	
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
        /// Ĭ������Ȩ��,	���������� ��ֹ������ ֻ�е�¼�û�
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
        /// �������ã� 	���������� ��ֹ������ ֻ�е�¼�û�
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
        /// �ض���(��ת)����
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
        /// ����
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
        /// ����·��
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
        /// ��BLOG��
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
        /// ������
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
        /// ������
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
        /// ��������
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
        /// ��������
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
        /// Ĭ��tab
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
        /// �Ƿ�ͨ�� 0Ϊ��ͨ (1�������, 2�����˹ر�, 3�����˺͹���Ա���ر�) 
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
        /// �ռ�ͷ��
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

        //�����
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


        //������־ID
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


        //������־����
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
