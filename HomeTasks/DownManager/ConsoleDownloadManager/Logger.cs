using System.IO;

namespace ConsoleDownloadManager
{
    public class Logger
    {
        private readonly string path;

        public Logger(string path)
        {
            this.path = path;
        }

        public void AddLog(string message)
        {
            var fileName = path + @"\\log.txt";
            // if (!File.Exists(fileName))
            // {
            //     File.Create(fileName);
            // }

            File.AppendAllText(fileName, message);
        }
    }
}