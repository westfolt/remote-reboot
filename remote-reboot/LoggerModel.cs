using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace remote_reboot
{
    class LoggerModel//singleton logger
    {
        private static LoggerModel logger;
        private readonly string logFilePath;

        private LoggerModel(string givenPath)
        {
            logFilePath = givenPath;
            if (!File.Exists(logFilePath))
            {
                using (StreamWriter sw = File.CreateText(logFilePath))
                {
                    sw.WriteLineAsync(DateTime.Now + ": New log file created");
                }
            }
        }

        public static LoggerModel GetLogger(string givenPath)
        {
            if (logger==null)
                logger = new LoggerModel(givenPath);

            return logger;
        }

        public void LogWrite(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))//adds string to log
                {
                    sw.WriteLine(DateTime.Now + ": " + message);
                }
            }
            catch (Exception ex)
            {
                
                throw new NotImplementedException("Not able to write log file");
            }
        }
    }
}
