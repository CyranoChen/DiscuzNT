using System;
using System.Web;

namespace Discuz.Space.Entities
{
	/// <summary>
	/// �ռ�ģ��Ĳ����ӿ�
	/// </summary>
	public interface ISpaceCommand
	{
        /// <summary>
        /// ���ģ���ύ����
        /// </summary>
        /// <param name="httpContext">��ǰhttpContext����</param>
        /// <returns>�������¼��ص�����(�������ܣ��¸��汾�Ľ�)</returns>
        string GetModulePost(HttpContext httpContext);        
        /// <summary>
        /// ɾ��ģ��ʱ����Ϊ����Ҫ��ɾ�����ݿ�����������
        /// </summary>
        void RemoveModule();
	}
}
