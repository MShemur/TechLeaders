using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DownloadManager;

namespace ConsoleDownloadManager
{
    class Program
    {
        private static Dictionary<string, string> files;
        private static FileCounter fileCounter;
        private static Logger logger;

        public static void Main(string[] args)
        {
            IFileDownloader fileDownloader = new FileDownloader();
            fileDownloader.OnFailed += PrintError;
            fileDownloader.OnDownloaded += PrintDownloaded;
            files = new Dictionary<string, string>();
            fileCounter = new FileCounter(files);
            StringGetter stringGetter = new StringGetter();
            var strings = stringGetter.GetStringsFromFile();

            var pathToSave = stringGetter.FileAddress.Substring(0, stringGetter.FileAddress.LastIndexOf('\\'));
            logger = new Logger(pathToSave);

            int id = 0;
            foreach (var address in strings)
            {
                files.Add(id.ToString(), address);
                fileDownloader.AddFileToDownloadingQueue(id.ToString(), address, pathToSave + $@"\IMG_{id}.jpg");
                id++;
            }

            Console.ReadKey();
        }

        private static void PrintDownloaded(string downloaded)
        {
            var countOk = fileCounter.CounterOK + 1;
            Console.WriteLine($"File with id = {downloaded} was successfully downloaded: {files[downloaded]}");
            Console.WriteLine("Downloaded: " +
                              Math.Round(
                                  (countOk + fileCounter.CounterMistakes) * 100.0 / files.Count) +
                              "%");
            fileCounter.CounterOK = countOk;
        }

        private static void PrintError(string downloaded, Exception ex)
        {
            var exceptionId = (ex.InnerException as FileDownloader.DownloadException)?.Id;
            Console.Write("Error downloading file id: ");
            Console.WriteLine(exceptionId + $", file: {files[exceptionId ?? string.Empty]}");
            logger.AddLog(ex.Message + ex.StackTrace);
            fileCounter.CounterMistakes++;
        }
    }
}