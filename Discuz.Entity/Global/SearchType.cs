using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// ��������
    /// </summary>
    public enum SearchType : int
    {
        /// <summary>
        /// ȫ������
        /// </summary>
        //All,
        /// <summary>
        /// ������������
        /// </summary>
        DigestTopic,
        /// <summary>
        /// �����������
        /// </summary>
        TopicTitle,
        /// <summary>
        /// �������ӱ���
        /// </summary>
        //PostTitle,
        /// <summary>
        /// ����ȫ�ļ���
        /// </summary>
        PostContent,
        /// <summary>
        /// ����������
        /// </summary>
        AlbumTitle,
        /// <summary>
        /// ������־����
        /// </summary>
        SpacePostTitle,
        /// <summary>
        /// ������������,������̳���ռ䡢���
        /// </summary>
        ByPoster,
        /// <summary>
        /// �Ƿ��Ĳ�����Ϣ
        /// </summary>
        Error
    }
}
