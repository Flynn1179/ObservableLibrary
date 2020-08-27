// <copyright file="ITestNotifyPropertyChanged.cs" company="Flynn1179">
//   Copyright © 2020 Flynn1179
// </copyright>

using System.ComponentModel;

namespace Flynn1179.Observable.Tests.TestClasses
{
    public interface ITestNotifyPropertyChanged<T> : INotifyPropertyChanged
    {
        T Property { get; set; }
    }
}
