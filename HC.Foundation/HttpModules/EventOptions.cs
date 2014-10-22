//--------------------------------------------------------------------------------
// 文件描述：事件枚举及事件枚举扩展类
// 文件作者：全体开发人员
// 创建日期：2014-2-12
//--------------------------------------------------------------------------------

using System;

namespace HC.Foundation.HttpModules
{
    /// <summary>
    ///     事件枚举
    /// </summary>
    [Flags]
    public enum EventOptions
    {
        /// <summary>
        ///     没有事件
        /// </summary>
        None = 0,

        /// <summary>
        ///     BeginRequest事件
        /// </summary>
        BeginRequest = 1,

        /// <summary>
        ///     AuthenticateRequest事件
        /// </summary>
        AuthenticateRequest = 2,

        /// <summary>
        ///     PostAuthenticateRequest事件
        /// </summary>
        PostAuthenticateRequest = 4,

        /// <summary>
        ///     AcquireRequestState事件
        /// </summary>
        AcquireRequestState = 8,

        /// <summary>
        ///     PreRequestHandlerExecute事件
        /// </summary>
        PreRequestHandlerExecute = 16,

        /// <summary>
        ///     PostRequestHandlerExecute事件
        /// </summary>
        PostRequestHandlerExecute = 32,

        /// <summary>
        ///     ReleaseRequestState事件
        /// </summary>
        ReleaseRequestState = 64,

        /// <summary>
        ///     EndRequest事件
        /// </summary>
        EndRequest = 128,

        /// <summary>
        ///     PreSendRequestHeaders事件
        /// </summary>
        PreSendRequestHeaders = 256,

        /// <summary>
        ///     Error事件
        /// </summary>
        Error = 512,

        /// <summary>
        ///     All(不包含None)
        /// </summary>
        All =
            BeginRequest | AuthenticateRequest | PostAuthenticateRequest | AcquireRequestState |
            PreRequestHandlerExecute | PostRequestHandlerExecute | ReleaseRequestState | EndRequest |
            PreSendRequestHeaders | Error,
    }

    /// <summary>
    ///     事件枚举扩展类
    /// </summary>
    public static class EventOptionsExtension
    {
        /// <summary>
        ///     是否包含该事件
        /// </summary>
        /// <param name="eventList">事件集合</param>
        /// <param name="eventObject">事件对象</param>
        /// <returns>包含返回True,否则返回False</returns>
        public static bool Contains(this EventOptions eventList, EventOptions eventObject)
        {
            if ((eventList | eventObject) == eventList)
            {
                return true;
            }
            return false;
        }
    }
}