using System.IO;

namespace WpfApp
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
 
            File.AppendAllText(fileName, message);
        }
    }
}