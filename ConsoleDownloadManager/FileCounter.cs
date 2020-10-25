using System;
using System.Collections.Generic;

namespace ConsoleDownloadManager
{
    public class FileCounter
    {
        public readonly Dictionary<string, string> files;
        public int counterOK = 0;
        public int counterMistakes = 0;

        public int CounterOK
        {
            get => counterOK;
            set
            {
                counterOK = value;
                if (counterOK + counterMistakes >= files.Count) Notify(counterOK, counterMistakes);
            }
        }

        public int CounterMistakes
        {
            get => counterMistakes;
            set
            {
                counterMistakes = value;
                if (counterOK + counterMistakes >= files.Count) Notify(counterOK, counterMistakes);
            }
        }

        public FileCounter(Dictionary<string, string> files)
        {
            this.files = files;
        }

        private static void Notify(in int counterOk, in int counterMistakes)
        {
            Console.WriteLine(
                $"{counterOk} files were downloaded properly, {counterMistakes} were downloaded with mistakes");
        }
    }
}