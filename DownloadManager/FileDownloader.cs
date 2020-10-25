using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DownloadManager
{
    public class FileDownloader : IFileDownloader
    {
        private int degreeOfParallelism = 4;
        private bool degreeOfParallelismSet;
        private readonly Queue<Tuple<string, string, string>> filesToDownload;
        Progress<ProgressModel> progress;

        private readonly List<Task<string>> taskQuery;

        public FileDownloader()
        {
            filesToDownload = new Queue<Tuple<string, string, string>>(100);
            taskQuery = new List<Task<string>>(100);
            progress = new Progress<ProgressModel>();

            progress.ProgressChanged += InvokeInvoker;
        }

        private void InvokeInvoker(object sender, ProgressModel e)
        {
            OnFileProgress?.Invoke(e.Id, e.FileSize, e.TotalRead);
        }

        public event Action<string> OnDownloaded;
        public event Action<string, Exception> OnFailed;
        public event Action<string, int, int> OnFileProgress;

        public void SetDegreeOfParallelism(int degreeOfParallelism)
        {
            if (degreeOfParallelismSet || taskQuery.Any())
                throw new Exception("Degeree of parallelism has already been set");

            this.degreeOfParallelism = degreeOfParallelism;
            degreeOfParallelismSet = true;
        }

        public void AddFileToDownloadingQueue(string fileId, string url, string pathToSave)
        {
            filesToDownload.Enqueue(new Tuple<string, string, string>(fileId, url, pathToSave));
            if (taskQuery.Count < 1)
            {
                var splitted = filesToDownload.Dequeue();

                taskQuery.Add(DownloadingFile(splitted.Item1, splitted.Item2, splitted.Item3, progress));
                Task.Run(ProcessingTaskQueue);
            }
        }

        private async Task ProcessingTaskQueue()
        {
            AddTasks();

            while (taskQuery.Any())
            {
                Task<string> result = null;

                try
                {
                    // Здесь добавляются таски в работу
                    result = await Task.WhenAny(taskQuery);
                    if (result.IsFaulted) await Task.Run(() => throw result.Exception);
                    await Task.Run(() => OnDownloaded?.Invoke(result.Result));
                }
                catch (Exception ex)
                {
                    OnFailed?.Invoke(ex.Message, ex);
                }

                taskQuery.Remove(result);
                if (taskQuery.Count < degreeOfParallelism && filesToDownload.Any())
                {
                    var splitted = filesToDownload.Dequeue();
                    taskQuery.Add(DownloadingFile(splitted.Item1, splitted.Item2, splitted.Item3, progress));
                }

                AddTasks();
            }
        }

        private void AddTasks()
        {
            while (taskQuery.Count < degreeOfParallelism && filesToDownload.Any())
            {
                var splitted = filesToDownload.Dequeue();
                taskQuery.Add(DownloadingFile(splitted.Item1, splitted.Item2, splitted.Item3, progress));
            }
        }

        private async Task<string> DownloadingFile(string id, string requestUri, string pathToSave,
            IProgress<ProgressModel> progress)
        {
            ProgressModel progressReport = new ProgressModel();

            var totalRead = 0;
            var fileSize = 0;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception e)
                    {
                        await Task.Run(() => throw new DownloadException(id));
                    }

                    if (response.Content.Headers.ContentLength.HasValue)
                        fileSize = (int)response.Content.Headers.ContentLength.Value;
                    fileSize = 0;
                    var bufferSize = fileSize != 0 ? fileSize / 1000 : 100000;

                    var buffer = new byte[bufferSize];

                    using (
                        Stream contentStream = await response.Content.ReadAsStreamAsync(),
                        stream = new FileStream(pathToSave, FileMode.Create, FileAccess.Write, FileShare.None,
                            bufferSize, true))
                    {
                        int bytesRead;
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await stream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;

                            await Task.Run(() => ProgressWorker(progress, progressReport, id, fileSize, totalRead));
                        }
                    }
                    return id;
                }
            }


        }
        private void ProgressWorker(IProgress<ProgressModel> progress, ProgressModel progressReport, string id, int fileSize, int totalRead)
        {
            progressReport.Id = id;
            progressReport.FileSize = fileSize;
            progressReport.TotalRead = totalRead;
            progress.Report(progressReport);
        }

        public class DownloadException : Exception
        {
            public string Id { get; set; }

            public DownloadException(string Id)
            {
                this.Id = Id;
            }
        }
    }

    public class ProgressModel
    {
        public string Id { get; set; }
        public int FileSize { get; set; }
        public int TotalRead { get; set; }
    }
}