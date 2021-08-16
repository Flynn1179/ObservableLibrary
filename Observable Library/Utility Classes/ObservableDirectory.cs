// <copyright file="ObservableDirectory.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Represents an observable directory, that notifies of files being added or removed.
    /// </summary>
    public class ObservableDirectory : SynchronizedObservableList<ObservableFile>
    {
        private readonly FileSystemWatcher watcher;

        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDirectory"/> class.
        /// </summary>
        /// <param name="path">The path of the directory to observe.</param>
        /// <exception cref="ArgumentException">Thrown if the path does not exist.</exception>
        public ObservableDirectory(string path)
        {
            this.path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(path));
            if (!Directory.Exists(this.path))
            {
                throw new ArgumentException("Cannot create an ObservableDirectory for a directory that does not exist.");
            }

            string[] fileNames = Directory.GetFiles(this.path);
            ObservableFile[] files = new ObservableFile[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
            {
                files[i] = new ObservableFile(fileNames[i]);
            }

            this.AddRange(files);

            this.watcher = new FileSystemWatcher
            {
                Path = this.path,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security,
                EnableRaisingEvents = true,
            };

            //// this.watcher.Changed += this.Watcher_Changed;
            this.watcher.Created += this.Watcher_Created;
            this.watcher.Deleted += this.Watcher_Deleted;
            //// this.watcher.Renamed += this.Watcher_Renamed;
            this.watcher.Error += this.Watcher_Error;
            this.watcher.Disposed += this.Watcher_Disposed;
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
                if (this.watcher is not null)
                {
                    //// this.watcher.Changed -= this.Watcher_Changed;
                    this.watcher.Created -= this.Watcher_Created;
                    this.watcher.Deleted -= this.Watcher_Deleted;
                    //// this.watcher.Renamed += this.Watcher_Renamed;
                    this.watcher.Error -= this.Watcher_Error;
                    this.watcher.Disposed -= this.Watcher_Disposed;
                    this.watcher.Dispose();
                }
            }
        }

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            Debug.WriteLine("ObservableDirectory received error " + e + ", " + e.GetException()?.Message);
            if (!Directory.Exists(this.path))
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
                if (string.Equals(this[i].FileName, e.FullPath, StringComparison.Ordinal))
                {
                    this.RemoveAt(i);
                    return;
                }
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("ObservableDirectory received create event: " + e + ", " + e.FullPath);
            this.Add(new ObservableFile(e.FullPath));
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
    }
}
