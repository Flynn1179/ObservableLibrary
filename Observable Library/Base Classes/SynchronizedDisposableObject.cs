// <copyright file="SynchronizedDisposableObject.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// Defines a base class for an object that implements the <see cref="INotifyDisposed"/> and <see cref="ISynchronizedObject"/> interfaces.
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    internal abstract class SynchronizedDisposableObject : DisposableObject, ISynchronizedObject
    {
        [NonSerialized]
        private readonly SynchronizationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedDisposableObject"/> class.
        /// </summary>
        protected SynchronizedDisposableObject()
            : this(SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedDisposableObject"/> class with the specified context.
        /// </summary>
        /// <param name="newContext">The <see cref="SynchronizationContext"/> used for event invocation.</param>
        protected SynchronizedDisposableObject(SynchronizationContext newContext)
        {
            this.context = newContext;
        }

        /// <summary>
        /// Gets the synchronization context to raise events on.
        /// </summary>
        public SynchronizationContext SynchronizationContext
        {
            [DebuggerStepThrough]
            get => this.context;
        }
    }
}
