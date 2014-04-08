using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 短消息接收设置
    /// </summary>
    public enum ReceivePMSettingType
    {
        /// <summary>
        /// 不接收短消息并且无短消息提示框
        /// </summary>
        ReceiveNone = 0,
        /// <summary>
        /// 接收系统短消息无提示框
        /// </summary>
        ReceiveSystemPM = 1,
        /// <summary>
        /// 接收用户短消息无提示框
        /// </summary>
        ReceiveUserPM = 2,
        /// <summary>
        /// 接收所有短消息无提示框
        /// </summary>
        ReceiveAllPM = 3,
        /// <summary>
        /// 接收系统短消息有提示框
        /// </summary>
        ReceiveSystemPMWithHint = 5,
        /// <summary>
        /// 接收用户短消息有提示框
        /// </summary>
        ReceiveUserPMWithHint = 6,
        /// <summary>
        /// 接收所有短消息有提示框
        /// </summary>
        ReceiveAllPMWithHint = 7
    }
}
