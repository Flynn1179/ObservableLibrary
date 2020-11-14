// <copyright file="DisposableObservableObject.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines a base class for an object that implements the <see cref="INotifyDisposed"/>, <see cref="INotifyPropertyChanging"/> and <see cref="INotifyPropertyChanged"/> interfaces.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public abstract class DisposableObservableObject : ObservableObject, INotifyDisposed
    {
        private bool isDisposed;

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObservableObject"/> class.
        /// </summary>
        /// <remarks>Does not call 'DoDispose', as finalizing objects should not be setting properties or raising events.</remarks>
        ~DisposableObservableObject()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Occurs before the instance disposes;
        /// </summary>
        /// <remarks>This is not raised if it was the finalizer that disposed it.</remarks>
        public event EventHandler Disposing;

        /// <summary>
        /// Occurs after the instance disposes.
        /// </summary>
        /// <remarks>This is not raised if it was the finalizer that disposed it.</remarks>
        public event EventHandler Disposed;

        /// <summary>
        /// Gets a value indicating whether or not this instance is disposed.
        /// </summary>
        public bool IsDisposed
        {
            [DebuggerStepThrough]
            get => this.isDisposed;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "DoDispose does call Dispose, after raising events & setting the IsDisposed property.")]
        public void Dispose()
        {
            this.DoDispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True if the <see cref="Dispose()"/> method was called, false if the finalizer was called.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Do nothing by default.
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True if the <see cref="Dispose()"/> method was called, false if the finalizer was called.</param>
        private void DoDispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.Disposing.SafeRaise(this, EventArgs.Empty);
            }

            this.Dispose(disposing);
            this.isDisposed = true;

            if (disposing)
            {
                this.Disposed.SafeRaise(this, EventArgs.Empty);
            }
        }
    }
}
