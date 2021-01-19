using System;
using Serilog;

namespace TryNBootLoader.Programm.Extensions
{
    internal static class LoggerExtension
    {
        public static void PrintLine(this ILogger logger, char lineSeparator = '-')
        {
            logger.Information(string.Empty.PadRight(Console.WindowWidth, lineSeparator));
        }
    }
}