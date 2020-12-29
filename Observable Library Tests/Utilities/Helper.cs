// <copyright file="Helper.cs" company="Flynn1179">
//   Copyright © 2020 Flynn1179
// </copyright>

namespace Flynn1179.Observable.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    internal static class Helper
    {
        internal static void InvokeProtected(object target, string methodName, params object[] functionParams)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, default, functionParams.Select(param => param?.GetType()).ToArray(), default);
            try
            {
                method.Invoke(target, functionParams);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }
        }
    }
}
