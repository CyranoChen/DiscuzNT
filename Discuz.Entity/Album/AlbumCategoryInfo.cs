using System;

namespace Discuz.Entity
{
	/// <summary>
	/// AlbumCategoryInfo ��ժҪ˵����
	/// </summary>
	public class AlbumCategoryInfo
	{
		public AlbumCategoryInfo()
		{

		}
        private int albumcateid = 0;
        private string title = string.Empty;
        private string description = string.Empty;
        private int albumcount = 0;
        private int displayorder = 0;

        /// <summary>
        /// ������id
        /// </summary>
        public int Albumcateid
        {
            get
            {
                return albumcateid;
            }
            set
            {
                albumcateid = value;
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Title
        {
            get
            {
                return title.Trim();
            }
            set
            {
                title = value;
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Description
        {
            get
            {
                return description.Trim();
            }
            set
            {
                description = value;
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        public int Albumcount
        {
            get
            {
                return albumcount;
            }
            set
            {
                albumcount = value;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Displayorder
        {
            get
            {
                return displayorder;
            }
            set
            {
                displayorder = value;
            }
        }


    }
}
