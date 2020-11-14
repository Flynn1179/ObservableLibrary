// <copyright file="ObservableObject.cs" company="Flynn1179">
//   Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines a base class for an object that implements the <see cref="INotifyPropertyChanging"/> and <see cref="INotifyPropertyChanged"/> interfaces.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Occurs before a property of this object changes.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Occurs after a property of this object changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Default implementation of the OnPropertyChanging method that just raises the event.
        /// </summary>
        /// <param name="propertyName">The name of the property to raise an event for.</param>
        /// <exception cref="ArgumentException">Throw if the property does not exist on the target, if a debugger is attached.</exception>
        /// <exception cref="ArgumentNullException">Thrown if propertyName is null or an empty string.</exception>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        protected virtual void OnPropertyChanging(string propertyName)
            => this.PropertyChanging.SafeRaise(this, propertyName);

        /// <summary>
        /// Default implementation of the OnPropertyChanged method that just raises the event.
        /// </summary>
        /// <param name="propertyName">The name of the property to raise an event for.</param>
        /// <exception cref="ArgumentException">Throw if the property does not exist on the target, if a debugger is attached.</exception>
        /// <exception cref="ArgumentNullException">Thrown if propertyName is null or an empty string.</exception>
        /// <exception cref="AggregateException">Thrown if any handlers raise exceptions, with the exceptions raised captured in the <see cref="AggregateException.InnerExceptions"/> property.</exception>
        protected virtual void OnPropertyChanged(string propertyName)
            => this.PropertyChanged.SafeRaise(this, propertyName);

        /// <summary>
        /// Sets the value of the referenced field if it wasn't already the same, and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="field">A reference to the field to change.</param>
        /// <param name="value">The new value of the field.</param>
        /// <param name="propertyName">The name of the property that exposes this field.</param>
        /// <returns>True if the field was changed and the event was raised, false if it already had the value given.</returns>
        protected bool Set(ref string field, string value, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, null, null, propertyName);

        /// <summary>
        /// Sets the value of the referenced field if it wasn't already the same, and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="field">A reference to the field to change.</param>
        /// <param name="value">The new value of the field.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="propertyName">The name of the property that exposes this field.</param>
        /// <returns>True if the field was changed and the event was raised, false if it already had the value given.</returns>
        protected bool Set(ref string field, string value, Action onChange, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, onChange, null, null, propertyName);

        /// <summary>
        /// Sets the value of the referenced field if it wasn't already the same, and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="field">A reference to the field to change.</param>
        /// <param name="value">The new value of the field.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="propertyName">The name of the property that exposes this field.</param>
        /// <returns>True if the field was changed and the event was raised, false if it already had the value given.</returns>
        protected bool Set(ref string field, string value, Action<string> onChangeWithPrevious, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, onChangeWithPrevious, null, propertyName);

        /// <summary>
        /// Sets the value of the referenced field if it wasn't already the same, and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="field">A reference to the field to change.</param>
        /// <param name="value">The new value of the field.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that exposes this field.</param>
        /// <returns>True if the field was changed and the event was raised, false if it already had the value given.</returns>
        protected bool Set(ref string field, string value, Func<string, string> validate, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, null, validate, propertyName);

        /// <summary>
        /// Sets the value of the referenced field if it wasn't already the same, and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="field">A reference to the field to change.</param>
        /// <param name="value">The new value of the field.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that exposes this field.</param>
        /// <returns>True if the field was changed and the event was raised, false if it already had the value given.</returns>
        protected bool Set(ref string field, string value, Action onChange, Func<string, string> validate, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, onChange, null, validate, propertyName);

        /// <summary>
        /// Sets the value of the referenced field if it wasn't already the same, and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="field">A reference to the field to change.</param>
        /// <param name="value">The new value of the field.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that exposes this field.</param>
        /// <returns>True if the field was changed and the event was raised, false if it already had the value given.</returns>
        protected bool Set(ref string field, string value, Action<string> onChangeWithPrevious, Func<string, string> validate, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, onChangeWithPrevious, validate, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, null, null, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, onChange, null, null, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, onChangeWithPrevious, null, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, null, validate, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, onChange, null, validate, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
            => this.Set(ref field, value, null, onChangeWithPrevious, validate, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : INotifyPropertyChanged
            => this.Set(ref field, value, null, null, null, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : INotifyPropertyChanged
            => this.Set(ref field, value, onChange, null, null, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : INotifyPropertyChanged
            => this.Set(ref field, value, null, onChangeWithPrevious, null, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : INotifyPropertyChanged
            => this.Set(ref field, value, null, null, validate, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : INotifyPropertyChanged
            => this.Set(ref field, value, onChange, null, validate, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : INotifyPropertyChanged
            => this.Set(ref field, value, null, onChangeWithPrevious, validate, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, (Action<TProp>)null, null, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, onChange, (Action<TProp>)null, null, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, onChangeWithPrevious, null, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, null, validate, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, onChange, null, validate, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, onChangeWithPrevious, validate, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, null, null, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, onChange, null, null, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, onChangeWithPrevious, null, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, null, validate, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler = null, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, onChange, null, validate, changeHandler, propertyName);

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler = null, [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
            => this.Set(ref field, value, disposePrevious, null, onChangeWithPrevious, validate, changeHandler, propertyName);

        /// <summary>
        /// Changes the value of a property after checking it's within a given range, and if it changed, raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the backing field for the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="min">The minimum permitted value for the property.</param>
        /// <param name="max">The maximum permitted value for the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the property was changed, false if it already held the desired value.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, TProp min, TProp max, [CallerMemberName] string propertyName = "")
            where TProp : IComparable
            => this.Set(ref field, value, min, max, null, null, propertyName);

        /// <summary>
        /// Changes the value of a property after checking it's within a given range, and if it changed, raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the backing field for the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="min">The minimum permitted value for the property.</param>
        /// <param name="max">The maximum permitted value for the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the property was changed, false if it already held the desired value.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, TProp min, TProp max, Action onChange = null, [CallerMemberName] string propertyName = "")
            where TProp : IComparable
            => this.Set(ref field, value, min, max, onChange, null, propertyName);

        /// <summary>
        /// Changes the value of a property after checking it's within a given range, and if it changed, raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the backing field for the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="min">The minimum permitted value for the property.</param>
        /// <param name="max">The maximum permitted value for the property.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the property was changed, false if it already held the desired value.</returns>
        protected bool Set<TProp>(ref TProp field, TProp value, TProp min, TProp max, Action<TProp> onChangeWithPrevious = null, [CallerMemberName] string propertyName = "")
            where TProp : IComparable
            => this.Set(ref field, value, min, max, null, onChangeWithPrevious, propertyName);

        /// <summary>
        /// Sets the value of the referenced field if it wasn't already the same, and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="field">A reference to the field to change.</param>
        /// <param name="value">The new value of the field.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that exposes this field.</param>
        /// <returns>True if the field was changed and the event was raised, false if it already had the value given.</returns>
        private bool Set(
            ref string field,
            string value,
            Action onChange = null,
            Action<string> onChangeWithPrevious = null,
            Func<string, string> validate = null,
            [CallerMemberName] string propertyName = "")
        {
            propertyName.ThrowIfNull(nameof(propertyName));
            this.GetType().ValidatePropertyName(propertyName, nameof(propertyName));
            if (string.Equals(field, value, StringComparison.Ordinal))
            {
                return false;
            }

            if (validate is not null && validate(value) is string message)
            {
                throw new ArgumentException(message);
            }

            string previous = field;
            this.OnPropertyChanging(propertyName);
            field = value;
            this.OnPropertyChanged(propertyName);
            if (onChange is not null)
            {
                onChange();
            }

            if (onChangeWithPrevious is not null)
            {
                onChangeWithPrevious(previous);
            }

            return true;
        }

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        private bool Set<TProp>(
            ref TProp field,
            TProp value,
            Action onChange = null,
            Action<TProp> onChangeWithPrevious = null,
            Func<TProp, string> validate = null,
            [CallerMemberName] string propertyName = "")
        {
            propertyName.ThrowIfNull(nameof(propertyName));
            this.GetType().ValidatePropertyName(propertyName, nameof(propertyName));
            if (typeof(TProp).IsClass ? ReferenceEquals(field, value) : EqualityComparer<TProp>.Default.Equals(field, value))
            {
                return false;
            }

            if (validate is not null && validate(value) is string message)
            {
                throw new ArgumentException(message);
            }

            TProp previous = field;
            this.OnPropertyChanging(propertyName);
            field = value;
            this.OnPropertyChanged(propertyName);
            if (onChange is not null)
            {
                onChange();
            }

            if (onChangeWithPrevious is not null)
            {
                onChangeWithPrevious(previous);
            }

            return true;
        }

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        private bool Set<TProp>(
            ref TProp field,
            TProp value,
            Action onChange = null,
            Action<TProp> onChangeWithPrevious = null,
            Func<TProp, string> validate = null,
            PropertyChangedEventHandler changeHandler = null,
            [CallerMemberName] string propertyName = "")
            where TProp : INotifyPropertyChanged
        {
            propertyName.ThrowIfNull(nameof(propertyName));
            this.GetType().ValidatePropertyName(propertyName, nameof(propertyName));
            if (typeof(TProp).IsClass ? ReferenceEquals(field, value) : EqualityComparer<TProp>.Default.Equals(field, value))
            {
                return false;
            }

            if (validate is not null && validate(value) is string message)
            {
                throw new ArgumentException(message);
            }

            TProp previous = field;
            this.OnPropertyChanging(propertyName);
            if (changeHandler is not null && field is INotifyPropertyChanged oldObservable)
            {
                oldObservable.PropertyChanged -= changeHandler;
            }

            field = value;
            if (changeHandler is not null && field is INotifyPropertyChanged newObservable)
            {
                newObservable.PropertyChanged += changeHandler;
            }

            this.OnPropertyChanged(propertyName);
            if (onChange is not null)
            {
                onChange();
            }

            if (onChangeWithPrevious is not null)
            {
                onChangeWithPrevious(previous);
            }

            return true;
        }

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        private bool Set<TProp>(
            ref TProp field,
            TProp value,
            bool disposePrevious,
            Action onChange = null,
            Action<TProp> onChangeWithPrevious = null,
            Func<TProp, string> validate = null,
            [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
        {
            propertyName.ThrowIfNull(nameof(propertyName));
            this.GetType().ValidatePropertyName(propertyName, nameof(propertyName));
            if (typeof(TProp).IsClass ? ReferenceEquals(field, value) : EqualityComparer<TProp>.Default.Equals(field, value))
            {
                return false;
            }

            if (validate is not null && validate(value) is string message)
            {
                throw new ArgumentException(message);
            }

            if (disposePrevious && field != null)
            {
                field.Dispose();
            }

            TProp previous = field;
            this.OnPropertyChanging(propertyName);
            field = value;
            this.OnPropertyChanged(propertyName);
            if (onChange is not null)
            {
                onChange();
            }

            if (onChangeWithPrevious is not null)
            {
                onChangeWithPrevious(previous);
            }

            return true;
        }

        /// <summary>
        /// Called by inheriting classes to set the value of a property and raise a changed event if it has actually changed. This will take no action if the
        /// value of the property is already the value it's being set to, and raise no events. If the value was changed and was previously not null, it is
        /// disposed.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the underlying private field to change.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="disposePrevious">If true and the previous value was disposable, it will be disposed.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="validate">A function to validate the new value of the property.</param>
        /// <param name="changeHandler">An event handler to handle any property changed events on the target.</param>
        /// <param name="propertyName">The name of the property that's changing.</param>
        /// <returns>True if the field was changed, false otherwise.</returns>
        private bool Set<TProp>(
            ref TProp field,
            TProp value,
            bool disposePrevious,
            Action onChange = null,
            Action<TProp> onChangeWithPrevious = null,
            Func<TProp, string> validate = null,
            PropertyChangedEventHandler changeHandler = null,
            [CallerMemberName] string propertyName = "")
            where TProp : IDisposable
        {
            propertyName.ThrowIfNull(nameof(propertyName));
            this.GetType().ValidatePropertyName(propertyName, nameof(propertyName));
            if (typeof(TProp).IsClass ? ReferenceEquals(field, value) : EqualityComparer<TProp>.Default.Equals(field, value))
            {
                return false;
            }

            if (validate is not null && validate(value) is string message)
            {
                throw new ArgumentException(message);
            }

            if (disposePrevious && field != null)
            {
                field.Dispose();
            }

            TProp previous = field;
            this.OnPropertyChanging(propertyName);
            if (changeHandler is not null && field is INotifyPropertyChanged oldObservable)
            {
                oldObservable.PropertyChanged -= changeHandler;
            }

            field = value;
            if (changeHandler is not null && field is INotifyPropertyChanged newObservable)
            {
                newObservable.PropertyChanged += changeHandler;
            }

            this.OnPropertyChanged(propertyName);
            if (onChange is not null)
            {
                onChange();
            }

            if (onChangeWithPrevious is not null)
            {
                onChangeWithPrevious(previous);
            }

            return true;
        }

        /// <summary>
        /// Changes the value of a property after checking it's within a given range, and if it changed, raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being changed.</typeparam>
        /// <param name="field">A reference to the backing field for the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="min">The minimum permitted value for the property.</param>
        /// <param name="max">The maximum permitted value for the property.</param>
        /// <param name="onChange">An action to be invoked after the property has changed value.</param>
        /// <param name="onChangeWithPrevious">An action to be invoked after the property has changed value, with the previous value passed as a parameter to the action.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the property was changed, false if it already held the desired value.</returns>
        private bool Set<TProp>(
            ref TProp field,
            TProp value,
            TProp min,
            TProp max,
            Action onChange = null,
            Action<TProp> onChangeWithPrevious = null,
            [CallerMemberName] string propertyName = "")
            where TProp : IComparable
        {
            propertyName.ThrowIfNull(nameof(propertyName));
            this.GetType().ValidatePropertyName(propertyName, nameof(propertyName));
            EqualityComparer<TProp> comparer = EqualityComparer<TProp>.Default;
            if (field is string ? Equals(field, value) : typeof(TProp).IsClass ? ReferenceEquals(field, value) : comparer.Equals(field, value))
            {
                return false;
            }

            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            TProp previous = field;
            this.OnPropertyChanging(propertyName);
            field = value;
            this.OnPropertyChanged(propertyName);
            if (onChange is not null)
            {
                onChange();
            }

            if (onChangeWithPrevious is not null)
            {
                onChangeWithPrevious(previous);
            }

            return true;
        }
    }
}