// <copyright file="PropertyListener.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Represents a class that listens for a property changing on a target object, and invokes the action when it does.
    /// </summary>
    /// <typeparam name="TPropertyType">The type of the property.</typeparam>
    public class PropertyListener<TPropertyType>
    {
        private readonly INotifyPropertyChanged target;

        private readonly string propertyName;

        private readonly Action<TPropertyType> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyListener{TPropertyType}"/> class.
        /// </summary>
        /// <param name="target">The target object to listen to.</param>
        /// <param name="propertyName">The property to listen for on the target object.</param>
        /// <param name="action">The action to be performed when the property changes.</param>
        public PropertyListener(INotifyPropertyChanged target, string propertyName, Action<TPropertyType> action)
        {
            target.ThrowIfNull(nameof(target));
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));
            action.ThrowIfNull(nameof(action));

            if (target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance) is not PropertyInfo propertyInfo)
            {
                throw new ArgumentException("Target object does not have a property with the given name.");
            }

            if (!propertyInfo.CanRead)
            {
                throw new ArgumentException("The target property is not readable");
            }

            if (!Type.Equals(propertyInfo.PropertyType, typeof(TPropertyType)))
            {
                throw new ArgumentException("The target property it not the correct type for the action.");
            }

            this.target = target;
            this.propertyName = propertyName;
            this.action = action;
            this.target.PropertyChanged += this.HandleTargetPropertyChanged;
        }

        private void HandleTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, this.propertyName, StringComparison.Ordinal))
            {
                this.action((TPropertyType)this.target.GetPropertyValue(this.propertyName));
            }
        }
    }
}
