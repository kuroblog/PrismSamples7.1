
namespace PEF.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 基于 Exception 类的扩展方法
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 获取 Exception 的错误消息，包括内部异常的错误消息
        /// </summary>
        /// <param name="error"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static string[] GetMessages(this Exception error, List<string> messages = null)
        {
            if (messages == null)
            {
                messages = new List<string>();
            }

            messages.Add(error.Message);

            if (error.InnerException != null)
            {
                GetMessages(error.InnerException, messages);
            }

            return messages.ToArray();
        }

        /// <summary>
        /// 获取 Exception 的完整错误消息，会拼接内存异常额错误信息
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetFullMessage(this Exception error) => string.Join(",", error.GetMessages());
    }
}
