// <copyright file="INotifyDisposed.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;

    /// <summary>
    /// Represents events and properties for an object that implements IDisposable, and also has a queriable <see cref="IsDisposed"/> property and
    /// <see cref="Disposing"/> and <see cref="Disposed"/> events.
    /// </summary>
    public interface INotifyDisposed : IDisposable
    {
        /// <summary>
        /// Occurs before the instance is disposed.
        /// </summary>
        /// <remarks>This is not raised if it was the finalizer that disposed it.</remarks>
        event EventHandler Disposing;

        /// <summary>
        /// Occurs after the instance is disposed.
        /// </summary>
        /// <remarks>This is not raised if it was the finalizer that disposed it.</remarks>
        event EventHandler Disposed;

        /// <summary>
        /// Gets a value indicating whether the instance is disposed or not.
        /// </summary>
        bool IsDisposed
        {
            get;
        }
    }
}
