// <copyright file="SynchronizedObject.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// Defines a base class for an object that implements the <see cref="ISynchronizedObject"/> interface.
    /// </summary>
    /// <remarks>Temporarily made internal, as the use cases for this class really aren't clear enough to be confident of a suitable implementation.</remarks>
    [Serializable]
    [ComVisible(false)]
    internal abstract class SynchronizedObject : ISynchronizedObject
    {
        [NonSerialized]
        private readonly SynchronizationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObject"/> class.
        /// </summary>
        protected SynchronizedObject()
            : this(SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObject"/> class with the specified context.
        /// </summary>
        /// <param name="newContext">The <see cref="SynchronizationContext"/> used for event invocation.</param>
        protected SynchronizedObject(SynchronizationContext newContext)
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
