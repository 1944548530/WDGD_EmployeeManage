using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Enum
{
    public class CommonEnum
    {
        public enum IsDeleted
        {
            /// <summary>
            /// 所有
            /// </summary>
            All = -1,
            /// <summary>
            /// 否
            /// </summary>
            No = 0,
            /// <summary>
            /// 是
            /// </summary>
            Yes = 1
        }

        public enum IsLocked
        {
            /// <summary>
            /// 未锁定
            /// </summary>
            UnLocked = 0,
            /// <summary>
            /// 已锁定
            /// </summary>
            Locked = -1
        }

        public enum UserStatus
        {
            /// <summary>
            /// 未指定
            /// </summary>
            All = -1,
            /// <summary>
            /// 已禁用
            /// </summary>
            Forbidden = 0,
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserType
        {
            /// <summary>
            /// 超级管理员
            /// </summary>
            SuperAdministrator = 0,
            /// <summary>
            /// 管理员
            /// </summary>
            Admin = 1,
            /// <summary>
            /// 一般用户
            /// </summary>
            GeneralUser = 2
        }
    }
}