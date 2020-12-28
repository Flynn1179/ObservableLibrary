// <copyright file="SynchronizedDisposableObservableObject.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// Defines a base class for an object that implements the <see cref="INotifyDisposed"/>, <see cref="INotifyPropertyChanging"/>, <see cref="INotifyPropertyChanged"/> and <see cref="ISynchronizedObject"/> interfaces.
    /// </summary>
    /// <remarks>Temporarily made internal, as the use cases for this class really aren't clear enough to be confident of a suitable implementation.</remarks>
    [Serializable]
    [ComVisible(false)]
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Implementation not to be exposed, but should be maintained until suitable for public use.")]
    internal class SynchronizedDisposableObservableObject : DisposableObservableObject, ISynchronizedObject
    {
        [NonSerialized]
        private readonly SynchronizationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedDisposableObservableObject"/> class.
        /// </summary>
        protected SynchronizedDisposableObservableObject()
            : this(SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedDisposableObservableObject"/> class with the specified context.
        /// </summary>
        /// <param name="newContext">The <see cref="SynchronizationContext"/> used for event invocation.</param>
        protected SynchronizedDisposableObservableObject(SynchronizationContext newContext)
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