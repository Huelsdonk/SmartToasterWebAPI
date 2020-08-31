using System;
using System.IO;

namespace ToastApiReact.Utils
{
    public static class EventLogger
    {
        public static void Log(string message)
        {

            Console.WriteLine(message);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                WriteToLog(message, w);
            }

            // Streamwriter implements a text writer to write
            // the characters to the log. Using "using" for Stream writer and
            // reader automatically calls dispose on the method and closes
            // the file so that you don't have to do it manually
            // this calls below WriteToLog method and appends it to log.txt

            using (StreamReader r = File.OpenText("log.txt"))
            {
                DumpLog(r);
            }

            // this reads the log and dumps it to the console
        }

        public static void WriteToLog(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }

        // provides formatting and content for the log message, called above 

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}



