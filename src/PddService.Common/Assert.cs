using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using PddService.Common.Exceptions;

namespace PddService.Common
{

    public class Assert
    {
        public static void IsNull(object value, string message = "对象不能为空")
        {

            if (value == null)
            {
                throw new ApiException(message);
            }
        }

        public static void IsNumber(string value, string message = "对象格式错误")
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ApiException(message);
            }
            Regex(value, @"^\d+$", message);
        }

        public static void IsNullOrEmpty(string value, string message = "值不能为空")
        {

            if (string.IsNullOrEmpty(value))
            {
                throw new ApiException(message);
            }
        }

        public static void Regex(string value, string pattern, string message = "输入格式错误")
        {
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
                    {
                        throw new ApiException(message);
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                    throw new ApiException($"正则格式错误：{pattern}");
                    // ignored
                }
            }
        }
        public static void IsNotNull(object value, string message = "对象不能为空")
        {
            if (value != null)
            {
                throw new ApiException(message);
            }
        }
        public static void IsTrue(bool value, string message = null)
        {
            if (value)
            {
                throw new ApiException(message);
            }
        }
        public static void IsFalse(bool value, string message = null)
        {
            if (!value)
            {
                throw new ApiException(message);
            }
        }

        public static void IsEquals<T>(T value1, T value2, string message = "参数异常")
        {
            IsNull(value1);
            IsNull(value2);
            if (value1.Equals(value2))
            {
                throw new ApiException(message);
            }
        }
        public static void IsNotEquals<T>(T value1, T value2, string message = "参数异常")
        {
            IsNull(value1);
            IsNull(value2);
            if (!value1.Equals(value2))
            {
                throw new ApiException(message);
            }
        }
    }
}
