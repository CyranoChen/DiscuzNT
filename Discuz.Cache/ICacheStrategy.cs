using System;
using System.Text;

namespace Discuz.Cache
{
    /// <summary>
    /// ����������Խӿ�
    /// </summary>
    public interface ICacheStrategy
    {
        /// <summary>
        /// ���ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        void AddObject(string objId, object o);
        /// <summary>
        /// ���ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        /// <param name="expire">����ʱ��,��λ:��</param>
        void AddObject(string objId, object o, int expire);
        /// <summary>
        /// ���ָ��ID�Ķ���(����ָ���ļ���)
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        /// <param name="files">�������ļ���</param>
        void AddObjectWithFileChange(string objId, object o, string[] files);
        /// <summary>
        /// ���ָ��ID�Ķ���(����ָ����ֵ��)
        /// </summary>
        /// <param name="objId">�����</param>
        /// <param name="o">�������</param>
        /// <param name="dependKey">������</param>
        void AddObjectWithDepend(string objId, object o, string[] dependKey);
        /// <summary>
        /// �Ƴ�ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        void RemoveObject(string objId);
        /// <summary>
        /// ����ָ��ID�Ķ���
        /// </summary>
        /// <param name="objId">�����</param>
        /// <returns></returns>
        object RetrieveObject(string objId);
        /// <summary>
        /// ����ʱ��,��λ����
        /// </summary>
        int TimeOut { set;get;}
        /// <summary>
        /// ��յ��л�������
        /// </summary>
        void FlushAll();       
   }
}
