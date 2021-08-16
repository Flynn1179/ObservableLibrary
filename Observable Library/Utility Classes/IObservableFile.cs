// <copyright file="IObservableFile.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    /// <summary>
    /// Defines properties for an object representing a file that notifies of changes.
    /// </summary>
    public interface IObservableFile
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        string FileName { get; }
    }
}