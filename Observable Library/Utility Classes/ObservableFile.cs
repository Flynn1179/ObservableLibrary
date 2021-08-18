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
    public class ObservableFile : DisposableObservableObject, IObservableDirectoryContent
    {
        private readonly FileSystemWatcher watcher;

        private readonly string directory;

        private string path;

        private string fileName;

        private string extension;

        private string fileNameWithoutExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableFile"/> class.
        /// </summary>
        /// <param name="fileName">The filename to monitor.</param>
        /// <exception cref="ArgumentException">Thrown if the file does not exist.</exception>
        public ObservableFile(string fileName)
        {
            this.FullPath = fileName;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name
        {
            get => this.fileName;
            private set => this.Set(ref this.fileName, value);
        }

        public string Extension
        {
            get => this.extension;
            private set => this.Set(ref this.extension, value);
        }

        public string FileNameWithoutExtension
        {
            get => this.fileNameWithoutExtension;
            private set => this.Set(ref this.fileNameWithoutExtension, value);
        }

        public string Directory
            => this.directory;

        public string FullPath
        {
            get => this.path;
            init
            {
                this.path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(value));
                if (!File.Exists(this.path))
                {
                    throw new ArgumentException("Cannot create an instance of ObservableFile for a filename that does not exist.");
                }

                this.fileName = Path.GetFileName(this.path);
                this.extension = Path.GetExtension(this.path);
                this.fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.path);
                this.directory = Path.GetDirectoryName(this.path);
                this.watcher = new FileSystemWatcher
                {
                    Path = this.directory,
                    Filter = this.fileName,
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
            Debug.WriteLine("Detected file rename, from " + e.OldFullPath + " to " + e.FullPath);

            // Cannot set the property directly, it has an 'init' block.
            this.Set(ref this.path, e.FullPath, nameof(IObservableDirectoryContent.FullPath));
            this.Name = Path.GetFileName(this.path);
            this.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.path);
            this.Extension = Path.GetExtension(this.path);
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
