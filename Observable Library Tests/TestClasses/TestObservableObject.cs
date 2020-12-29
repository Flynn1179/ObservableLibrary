// <copyright file="TestObservableObject.cs" company="Flynn1179">
//   Copyright © 2020 Flynn1179
// </copyright>

using System;
using System.ComponentModel;

namespace Flynn1179.Observable.Tests
{
    public class TestObservableObject : TestObservableObject<object> { }

    public class TestObservableObject<T> : ObservableObject, ITestNotifyPropertyChanged<T>
    {
        private readonly T[] indexed = new T[8];

        private string stringField;

        private T field;

        public string StringProperty
        {
            get => this.stringField;
            set => this.Set(ref this.stringField, value);
        }

        public T Property
        {
            get => this.field;
            set => this.Set(ref this.field, value);
        }

        public T this[int index]
        {
            get => this.indexed[index];
            set => this.Set(ref this.indexed[index], value, "Item[]");
        }
    }
}
