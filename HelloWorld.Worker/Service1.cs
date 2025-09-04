using System;
using System.IO;
using System.ServiceProcess;

namespace HelloWorld.Worker
{
    public partial class Service1 : ServiceBase
    {
        private FileSystemWatcher _watcher;
        private readonly string _sourceFolder = @"C:\temp";
        private readonly string _destinationFile = @"C:\Apps\HelloWorld\IN\input.txt";
        private readonly object _lock = new object();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _watcher = new FileSystemWatcher
            {
                Path = _sourceFolder,
                Filter = "*.txt",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime,
                EnableRaisingEvents = true
            };

            _watcher.Created += OnTextFileCreated;
        }

        private void OnTextFileCreated(object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    // Wait until file is ready
                    WaitForFileAccess(e.FullPath);

                    // Ensure destination directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(_destinationFile));

                    // Read source content
                    string content = File.ReadAllText(e.FullPath);

                    // Append to destination
                    File.AppendAllText(_destinationFile, content + Environment.NewLine);

                    File.Delete(e.FullPath); // ✅ Delete after successful append

                }
                catch (Exception)
                {
                    // TODO: Add logging to file/event viewer
                }
            }
        }

        private void WaitForFileAccess(string filePath)
        {
            const int maxAttempts = 10;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                try
                {
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        return; // File is ready
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(500);
                    attempts++;
                }
            }

            throw new IOException($"File {filePath} is not accessible after multiple attempts.");
        }
    }
}