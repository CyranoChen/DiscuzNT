using System;

namespace Discuz.Entity
{
    /// <summary>
    /// ���ֲ������ͣ��緢�������
    /// </summary>
    public enum CreditsOperationType
    {
        /// <summary>
        /// ���߷����������ӵĻ�����, ��������ⱻɾ��, ���߻���Ҳ�ᰴ�˱�׼��Ӧ����
        /// </summary>
        PostTopic = 4,
        /// <summary>
        /// ���߷��»ظ����ӵĻ�����, ����ûظ���ɾ��, ���߻���Ҳ�ᰴ�˱�׼��Ӧ����
        /// </summary>
        PostReply = 5,
        /// <summary>
        /// ���ⱻ���뾫��ʱ��λ�����������ӵĻ�����(���ݾ����������1��3), ��������ⱻ�Ƴ�����, ���߻���Ҳ�ᰴ�˱�׼��Ӧ����
        /// </summary>
        Digest = 6,
        /// <summary>
        /// �û�ÿ�ϴ�һ���������ӵĻ�����, ����ø�����ɾ��, �����߻���Ҳ�ᰴ�˱�׼��Ӧ����
        /// </summary>
        UploadAttachment = 7,
        /// <summary>
        /// �û�ÿ����һ�������۳��Ļ�����. ע��: ��������ο������ظ���, �����Խ����ܱ��ƹ�
        /// </summary>
        DownloadAttachment = 8,
        /// <summary>
        /// �û�ÿ����һ������Ϣ�۳��Ļ�����
        /// </summary>
        SendMessage = 9,
        /// <summary>
        /// �û�ÿ����һ���������������Ϣ�����۳��Ļ�����
        /// </summary>
        Search = 10,
        /// <summary>
        /// �û�ÿ�ɹ�����һ�ν��׺����ӵĻ�����
        /// </summary>
        TradeSucceed = 11,
        /// <summary>
        /// �û�ÿ����һ��ͶƱ�����ӵĻ�����
        /// </summary>
        Vote = 12,
        /// <summary>
        /// �û�ÿ����ɹ�һ���û�ע������ӵĻ�����
        /// </summary>
        Invite = 13,
        /// <summary>
        /// ����Աɾ���û�����ʱ(�ۼ��û�������)
        /// </summary>
        DeletePost = 14,
    }
}
