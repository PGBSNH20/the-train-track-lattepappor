using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Objects;
using System.IO;

namespace TrainEngine.Utils
{
    /* [1][T>][-][-][-][2]
     * [1][-][<T][-][-][-][3]
     * [4][-][-][/][\][-][-][-]
     */
    public class ControllerLog
    {
        public static string Content = string.Empty;
        private const string PATH = @"C:\Users\Sebastian\source\repos\the-train-track-lattepappor\Source\TrainEngine\Data\controllerlog.txt";
        public static void Log(string path, DateTime timestamp)
        {
            string hour = timestamp.ToString("HH:mm");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[Mr_Carlos][{hour}]: {Content}\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Join('\n', Mr_Carlos.TimeLine));

            if (Content != string.Empty)
            {
                File.AppendAllText(PATH, $"[{hour}]: {Content}\n");
            }
            Console.ResetColor();
        }
    }
}
