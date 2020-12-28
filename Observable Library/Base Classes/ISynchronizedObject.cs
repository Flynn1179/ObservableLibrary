// <copyright file="ISynchronizedObject.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System.Threading;

    /// <summary>
    /// Defines properties for an object that has a <see cref="System.Threading.SynchronizationContext"/> on which to raise events.
    /// </summary>
    internal interface ISynchronizedObject
    {
        /// <summary>
        /// Gets the synchronization context to raise events on.
        /// </summary>
        SynchronizationContext SynchronizationContext
        {
            get;
        }
    }
}
