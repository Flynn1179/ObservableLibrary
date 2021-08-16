// <copyright file="ObservableFile.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Represents a file, notifying of changes to the properties of the file.
    /// </summary>
    public class ObservableFile : DisposableObservableObject, IObservableFile
    {
        private readonly FileSystemWatcher watcher;

        private string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableFile"/> class.
        /// </summary>
        /// <param name="fileName">The filename to monitor.</param>
        /// <exception cref="ArgumentException">Thrown if the file does not exist.</exception>
        public ObservableFile(string fileName)
        {
            string actualFileName = Path.GetFullPath(Environment.ExpandEnvironmentVariables(fileName));
            if (!File.Exists(actualFileName))
            {
                throw new ArgumentException("Cannot create an instance of ObservableFile for a filename that does not exist.");
            }

            this.fileName = actualFileName;
            this.watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(this.fileName),
                Filter = Path.GetFileName(this.fileName),
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security,
                EnableRaisingEvents = true,
            };

            this.watcher.Renamed += this.HandleWatcherRenamed;
            this.watcher.Deleted += this.HandleWatcherDeleted;
            this.watcher.Changed += this.HandleWatcherChanged;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string FileName
        {
            get => this.fileName;
            private set => this.Set(ref this.fileName, value);
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        /// <param name="disposing">True if the object is disposing, false otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this.watcher is not null)
                {
                    this.watcher.Renamed -= this.HandleWatcherRenamed;
                    this.watcher.Deleted -= this.HandleWatcherDeleted;
                    this.watcher.Changed -= this.HandleWatcherChanged;
                    this.watcher.Dispose();
                }
            }
        }

        private void HandleWatcherRenamed(object sender, RenamedEventArgs e)
        {
            Debug.WriteLine("Detected file rename, from " + e.OldFullPath + " to " + e.FullPath);
            this.FileName = e.FullPath;
            this.watcher.Filter = e.Name;
        }

        private void HandleWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("Detected file deletion, disposing ObservableFile.");
            this.Dispose();
        }

        private void HandleWatcherChanged(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("Detected change " + e.ChangeType + " to " + e.FullPath);
        }
    }
}
