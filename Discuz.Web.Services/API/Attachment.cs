using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Discuz.Web.Services.API
{
    public class Attachment
    {
		
		private int aid;	//附件aid
		private int uid;	//对应的帖子书posterid
		private int tid;	//对应的主题tid
		private int pid;	//对应的帖子pid
		private string postdatetime;	//发布时间
		private int readperm;	//所需阅读权限
		private string filename;	//存储文件名
		private string description;	//描述
		private string filetype;	//文件类型
		private long filesize;	//文件尺寸
		private string attachment;	//附件原始文件名
		private int downloads;	//下载次数
        private int attachprice;    //附件的售价


        private int getattachperm; //下载附件权限
        private int attachimgpost; //附件是否为图片
        private int allowread; //附件是否允许读取
        private string preview = string.Empty; //预览信息
        private int isbought = 0;//附件是否被买卖
        private int inserted = 0; //是否已插入到内容
        /// <summary>
        /// 下载附件权限
        /// </summary>
        [JsonProperty("download_perm")]
        [XmlElement("download_perm")]
        public int DownloadPerm
        {
            get { return getattachperm; }
            set { getattachperm = value; }
        }

        /// <summary>
        /// 附件是否为图片
        /// </summary>
        [JsonProperty("is_image")]
        [XmlElement("is_image")]
        public int IsImage
        {
            get { return attachimgpost; }
            set { attachimgpost = value; }
        }

        /// <summary>
        /// 附件是否允许读取
        /// </summary>
        [JsonProperty("allow_read")]
        [XmlElement("allow_read")]
        public int AllowRead
        {
            get { return allowread; }
            set { allowread = value; }
        }

        /// <summary>
        /// 预览信息
        /// </summary>
        [JsonProperty("preview")]
        [XmlElement("preview")]
        public string Preview
        {
            get { return preview; }
            set { preview = value; }
        }

        /// <summary>
        /// 附件是否被买卖
        /// </summary>
        [JsonProperty("is_bought")]
        [XmlElement("is_bought")]
        public int IsBought
        {
            get { return isbought; }
            set { isbought = value; }
        }


        /// <summary>
        /// 是否已插入到内容
        /// </summary>
        [JsonProperty("inserted")]
        [XmlElement("inserted")]
        public int Inserted
        {
            get { return inserted; }
            set { inserted = value; }
        }

		///<summary>
		///附件aid
		///</summary>
        [JsonProperty("aid")]
        [XmlElement("aid")]
		public int AId
		{
			get { return aid;}
			set { aid = value;}
		}
		///<summary>
		///对应的帖子posterid
		///</summary>
        [JsonProperty("uid")]
        [XmlElement("uid")]
        public int UId
		{
			get { return uid;}
			set { uid = value;}
		}
		///<summary>
		///对应的主题tid
		///</summary>
        [JsonProperty("tid")]
        [XmlElement("tid")]
        public int TId
		{
			get { return tid;}
			set { tid = value;}
		}
		///<summary>
		///对应的帖子pid
		///</summary>
        [JsonProperty("pid")]
        [XmlElement("pid")]
        public int PId
		{
			get { return pid;}
			set { pid = value;}
		}
		///<summary>
		///发布时间
		///</summary>
        [JsonProperty("post_date_time")]
        [XmlElement("post_date_time")]
        public string PostDateTime
		{
			get { return postdatetime;}
			set { postdatetime = value;}
		}
		///<summary>
		///所需阅读权限
		///</summary>
        [JsonProperty("read_perm")]
        [XmlElement("read_perm")]
        public int ReadPerm
		{
			get { return readperm;}
			set { readperm = value;}
		}
		///<summary>
		///存储文件名
		///</summary>
        [JsonProperty("file_name")]
        [XmlElement("file_name")]
        public string FileName
		{
			get { return filename;}
			set { filename = value;}
		}
		///<summary>
		///描述
		///</summary>
        [JsonProperty("description")]
        [XmlElement("description")]
        public string Description
		{
			get { return description;}
			set { description = value;}
		}
		///<summary>
		///文件类型
		///</summary>
        [JsonProperty("file_type")]
        [XmlElement("file_type")]
        public string FileType
		{
			get { return filetype;}
			set { filetype = value;}
		}
		///<summary>
		///文件尺寸
		///</summary>
        [JsonProperty("file_size")]
        [XmlElement("file_size")]
        public long FileSize
		{
			get { return filesize;}
			set { filesize = value;}
		}
		///<summary>
		///附件原始文件名
		///</summary>
        [JsonProperty("original_file_name")]
        [XmlElement("original_file_name")]
        public string OriginalFileName
		{
			get { return attachment;}
			set { attachment = value;}
		}
		///<summary>
		///下载次数
		///</summary>
        [JsonProperty("download_count")]
        [XmlElement("download_count")]
        public int DownloadCount
		{
			get { return downloads;}
			set { downloads = value;}
		}

        /// <summary>
        /// 附件的售价
        /// </summary>
        [JsonProperty("price")]
        [XmlElement("price")]
        public int AttachPrice    
        {
			get { return attachprice;}
            set { attachprice = value; }
		}
        

	}    
}
