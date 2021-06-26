// <copyright file="SynchronizedObservableList.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    /// <summary>Represents a thread-safe dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.</summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    [Serializable]
    public class SynchronizedObservableList<T> : SynchronizedDisposableObservableObject, IList<T>, IList, IReadOnlyList<T>, INotifyCollectionChanged
    {
        private const string ReentrancyNotAllowedError = "SynchronizedObservableList reentrancy not allowed";

        private const string ValueWrongTypeError = "value is the wrong type";

        private readonly List<T> items = new ();

        [NonSerialized]
        private readonly ReaderWriterLockSlim itemsLocker = new ();

        [NonSerialized]
        private int monitor;

        /// <summary>Initializes a new instance of the <see cref="SynchronizedObservableList{T}" /> class.</summary>
        public SynchronizedObservableList()
            : base()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SynchronizedObservableList{T}" /> class that contains elements copied from the specified collection.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public SynchronizedObservableList(IEnumerable<T> collection)
            : base()
        {
            this.items.AddRange(collection);
        }

        /// <summary>Initializes a new instance of the <see cref="SynchronizedObservableList{T}" /> class with the specified context.</summary>
        /// <param name="context">The context used for event invokation.</param>
        public SynchronizedObservableList(SynchronizationContext context)
            : base(context)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SynchronizedObservableList{T}" /> class that contains elements copied from the specified collection with the specified context.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <param name="context">The context used for event invokation.</param>
        public SynchronizedObservableList(IEnumerable<T> collection, SynchronizationContext context)
            : base(context)
        {
            this.items.AddRange(collection);
        }

        /// <summary>Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Gets the number of elements actually contained in the <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <returns>The number of elements actually contained in the <see cref="SynchronizedObservableList{T}" />.</returns>
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                this.itemsLocker.EnterReadLock();

                try
                {
                    return this.items.Count;
                }
                finally
                {
                    this.itemsLocker.ExitReadLock();
                }
            }
        }

        /// <inheritdoc/>
        bool IList.IsFixedSize
            => this.IsFixedSize;

        /// <inheritdoc/>
        bool ICollection<T>.IsReadOnly
            => this.IsReadOnly;

        /// <inheritdoc/>
        bool IList.IsReadOnly
            => this.IsReadOnly;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized
            => this.IsSynchronized;

        /// <inheritdoc/>
        object ICollection.SyncRoot
            => this.SyncRoot;

        /// <summary>Gets a value indicating whether the <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <returns>true if the <see cref="SynchronizedObservableList{T}" /> has a fixed size; otherwise, false.</returns>
        protected bool IsFixedSize
            => false;

        /// <summary>Gets a value indicating whether the <see cref="SynchronizedObservableList{T}" /> is read-only.</summary>
        /// <returns>true if the <see cref="SynchronizedObservableList{T}" /> is read-only; otherwise, false.</returns>
        protected bool IsReadOnly
            => false;

        /// <summary>Gets a value indicating whether access to the <see cref="SynchronizedObservableList{T}" /> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="SynchronizedObservableList{T}" /> is synchronized (thread safe); otherwise, false.</returns>
        protected bool IsSynchronized
            => true;

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="SynchronizedObservableList{T}" />.</returns>
        protected object SyncRoot
            => (this.items as ICollection).SyncRoot;

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <returns>The element at the specified index.</returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than zero.-or-<paramref name="index" /> is equal to or greater than <see cref="SynchronizedObservableList{T}.Count" />. </exception>
        public T this[int index]
        {
            [DebuggerStepThrough]
            get
            {
                this.itemsLocker.EnterReadLock();

                try
                {
                    if (index < 0 || index >= this.items.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }

                    return this.items[index];
                }
                finally
                {
                    this.itemsLocker.ExitReadLock();
                }
            }

            [DebuggerStepThrough]
            set
            {
                T oldValue;

                this.itemsLocker.EnterWriteLock();

                try
                {
                    if (index < 0 || index >= this.items.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }

                    if (this.monitor > 0)
                    {
                        throw new InvalidOperationException(ReentrancyNotAllowedError);
                    }

                    oldValue = this.items[index];

                    this.items[index] = value;
                }
                finally
                {
                    this.itemsLocker.ExitWriteLock();
                }

                Interlocked.Increment(ref this.monitor);
                try
                {
                    this.SynchronizationContext.Send(
                        state =>
                        {
                            this.OnPropertyChanged("Items[]");
                            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue, index));
                        }, null);
                }
                finally
                {
                    Interlocked.Decrement(ref this.monitor);
                }
            }
        }

        /// <inheritdoc/>
        object IList.this[int index]
        {
            [DebuggerStepThrough]
            get => this[index];
            [DebuggerStepThrough]

            set
            {
                try
                {
                    if (value is null && !(default(T) is null))
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException(ValueWrongTypeError);
                }
            }
        }

        /// <summary>Adds an object to the end of the <see cref="SynchronizedObservableList{T}" />. </summary>
        /// <param name="item">The object to be added to the end of the <see cref="SynchronizedObservableList{T}" />. The value can be null for reference types.</param>
        public void Add(T item)
        {
            this.itemsLocker.EnterWriteLock();

            int index;

            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                index = this.items.Count;
                this.items.Insert(index, item);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <summary>
        /// Adds the elements of the given collection to the end of this list. If required, the capacity of the list is increased to twice the previous capacity or the new size, whichever is larger.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            this.itemsLocker.EnterWriteLock();
            int index;
            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                index = this.items.Count;
                this.items.InsertRange(index, collection);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList(), index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="SynchronizedObservableList{T}" />.
        /// </summary>
        public void Clear()
        {
            this.itemsLocker.EnterWriteLock();

            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                this.items.Clear();
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <summary>Copies the <see cref="SynchronizedObservableList{T}" /> elements to an existing one-dimensional <see cref="System.Array" />, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements copied from <see cref="SynchronizedObservableList{T}" />. The <see cref="System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="array" /> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex" /> is less than zero.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source <see cref="SynchronizedObservableList{T}" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            array.ThrowIfNull(nameof(array));
            this.ThrowIfCheckFails(nameof(arrayIndex), arrayIndex >= 0 && arrayIndex < array.Length);
            this.ThrowIfCheckFails(nameof(arrayIndex), array.Length - arrayIndex >= this.Count, "Invalid offset length.");

            this.itemsLocker.EnterReadLock();

            try
            {
                this.items.CopyTo(array, arrayIndex);
            }
            finally
            {
                this.itemsLocker.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        int IList.Add(object value)
        {
            this.itemsLocker.EnterWriteLock();

            int index;
            T item;

            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                index = this.items.Count;
                item = (T)value;

                this.items.Insert(index, item);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(ValueWrongTypeError);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }

            return index;
        }

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            array.ThrowIfCheckFails(nameof(array), array.Rank == 1, "Multidimensional array are not supported");
            array.ThrowIfCheckFails(nameof(array), array.GetLowerBound(0) == 0, "Non-zero lower bound is not supported");
            array.ThrowIfCheckFails(nameof(arrayIndex), arrayIndex >= 0 && arrayIndex < array.Length);
            array.ThrowIfCheckFails(nameof(arrayIndex), array.Length - arrayIndex >= this.Count, "Invalid offset length.");

            this.itemsLocker.EnterReadLock();

            try
            {
                if (array is T[] tArray)
                {
                    this.items.CopyTo(tArray, arrayIndex);
                }
                else
                {
                    // Catch the obvious case assignment will fail.
                    // We can found all possible problems by doing the check though.
                    // For example, if the element type of the Array is derived from T,
                    // we can't figure out if we can successfully copy the element beforehand.
                    Type targetType = array.GetType().GetElementType();
                    Type sourceType = typeof(T);
                    if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }

                    // We can't cast array of value type to object[], so we don't support widening of primitive types here.
                    if (array is not object[] objects)
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }

                    int count = this.items.Count;
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            objects[arrayIndex++] = this.items[i];
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }
                }
            }
            finally
            {
                this.itemsLocker.ExitReadLock();
            }
        }

        /// <summary>Determines whether an element is in the <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="SynchronizedObservableList{T}" />; otherwise, false.</returns>
        /// <param name="item">The object to locate in the <see cref="SynchronizedObservableList{T}" />. The value can be null for reference types.</param>
        public bool Contains(T item)
        {
            this.itemsLocker.EnterReadLock();

            try
            {
                return this.items.Contains(item);
            }
            finally
            {
                this.itemsLocker.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        bool IList.Contains(object value)
        {
            if (!typeof(T).IsAssignableFrom(value?.GetType()))
            {
                return false;
            }

            this.itemsLocker.EnterReadLock();

            try
            {
                return this.items.Contains((T)value);
            }
            finally
            {
                this.itemsLocker.ExitReadLock();
            }
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <returns>An <see cref="IEnumerator{T}" /> for the <see cref="SynchronizedObservableList{T}" />.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            this.itemsLocker.EnterReadLock();

            try
            {
                return this.items.ToList().GetEnumerator();
            }
            finally
            {
                this.itemsLocker.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            // ReSharper disable once RedundantCast
            return (IEnumerator)this.GetEnumerator();
        }

        /// <summary>Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <returns>The zero-based index of the first occurrence of <paramref name="item" /> within the entire <see cref="SynchronizedObservableList{T}" />, if found; otherwise, -1.</returns>
        /// <param name="item">The object to locate in the <see cref="SynchronizedObservableList{T}" />. The value can be null for reference types.</param>
        public int IndexOf(T item)
        {
            this.itemsLocker.EnterReadLock();

            try
            {
                return this.items.IndexOf(item);
            }
            finally
            {
                this.itemsLocker.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        int IList.IndexOf(object value)
        {
            if (!typeof(T).IsAssignableFrom(value?.GetType()))
            {
                return -1;
            }

            this.itemsLocker.EnterReadLock();

            try
            {
                return this.items.IndexOf((T)value);
            }
            finally
            {
                this.itemsLocker.ExitReadLock();
            }
        }

        /// <summary>Inserts an element into the <see cref="SynchronizedObservableList{T}" /> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than zero.-or-<paramref name="index" /> is greater than <see cref="SynchronizedObservableList{T}.Count" />.</exception>
        public void Insert(int index, T item)
        {
            this.itemsLocker.EnterWriteLock();

            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                if (index < 0 || index > this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                this.items.Insert(index, item);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <summary>
        /// Inserts the elements of the given collection at a given index. If required, the capacity of the list is increased to twice the previous capacity
        /// or the new size, whichever is larger. Ranges may be added to the end of the list by setting index to the List's size.
        /// </summary>
        /// <param name="index">The index at which to add new items.</param>
        /// <param name="collection">Items to add to the list.</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            this.itemsLocker.EnterWriteLock();

            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                if (index < 0 || index > this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                this.items.InsertRange(index, collection);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList(), index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <inheritdoc/>
        void IList.Insert(int index, object value)
        {
            try
            {
                this.Insert(index, (T)value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("'value' is the wrong type");
            }
        }

        /// <summary>Moves the item at the specified index to a new location in the collection.</summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        public void Move(int oldIndex, int newIndex)
        {
            T item;

            this.itemsLocker.EnterWriteLock();

            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                if (oldIndex < 0 || oldIndex >= this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(oldIndex));
                }

                if (newIndex < 0 || newIndex >= this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(newIndex));
                }

                item = this.items[oldIndex];

                this.items.RemoveAt(oldIndex);
                this.items.Insert(newIndex, item);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged("Items[]");
                        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <returns>true if <paramref name="item" /> is successfully removed; otherwise, false.  This method also returns false if <paramref name="item" /> was not found in the original <see cref="SynchronizedObservableList{T}" />.</returns>
        /// <param name="item">The object to remove from the <see cref="SynchronizedObservableList{T}" />. The value can be null for reference types.</param>
        public bool Remove(T item)
        {
            int index;
            T value;

            this.itemsLocker.EnterWriteLock();

            try
            {
                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                index = this.items.IndexOf(item);

                if (index < 0)
                {
                    return false;
                }

                value = this.items[index];

                this.items.RemoveAt(index);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }

            return true;
        }

        /// <inheritdoc/>
        void IList.Remove(object value)
        {
            if (typeof(T).IsAssignableFrom(value?.GetType()))
            {
                this.Remove((T)value);
            }
        }

        /// <summary>Removes the element at the specified index of the <see cref="SynchronizedObservableList{T}" />.</summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than zero.-or-<paramref name="index" /> is equal to or greater than <see cref="SynchronizedObservableList{T}.Count" />.</exception>
        public void RemoveAt(int index)
        {
            T value;

            this.itemsLocker.EnterWriteLock();

            try
            {
                if (index < 0 || index > this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                value = this.items[index];

                this.items.RemoveAt(index);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <summary>
        /// Removes a range of elements from this list.
        /// </summary>
        /// <param name="index">The starting index to remove from.</param>
        /// <param name="count">The number of items to remove.</param>
        public void RemoveRange(int index, int count)
        {
            IList value;

            this.itemsLocker.EnterWriteLock();

            try
            {
                if (index < 0 || count < 0 || index + count > this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (this.monitor > 0)
                {
                    throw new InvalidOperationException(ReentrancyNotAllowedError);
                }

                value = this.items.Skip(index).Take(count).ToArray();

                this.items.RemoveRange(index, count);
            }
            finally
            {
                this.itemsLocker.ExitWriteLock();
            }

            Interlocked.Increment(ref this.monitor);
            try
            {
                this.SynchronizationContext.Send(
                    state =>
                    {
                        this.OnPropertyChanged(nameof(ICollection.Count));
                        this.OnPropertyChanged("Item[]");
                        this.CollectionChanged.SafeRaise(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
                    }, null);
            }
            finally
            {
                Interlocked.Decrement(ref this.monitor);
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="SynchronizedObservableList{T}"/>.
        /// </summary>
        /// <param name="disposing">Not used.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.itemsLocker.Dispose();
            }
        }
    }
}
