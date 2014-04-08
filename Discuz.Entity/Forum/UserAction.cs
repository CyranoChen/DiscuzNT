using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 定义论坛的动作
    /// </summary>
    public struct UserAction
    {

        #region
        /// <summary>
        /// 根据动作ID返回指定的动作名称
        /// </summary>
        /// <param name="actionid">动作ID</param>
        /// <returns></returns>
        public static string GetActionDescriptionByID(int actionid)
        {
            if (actionid == IndexShow.ActionID)
            {
                return IndexShow.ActionDescription;
            }

            if (actionid == ShowForum.ActionID)
            {
                return ShowForum.ActionDescription;
            }

            if (actionid == ShowTopic.ActionID)
            {
                return ShowTopic.ActionDescription;
            }

            if (actionid == Login.ActionID)
            {
                return Login.ActionDescription;
            }

            if (actionid == PostTopic.ActionID)
            {
                return PostTopic.ActionDescription;
            }

            if (actionid == PostReply.ActionID)
            {
                return PostReply.ActionDescription;
            }

            if (actionid == ActivationUser.ActionID)
            {
                return ActivationUser.ActionDescription;
            }

            if (actionid == Register.ActionID)
            {
                return Register.ActionDescription;
            }

            return "";
        }

        /// <summary>
        /// 通过动作ID得到动作名称
        /// </summary>
        /// <param name="actionid">动作ID</param>
        /// <returns></returns>
        private static string GetActionName(int actionid)
        {
            //从配置文件中读取
            return "";
        }

        /// <summary>
        /// 通过动作ID得到动作描述
        /// </summary>
        /// <param name="actionid">动作描述</param>
        /// <returns></returns>
        private static string GetActionDescription(int actionid)
        {
            //从配置文件中读取
            return "";
        }

        /// <summary>
        /// 首页
        /// </summary>
        public struct IndexShow
        {

            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 1;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ActionID);
                    return m_actionname != "" ? m_actionname : "IndexShow";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "浏览首页";
                }
            }
        }

        /// <summary>
        /// 显示版块
        /// </summary>
        public struct ShowForum
        {
            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 2;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ShowTopic.ActionID);
                    return m_actionname != "" ? m_actionname : "ShowForum";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ShowTopic.ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "浏览论坛板块";
                }
            }
        }

        /// <summary>
        /// 主题显示
        /// </summary>
        public struct ShowTopic
        {
            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 3;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ActionID);
                    return m_actionname != "" ? m_actionname : "ShowTopic";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "浏览帖子";
                }
            }
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        public struct Login
        {
            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 4;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ActionID);
                    return m_actionname != "" ? m_actionname : "Login";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "论坛登陆";
                }
            }
        }

        /// <summary>
        /// 发表主题
        /// </summary>
        public struct PostTopic
        {
            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 5;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ActionID);
                    return m_actionname != "" ? m_actionname : "PostTopic";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "发表主题";
                }
            }
        }

        /// <summary>
        /// 发表回复
        /// </summary>
        public struct PostReply
        {
            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 6;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ActionID);
                    return m_actionname != "" ? m_actionname : "PostReply";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "发表回复";
                }
            }
        }

        /// <summary>
        /// 激活用户
        /// </summary>
        public struct ActivationUser
        {
            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 7;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ActionID);
                    return m_actionname != "" ? m_actionname : "ActivationUser";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "激活用户帐户";
                }
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        public struct Register
        {
            /// <summary>
            /// 动作ID
            /// </summary>
            public static int ActionID
            {
                get
                {
                    return 8;
                }
            }

            /// <summary>
            /// 动作名称
            /// </summary>
            public static string ActionName
            {
                get
                {
                    string m_actionname = GetActionName(ActionID);
                    return m_actionname != "" ? m_actionname : "Register";
                }
            }

            /// <summary>
            /// 动作描述
            /// </summary>
            public static string ActionDescription
            {
                get
                {
                    string m_actiondescription = GetActionDescription(ActionID);
                    return m_actiondescription != "" ? m_actiondescription : "注册新用户";
                }
            }
        }
        #endregion

    }

}
