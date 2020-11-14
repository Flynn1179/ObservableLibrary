// <copyright file="InternalExtensions.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines extension methods internal to this project. Most of these are intended to be in-lined.
    /// </summary>
    internal static class InternalExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the object is null.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if the object is null.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfNull(this object parameter, string name)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(name);
            }

            Contract.Assert(parameter is not null);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null or empty.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if the string is null or empty.</exception>
        /// <param name="message">The error message to include in the exception. Defaults to 'Argument out of range'.</param>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfNullOrEmpty(this string parameter, string name, string message = "Parameter must contain a non-empty string")
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(name, message);
            }

            if (parameter.Length == 0)
            {
                throw new ArgumentException(message, name);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the parameter is outside the specified range.
        /// </summary>
        /// <typeparam name="T">The type of parameter to check.</typeparam>
        /// <param name="parameter">The parameter to check the value of.</param>
        /// <param name="name">The name of the parameter to check.</param>
        /// <param name="minimum">The minimum value acceptable as the parameter, inclusive.</param>
        /// <param name="maximum">The maximum value acceptable as the parameter, inclusive.</param>
        /// <param name="message">The error message to include in the exception. Defaults to 'Argument out of range'.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the parameter is out of range.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfOutOfRange<T>(this T parameter, string name, T minimum, T maximum, string message = "Argument out of range")
            where T : IComparable
        {
            parameter.ThrowIfNull(name);

            // It's a programmer error if these are null, they don't need checking in release mode.
            Debug.Assert(minimum != null, "Null passed to out of range check as minumum");
            Debug.Assert(maximum != null, "Null passed to out of range check as maximum");
            if (parameter.CompareTo(minimum) < 0 || parameter.CompareTo(maximum) > 0)
            {
                throw new ArgumentOutOfRangeException(name, message);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the parameter does not meet a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to check the value of.</param>
        /// <param name="name">The name of the parameter to check.</param>
        /// <param name="predicate">A predicate to test the parameter against.</param>
        /// <param name="message">The message to include in the exception if the check fails.</param>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfCheckFails<T>(this T parameter, string name, Func<T, bool> predicate, string message = "Argument is not valid.")
        {
            predicate.ThrowIfNull(nameof(predicate));
            if (!predicate(parameter))
            {
                throw new ArgumentException(message, name);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the parameter has failed a check.
        /// </summary>
        /// <param name="parameter">The parameter to check the value of. This is passed as parameter 0 to a string.Format call, passing 'message' as the format string.</param>
        /// <param name="name">The name of the parameter to check.</param>
        /// <param name="checkPassed">True if an exception should not be raised, false otherwise.</param>
        /// <param name="message">The message to include in the exception if the check failed.</param>
        /// <remarks>This overload exists where the check is trivial to perform in client code, and doesn't need the complexity of a predicate.</remarks>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfCheckFails(this object parameter, string name, bool checkPassed, string message = "Argument is not valid.")
        {
            if (!checkPassed)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, message, parameter), name);
            }
        }

        /// <summary>
        /// Get the value of a named property on the target.
        /// </summary>
        /// <param name="target">The target to find the property on.</param>
        /// <param name="propertyName">The name of the property to get the value of.</param>
        /// <returns>The value on the named property of the target object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the target parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the propertyName parameter is null or empty.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object GetPropertyValue(this object target, string propertyName)
        {
            target.ThrowIfNull(nameof(target));
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));
            System.Reflection.PropertyInfo property = target.GetType().GetProperty(propertyName);
            if (property is null)
            {
                throw new ArgumentException(Properties.Resources.PropertyNotFoundError);
            }

            return property.GetValue(target);
        }
    }
}
