// <copyright file="ObservableDirectory.cs" company="Flynn1179">
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
    public class ObservableDirectory : DisposableObservableObject, IObservableDirectoryContent
    {
        private readonly FileSystemWatcher watcher;

        private readonly string directory;

        private string path;

        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDirectory"/> class.
        /// </summary>
        /// <param name="directoryName">The directory name to monitor.</param>
        /// <exception cref="ArgumentException">Thrown if the directory does not exist.</exception>
        public ObservableDirectory(string directoryName)
        {
            this.FullPath = directoryName;
        }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        public string Name
        {
            get => this.name;
            private set => this.Set(ref this.name, value);
        }

        public string Directory
            => this.directory;

        public string FullPath
        {
            get => this.path;
            init
            {
                this.path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(value));
                if (!System.IO.Directory.Exists(this.path))
                {
                    throw new ArgumentException("Cannot create an instance of ObservableDirectory for a filename that does not exist.");
                }

                this.directory = Path.GetDirectoryName(this.path);
                this.name = Path.GetFileName(this.path);
                this.watcher = new FileSystemWatcher
                {
                    Path = this.directory,
                    Filter = this.name,
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security,
                    EnableRaisingEvents = true,
                };

                this.watcher.Renamed += this.HandleWatcherRenamed;
                this.watcher.Deleted += this.HandleWatcherDeleted;
                this.watcher.Changed += this.HandleWatcherChanged;
            }
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
            Debug.WriteLine("Detected directory rename, from " + e.OldFullPath + " to " + e.FullPath);
            this.Set(ref this.path, e.FullPath);
            this.Name = Path.GetFileName(this.path);
            this.watcher.Filter = e.Name;
        }

        private void HandleWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("Detected { deletion, disposing ObservableDirectory.");
            this.Dispose();
        }

        private void HandleWatcherChanged(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("Detected change " + e.ChangeType + " to " + e.FullPath);
        }
    }
}
