using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using System.Collections;

namespace Discuz.Web.Services.API
{
    public class ErrorDetails : Hashtable
    {
        public ErrorDetails()
        {
            this.Add(1, "未知错误,请重新提交");
            this.Add(2, "服务目前不可使用");
            this.Add(3, "未知方法或方法内部错误");
            this.Add(4, "整合程序已达到允许的最大同时请求数");
            this.Add(5, "请求来自一个未被当前整合程序允许的远程地址");
            this.Add(100, "指定参数不存在或不是有效参数，请检查是否有必要参数未提交，或者提交的参数值不是合法的");
            this.Add(101, "所提交的api_key未关联到任何设定程序");
            this.Add(102, "session_key已过期或失效,请重定向让用户重新登录并获得新的session_key");
            this.Add(103, "当前会话所提交的call_id没有大于前一次的call_id");
            this.Add(104, "签名(sig)参数不正确");
            this.Add(105, "垃圾信息");
            this.Add(109, "当前不允许注册或不满足注册条件");
            this.Add(111, "Email已存在或非法");
            this.Add(121, "版块RewriteName已存在或非法");
            this.Add(131, "主题已关闭，无法通过API进行回复");
            this.Add(132, "当前用户阅读权限不足，无法查看主题或回复");
            this.Add(133, "版块设置了密码，故无法使用API");
            this.Add(134, "没有版块访问权限");
            this.Add(135, "没有回复权限");
            this.Add(136, "注册时间未达到系统要求的可发帖时间");
            this.Add(137, "标题太长或含有非法字符");
            this.Add(138, "内容长度不符合系统要求");
        }
    }
}
