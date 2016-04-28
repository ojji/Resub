using System;

namespace Resub
{
    /// <summary>
    ///     Command line invocation:
    ///     resub.exe -help || resub.exe /?
    ///     resub.exe [inputfile_options] -i inputfile -offset offsetvalue [[outputfile_options] -o outputfile]
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-help" || args[0] == "/?")
            {
                Console.WriteLine(ResubOptions.GetHelp());
                return;
            }
            var options = new ResubOptions(args);
            if (!options.IsValid)
            {
                Console.WriteLine(ResubOptions.GetHelp());
                return;
            }

            var resub = new Resub(options);
            try
            {
                resub.ReadjustSubtitles();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error adjusting: {exception.GetType().Name} ({exception.Message})");
            }
        }
    }
}