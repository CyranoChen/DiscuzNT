using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// ����Ϣ��������
    /// </summary>
    public enum ReceivePMSettingType
    {
        /// <summary>
        /// �����ն���Ϣ�����޶���Ϣ��ʾ��
        /// </summary>
        ReceiveNone = 0,
        /// <summary>
        /// ����ϵͳ����Ϣ����ʾ��
        /// </summary>
        ReceiveSystemPM = 1,
        /// <summary>
        /// �����û�����Ϣ����ʾ��
        /// </summary>
        ReceiveUserPM = 2,
        /// <summary>
        /// �������ж���Ϣ����ʾ��
        /// </summary>
        ReceiveAllPM = 3,
        /// <summary>
        /// ����ϵͳ����Ϣ����ʾ��
        /// </summary>
        ReceiveSystemPMWithHint = 5,
        /// <summary>
        /// �����û�����Ϣ����ʾ��
        /// </summary>
        ReceiveUserPMWithHint = 6,
        /// <summary>
        /// �������ж���Ϣ����ʾ��
        /// </summary>
        ReceiveAllPMWithHint = 7
    }
}
