using System;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Discuz.Entity
{
    /// <summary>
    /// ��ȡ����������
    /// </summary>
    public enum AttachmentFileType 
    { 
        /// <summary>
        /// �ļ�����
        /// </summary>
        FileAttachment, 
        /// <summary>
        /// ͼƬ����
        /// </summary>
        ImageAttachment ,
        /// <summary>
        /// ȫ������
        /// </summary>
        All 
    }
    /// <summary>
    /// ������Ϣ������
    /// </summary>
    public class AttachmentInfo
    {

        private int m_aid;	//����aid
        private int m_uid;	//��Ӧ��������posterid
        private int m_tid;	//��Ӧ������tid
        private int m_pid;	//��Ӧ������pid
        private string m_postdatetime = string.Empty;	//����ʱ��
        private int m_readperm;	//�����Ķ�Ȩ��
        private string m_filename = string.Empty;	//�洢�ļ���
        private string m_description = string.Empty;	//����
        private string m_filetype = string.Empty;	//�ļ�����
        private long m_filesize;	//�ļ��ߴ�
        private string m_attachment = string.Empty;	//����ԭʼ�ļ���
        private int m_downloads;	//���ش���
        private int m_attachprice;    //�������ۼ�
        private int m_width;	//ͼƬ�������
        private int m_height;    //ͼƬ�����߶�
        private int m_isimage;  //�����Ƿ�Ϊflash�ϴ�ͼƬ����Ϊ1����Ϊ0

        private int m_sys_index;  //�����ݿ��ֶ�,���������ϴ��ļ�����Ӧ�ϴ����(file)��Index
        private string m_sys_noupload = string.Empty; //�����ݿ��ֶ�,�������δ���ϴ����ļ���

        ///<summary>
        ///����aid
        ///</summary>
        public int Aid
        {
            get { return m_aid; }
            set { m_aid = value; }
        }

        ///<summary>
        ///��Ӧ������posterid
        ///</summary>
        public int Uid
        {
            get { return m_uid; }
            set { m_uid = value; }
        }

        ///<summary>
        ///��Ӧ������tid
        ///</summary>
        public int Tid
        {
            get { return m_tid; }
            set { m_tid = value; }
        }

        ///<summary>
        ///��Ӧ������pid
        ///</summary>
        public int Pid
        {
            get { return m_pid; }
            set { m_pid = value; }
        }

        ///<summary>
        ///����ʱ��
        ///</summary>
        public string Postdatetime
        {
            get { return m_postdatetime; }
            set { m_postdatetime = value; }
        }

        ///<summary>
        ///�����Ķ�Ȩ��
        ///</summary>
        public int Readperm
        {
            get { return m_readperm; }
            set { m_readperm = value; }
        }

        ///<summary>
        ///�洢�ļ���
        ///</summary>
        public string Filename
        {
            get { return m_filename.Trim(); }
            set { m_filename = value; }
        }

        ///<summary>
        ///����
        ///</summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        ///<summary>
        ///�ļ�����
        ///</summary>
        public string Filetype
        {
            get { return m_filetype.Trim(); }
            set { m_filetype = value; }
        }

        ///<summary>
        ///�ļ��ߴ�
        ///</summary>
        public long Filesize
        {
            get { return m_filesize; }
            set { m_filesize = value; }
        }

        ///<summary>
        ///����ԭʼ�ļ���
        ///</summary>
        public string Attachment
        {
            get { return m_attachment; }
            set { m_attachment = value; }
        }

        ///<summary>
        ///���ش���
        ///</summary>
        public int Downloads
        {
            get { return m_downloads; }
            set { m_downloads = value; }
        }

        /// <summary>
        /// �������ۼ�
        /// </summary>
        public int Attachprice
        {
            get { return m_attachprice; }
            set { m_attachprice = value; }
        }

        /// <summary>
        /// ͼƬ�������
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        /// ͼƬ�����߶�
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        ///<summary>
        ///�����ݿ��ֶ�,���������ϴ��ļ�����Ӧ�ϴ����(file)��Index
        ///</summary>
        public int Sys_index
        {
            get { return m_sys_index; }
            set { m_sys_index = value; }
        }

        ///<summary>
        ///�����ݿ��ֶ�,�������δ���ϴ����ļ���
        ///</summary>
        public string Sys_noupload
        {
            get { return m_sys_noupload; }
            set { m_sys_noupload = value; }
        }

        /// <summary>
        /// �����Ƿ�Ϊflash�ϴ�ͼƬ����Ϊ1����Ϊ0
        /// </summary>
        public int Isimage
        {
            get { return m_isimage; }
            set { m_isimage = value; }
        }

    }
}
