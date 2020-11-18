// <copyright file="Extensions.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

// This causes multiple false positives in this file.
#pragma warning disable CA1508 // Avoid dead conditional code
namespace Flynn1179.Observable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// General purpose utility and extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handler">The event to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        public static void SafeRaise(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler is null)
            {
                return;
            }

            List<Exception> raisedExceptions = null;
            foreach (Delegate del in handler.GetInvocationList())
            {
                try
                {
                    del.DynamicInvoke(sender, e);
                }
                catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                {
                    if (raisedExceptions is null)
                    {
                        raisedExceptions = new List<Exception>();
                    }

                    raisedExceptions.Add(ex.InnerException);
                }
            }

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handler">The event to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        /// <typeparam name="T">The type of the event arguments for the event.</typeparam>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        public static void SafeRaise<T>(this EventHandler<T> handler, object sender, T e)
        {
            if (handler is null)
            {
                return;
            }

            List<Exception> raisedExceptions = null;
            foreach (Delegate del in handler.GetInvocationList())
            {
                try
                {
                    del.DynamicInvoke(sender, e);
                }
                catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                {
                    if (raisedExceptions is null)
                    {
                        raisedExceptions = new List<Exception>();
                    }

                    raisedExceptions.Add(ex.InnerException);
                }
            }

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handler">The event to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        public static void SafeRaise(this EventHandler handler, ISynchronizedObject sender, EventArgs e)
        {
            sender.ThrowIfNull(nameof(sender));
            if (handler is null)
            {
                return;
            }

            List<Exception> raisedExceptions = null;
            sender.SynchronizationContext.Post(
                state =>
                {
                    foreach (Delegate del in handler.GetInvocationList())
                    {
                        try
                        {
                            del.DynamicInvoke(sender, e);
                        }
                        catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                        {
                            if (raisedExceptions is null)
                            {
                                raisedExceptions = new List<Exception>();
                            }

                            raisedExceptions.Add(ex.InnerException);
                        }
                    }
                }, null);

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handler">The event to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="propertyName">The name of the property relating to this event.</param>
        /// <exception cref="ArgumentNullException">Thrown if propertyName is null or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if sender is not null, and the type of object in the sender does not have the named property.</exception>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        /// <remarks>For static events, the type raising the event is not passed in to this function, so no check can be performed on whether or not the class raising the event has the static property.</remarks>
        public static void SafeRaise(this PropertyChangedEventHandler handler, INotifyPropertyChanged sender, string propertyName)
        {
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));
            sender?.GetType().ValidatePropertyName(propertyName, nameof(propertyName));

            if (handler is null)
            {
                return;
            }

            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            List<Exception> raisedExceptions = null;
            foreach (Delegate del in handler.GetInvocationList())
            {
                try
                {
                    del.DynamicInvoke(sender, e);
                }
                catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                {
                    if (raisedExceptions is null)
                    {
                        raisedExceptions = new List<Exception>();
                    }

                    raisedExceptions.Add(ex.InnerException);
                }
            }

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handlers">A list of <see cref="PropertyChangedEventHandler"/> to invoke.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="propertyName">The name of the property relating to this event.</param>
        /// <exception cref="ArgumentNullException">Thrown if propertyName is null or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if sender is not null, and the type of object in the sender does not have the named property.</exception>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        /// <remarks>For static events, the type raising the event is not passed in to this function, so no check can be performed on whether or not the class raising the event has the static property.</remarks>
        public static void SafeRaise(this IEnumerable<PropertyChangedEventHandler> handlers, INotifyPropertyChanged sender, string propertyName)
        {
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));
            sender?.GetType().ValidatePropertyName(propertyName, nameof(propertyName));

            if (handlers is null || !handlers.Any())
            {
                return;
            }

            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            List<Exception> raisedExceptions = null;
            foreach (PropertyChangedEventHandler del in handlers)
            {
                try
                {
                    del.DynamicInvoke(sender, e);
                }
                catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                {
                    if (raisedExceptions is null)
                    {
                        raisedExceptions = new List<Exception>();
                    }

                    raisedExceptions.Add(ex.InnerException);
                }
            }

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handler">The event to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="propertyName">The name of the property relating to this event.</param>
        /// <exception cref="ArgumentNullException">Thrown if propertyName is null or an empty string.</exception>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        public static void SafeRaise(this PropertyChangingEventHandler handler, INotifyPropertyChanging sender, string propertyName)
        {
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));
            sender?.GetType().ValidatePropertyName(propertyName, nameof(propertyName));

            if (handler is null)
            {
                return;
            }

            PropertyChangingEventArgs e = new PropertyChangingEventArgs(propertyName);
            List<Exception> raisedExceptions = null;
            foreach (Delegate del in handler.GetInvocationList())
            {
                try
                {
                    del.DynamicInvoke(sender, e);
                }
                catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                {
                    if (raisedExceptions is null)
                    {
                        raisedExceptions = new List<Exception>();
                    }

                    raisedExceptions.Add(ex.InnerException);
                }
            }

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handlers">A list of <see cref="PropertyChangingEventHandler"/> to invoke.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="propertyName">The name of the property relating to this event.</param>
        /// <exception cref="ArgumentNullException">Thrown if propertyName is null or an empty string.</exception>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        public static void SafeRaise(this IEnumerable<PropertyChangingEventHandler> handlers, INotifyPropertyChanging sender, string propertyName)
        {
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));
            sender?.GetType().ValidatePropertyName(propertyName, nameof(propertyName));

            if (handlers is null || !handlers.Any())
            {
                return;
            }

            PropertyChangingEventArgs e = new PropertyChangingEventArgs(propertyName);
            List<Exception> raisedExceptions = null;
            foreach (PropertyChangingEventHandler del in handlers)
            {
                try
                {
                    del.DynamicInvoke(sender, e);
                }
                catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                {
                    if (raisedExceptions is null)
                    {
                        raisedExceptions = new List<Exception>();
                    }

                    raisedExceptions.Add(ex.InnerException);
                }
            }

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Raises an event safely, ensuring that all handlers are called on the proper thread, and any exceptions do not prevent other handlers being called.
        /// </summary>
        /// <param name="handler">The event to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        public static void SafeRaise(this Delegate handler, ISynchronizedObject sender, EventArgs e)
        {
            sender.ThrowIfNull(nameof(sender));
            if (handler is null)
            {
                return;
            }

            if (handler.Method.GetParameters() is ParameterInfo[] handlerParams && (handlerParams.Length != 2
                || !handlerParams[0].ParameterType.IsAssignableFrom(sender.GetType())
                || !handlerParams[1].ParameterType.IsAssignableFrom(e?.GetType())))
            {
                throw new ArgumentException("Sender and event args must match handler parameter types.");
            }

            List<Exception> raisedExceptions = null;
            sender.SynchronizationContext.Post(
                state =>
                {
                    foreach (Delegate del in handler.GetInvocationList())
                    {
                        try
                        {
                            del.DynamicInvoke(sender, e);
                        }
                        catch (TargetInvocationException ex) when (ex.InnerException is Exception)
                        {
                            if (raisedExceptions is null)
                            {
                                raisedExceptions = new List<Exception>();
                            }

                            raisedExceptions.Add(ex.InnerException);
                        }
                    }
                }, null);

            // Check list of exceptions is either still null, or not empty.
            Debug.Assert(raisedExceptions is null || raisedExceptions.Any(), "Empty list of exceptions after handling event.");
            if (raisedExceptions is List<Exception>)
            {
                throw new AggregateException(Properties.Resources.SafeRaiseExceptionMessage, raisedExceptions);
            }
        }

        /// <summary>
        /// Checks if a property name is valid for a given type.
        /// </summary>
        /// <param name="type">The type to check the property of.</param>
        /// <param name="propertyName">The name of the property to check.</param>
        /// <param name="paramName">The name of the parameter the property name was passed by.</param>
        /// <exception cref="ArgumentException">Thrown if the propertyName is not the name of a property on the given type.</exception>
        [Conditional("DEBUG")]
        internal static void ValidatePropertyName(this Type type, string propertyName, string paramName)
        {
            const BindingFlags propFlags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;
            if (propertyName.EndsWith("[]", StringComparison.Ordinal))
            {
                if (type.GetProperty(propertyName[0..^2], propFlags) is PropertyInfo property)
                {
                    if ((property.GetIndexParameters()?.Length ?? 0) == 0)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Property '{0}' on type '{1}' is not an indexer.", propertyName, type), paramName);
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "No indexer named '{0}' found on type '{1}'", propertyName, type), paramName);
                }
            }
            else
            {
                if (type.GetProperty(propertyName, propFlags) is PropertyInfo property)
                {
                    if ((property.GetIndexParameters()?.Length ?? 0) > 0)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Property '{0}' on type '{1}' is an indexer.", propertyName, type), paramName);
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "No property named '{0}' found on type '{1}'", propertyName, type), paramName);
                }
            }
        }
    }
}
#pragma warning restore CA1508 // Avoid dead conditional code
