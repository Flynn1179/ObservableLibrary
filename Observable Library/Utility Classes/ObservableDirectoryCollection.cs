// <copyright file="ObservableDirectoryCollection.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

// TODO: Observe contents and parent.

namespace Flynn1179.Observable
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Represents an observable directory, that notifies of files being added or removed.
    /// </summary>
    public class ObservableDirectoryCollection : SynchronizedObservableList<IObservableDirectoryContent>, IObservableDirectoryContent
    {
        private readonly FileSystemWatcher contentsWatcher;

        private readonly FileSystemWatcher watcher;

        private readonly string directory;

        private string path;

        private string name;

        private bool recursive;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDirectoryCollection"/> class.
        /// </summary>
        /// <param name="path">The path of the directory to observe.</param>
        /// <exception cref="ArgumentException">Thrown if the path does not exist.</exception>
        public ObservableDirectoryCollection(string path)
        {
            this.FullPath = path;
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
                    throw new ArgumentException("Cannot create an ObservableDirectory for a directory that does not exist.");
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

                string[] fileNames = System.IO.Directory.GetFiles(this.path);
                IObservableDirectoryContent[] files = new IObservableDirectoryContent[fileNames.Length];
                for (int i = 0; i < fileNames.Length; i++)
                {
                    if (File.Exists(fileNames[i]))
                    {
                        files[i] = new ObservableFile(fileNames[i]);
                    }
                    else if (System.IO.Directory.Exists(fileNames[i]))
                    {
                        files[i] = this.Recursive ? new ObservableDirectoryCollection(fileNames[i]) : new ObservableDirectory(fileNames[i]);
                    }
                }

                this.AddRange(files);

                this.contentsWatcher = new FileSystemWatcher
                {
                    Path = this.path,
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security,
                    EnableRaisingEvents = true,
                };

                //// this.watcher.Changed += this.Watcher_Changed;
                this.contentsWatcher.Created += this.Watcher_Created;
                this.contentsWatcher.Deleted += this.Watcher_Deleted;
                //// this.watcher.Renamed += this.Watcher_Renamed;
                this.contentsWatcher.Error += this.Watcher_Error;
                this.contentsWatcher.Disposed += this.Watcher_Disposed;
            }
        }

        /// <summary>
        /// Gets the name of this directory.
        /// </summary>
        public string Name
        {
            get => this.name;
            private set => this.Set(ref this.name, value);
        }

        /// <summary>
        /// Gets a value indicating whether or not this instances observes a directory recursively. If set to true, an instance of <see cref="ObservableDirectoryCollection"/> will be instantiated for each folder within this folder's contents, otherwise an instance of <see cref="ObservableDirectory"/> will be instantiated.
        /// </summary>
        public bool Recursive
        {
            get => this.recursive;
            init => this.Set(ref this.recursive, value);
        }

        /// <summary>
        /// Disposes of the observable directory.
        /// </summary>
        /// <param name="disposing">True if the Dispose method was called, false otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this.contentsWatcher is not null)
                {
                    //// this.watcher.Changed -= this.Watcher_Changed;
                    this.contentsWatcher.Created -= this.Watcher_Created;
                    this.contentsWatcher.Deleted -= this.Watcher_Deleted;
                    //// this.watcher.Renamed += this.Watcher_Renamed;
                    this.contentsWatcher.Error -= this.Watcher_Error;
                    this.contentsWatcher.Disposed -= this.Watcher_Disposed;
                    this.contentsWatcher.Dispose();
                }

                if (this.watcher is not null)
                {
                    this.watcher.Renamed -= this.HandleWatcherRenamed;
                    this.watcher.Deleted -= this.HandleWatcherDeleted;
                    this.watcher.Changed -= this.HandleWatcherChanged;
                    this.watcher.Dispose();
                }
            }
        }

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            Debug.WriteLine("ObservableDirectory received error " + e + ", " + e.GetException()?.Message);
            if (!System.IO.Directory.Exists(this.path))
            {
                this.Dispose();
            }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Debug.WriteLine("ObservableDirectory received rename event: " + e.OldFullPath + " to " + e.FullPath);

            // This will be handled by the ObservableFile for now.
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("ObservableDirectory received delete event: " + e + ", " + e.FullPath);
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (string.Equals(this[i].Name, e.FullPath, StringComparison.Ordinal))
                {
                    this.RemoveAt(i);
                    return;
                }
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("ObservableDirectory received create event: " + e + ", " + e.FullPath);
            if (File.Exists(e.FullPath))
            {
                this.Add(new ObservableFile(e.FullPath));
            }
            else if (System.IO.Directory.Exists(e.FullPath))
            {
                this.Add(this.Recursive ? new ObservableDirectoryCollection(e.FullPath) { Recursive = true } : new ObservableDirectory(e.FullPath));
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("Received change event " + e.ChangeType + ", " + e.FullPath);

            // This will be handled by the ObservableFile for now.
        }

        private void Watcher_Disposed(object sender, EventArgs e)
        {
            Debug.WriteLine("ObservableDirectory received disposed event");
            this.Dispose();
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
