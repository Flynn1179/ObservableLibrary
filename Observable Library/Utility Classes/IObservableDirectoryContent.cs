// <copyright file="IObservableDirectoryContent.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System.ComponentModel;

    /// <summary>
    /// Defines properties for an object representing a file or directory that notifies of changes.
    /// </summary>
    public interface IObservableDirectoryContent : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the name of the file or directory.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the name of the directory containing the file or directory.
        /// </summary>
        string Directory { get; }

        /// <summary>
        /// Gets the full path of the file or directory.
        /// </summary>
        string FullPath { get; init; }
    }
}