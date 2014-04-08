using System;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Discuz.Entity
{
    /// <summary>
    /// 获取附件的类型
    /// </summary>
    public enum AttachmentFileType 
    { 
        /// <summary>
        /// 文件附件
        /// </summary>
        FileAttachment, 
        /// <summary>
        /// 图片附件
        /// </summary>
        ImageAttachment ,
        /// <summary>
        /// 全部附件
        /// </summary>
        All 
    }
    /// <summary>
    /// 附件信息描述类
    /// </summary>
    public class AttachmentInfo
    {

        private int m_aid;	//附件aid
        private int m_uid;	//对应的帖子书posterid
        private int m_tid;	//对应的主题tid
        private int m_pid;	//对应的帖子pid
        private string m_postdatetime = string.Empty;	//发布时间
        private int m_readperm;	//所需阅读权限
        private string m_filename = string.Empty;	//存储文件名
        private string m_description = string.Empty;	//描述
        private string m_filetype = string.Empty;	//文件类型
        private long m_filesize;	//文件尺寸
        private string m_attachment = string.Empty;	//附件原始文件名
        private int m_downloads;	//下载次数
        private int m_attachprice;    //附件的售价
        private int m_width;	//图片附件宽度
        private int m_height;    //图片附件高度
        private int m_isimage;  //附件是否为flash上传图片，是为1，否为0

        private int m_sys_index;  //非数据库字段,用来代替上传文件所对应上传组件(file)的Index
        private string m_sys_noupload = string.Empty; //非数据库字段,用来存放未被上传的文件名

        ///<summary>
        ///附件aid
        ///</summary>
        public int Aid
        {
            get { return m_aid; }
            set { m_aid = value; }
        }

        ///<summary>
        ///对应的帖子posterid
        ///</summary>
        public int Uid
        {
            get { return m_uid; }
            set { m_uid = value; }
        }

        ///<summary>
        ///对应的主题tid
        ///</summary>
        public int Tid
        {
            get { return m_tid; }
            set { m_tid = value; }
        }

        ///<summary>
        ///对应的帖子pid
        ///</summary>
        public int Pid
        {
            get { return m_pid; }
            set { m_pid = value; }
        }

        ///<summary>
        ///发布时间
        ///</summary>
        public string Postdatetime
        {
            get { return m_postdatetime; }
            set { m_postdatetime = value; }
        }

        ///<summary>
        ///所需阅读权限
        ///</summary>
        public int Readperm
        {
            get { return m_readperm; }
            set { m_readperm = value; }
        }

        ///<summary>
        ///存储文件名
        ///</summary>
        public string Filename
        {
            get { return m_filename.Trim(); }
            set { m_filename = value; }
        }

        ///<summary>
        ///描述
        ///</summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        ///<summary>
        ///文件类型
        ///</summary>
        public string Filetype
        {
            get { return m_filetype.Trim(); }
            set { m_filetype = value; }
        }

        ///<summary>
        ///文件尺寸
        ///</summary>
        public long Filesize
        {
            get { return m_filesize; }
            set { m_filesize = value; }
        }

        ///<summary>
        ///附件原始文件名
        ///</summary>
        public string Attachment
        {
            get { return m_attachment; }
            set { m_attachment = value; }
        }

        ///<summary>
        ///下载次数
        ///</summary>
        public int Downloads
        {
            get { return m_downloads; }
            set { m_downloads = value; }
        }

        /// <summary>
        /// 附件的售价
        /// </summary>
        public int Attachprice
        {
            get { return m_attachprice; }
            set { m_attachprice = value; }
        }

        /// <summary>
        /// 图片附件宽度
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        /// 图片附件高度
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        ///<summary>
        ///非数据库字段,用来代替上传文件所对应上传组件(file)的Index
        ///</summary>
        public int Sys_index
        {
            get { return m_sys_index; }
            set { m_sys_index = value; }
        }

        ///<summary>
        ///非数据库字段,用来存放未被上传的文件名
        ///</summary>
        public string Sys_noupload
        {
            get { return m_sys_noupload; }
            set { m_sys_noupload = value; }
        }

        /// <summary>
        /// 附件是否为flash上传图片，是为1，否为0
        /// </summary>
        public int Isimage
        {
            get { return m_isimage; }
            set { m_isimage = value; }
        }

    }
}
