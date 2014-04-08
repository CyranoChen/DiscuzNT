using System;

namespace Discuz.Entity
{
　　/// <summary>
	/// SpaceattachmentsInfo 的摘要说明。
	/// </summary>
	public class SpaceAttachmentInfo
　　{
      /// <summary>
      /// 附件ID
      /// </summary>
		private int _aid;
		public int AID
		{
			get { return _aid; }
			set { _aid = value; }
		}
        /// <summary>
        /// 用户ID
        /// </summary>
　　　　private int _uid;
		public int UID
		{
			get { return _uid; }
			set { _uid = value; }
		}
        /// <summary>
        /// 日志ID
        /// </summary>
　　　　private int _spacePostID;
		public int SpacePostID
		{
			get { return _spacePostID; }
			set { _spacePostID = value; }
		}
        /// <summary>
        /// 上传时间
        /// </summary>
　　　　private DateTime _postDateTime;
		public DateTime PostDateTime
		{
			get { return _postDateTime; }
			set { _postDateTime = value; }
		}
        /// <summary>
        /// 文件名
        /// </summary>
　　　　private string _fileName;
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}
        /// <summary>
        /// 文件类型
        /// </summary>
　　　　private string _fileType;
		public string FileType
		{
			get { return _fileType; }
			set { _fileType = value; }
		}
        /// <summary>
        /// 文件大小
        /// </summary>
　　　　private int _fileSize;
		public int FileSize
		{
			get { return _fileSize; }
			set { _fileSize = value; }
		}
        /// <summary>
        /// 附件名
        /// </summary>
　　　　private string _attachment;
		public string Attachment
		{
			get { return _attachment; }
			set { _attachment = value; }
		}
        /// <summary>
        /// 下载次数
        /// </summary>
　　　　private int _downloads;
		public int Downloads
		{
			get { return _downloads; }
			set { _downloads = value; }
		}
		
	}
}

