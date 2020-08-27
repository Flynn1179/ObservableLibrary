// <copyright file="SynchronizedObservableObject.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// Defines a base class for an object that implements the <see cref="INotifyPropertyChanging"/>, <see cref="INotifyPropertyChanged"/> and <see cref="ISynchronizedObject"/> interfaces.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class SynchronizedObservableObject : ObservableObject, ISynchronizedObject
    {
        [NonSerialized]
        private readonly SynchronizationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObservableObject"/> class.
        /// </summary>
        protected SynchronizedObservableObject()
            : this(SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedObservableObject"/> class with the specified context.
        /// </summary>
        /// <param name="newContext">The <see cref="SynchronizationContext"/> used for event invocation.</param>
        protected SynchronizedObservableObject(SynchronizationContext newContext)
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
