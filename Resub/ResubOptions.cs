using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Resub
{
    /// <summary>
    /// Valid parameters:
    /// resub.exe [inputfile_options] -i inputfile -offset offsetvalue [outputfile_options] -o outputfile
    /// </summary>
    public class ResubOptions
    {
        public static readonly Encoding DefaultInputEncoding = Encoding.GetEncoding(1252);
        
        public string InputPath { get; private set; }

        public Encoding InputEncoding { get; private set; }

        public string OutputPath { get; private set; }
        public int Offset { get; private set; }

        public bool IsValid => !string.IsNullOrEmpty(InputPath) &&
                               !string.IsNullOrEmpty(OutputPath) &&
                               _isValidOffset && InputEncoding != null;

        public ResubOptions(string[] arguments)
        {
            _arguments = arguments;
            ReadInputFileOptions();
            ReadInputFilePath();
            ReadOffset();
            ReadOutputFileOptions();
            ReadOutputFilePath();
        }

        public static string GetHelp()
        {
            return @"resub.exe [inputfile_options] -i inputfile -offset offsetvalue [[outputfile_options] -o outputfile]";
        }

        private void ReadInputFileOptions()
        {
            InputEncoding = DefaultInputEncoding;
            // for the time we just skip until we find the -i switch
            int i = -1;
            while (i + 1 < _arguments.Length && _arguments[i + 1] != "-i")
            {
                var currentArgument = _arguments[i + 1];
                if (currentArgument.StartsWith("-ienc="))
                {
                    // utf-8
                    // windows-1252
                    string encoding = currentArgument.Substring("-ienc=".Length);
                    try
                    {
                        InputEncoding = Encoding.GetEncoding(encoding);
                    }
                    catch (ArgumentException)
                    {
                        InputEncoding = null;
                    }
                }
                i++;
            }
            _indexLastRead = i;
        }

        private void ReadInputFilePath()
        {
            if (_indexLastRead < _arguments.Length - 2 && _arguments[_indexLastRead+1] == "-i")
            {
                InputPath = _arguments[_indexLastRead + 2];
            }
            _indexLastRead += 2;
        }

        private void ReadOffset()
        {
            if (_indexLastRead < _arguments.Length - 2 && _arguments[_indexLastRead + 1] == "-offset")
            {
                Offset = ParseOffset(_arguments[_indexLastRead + 2]);
            }
            _indexLastRead += 2;
        }

        private void ReadOutputFileOptions()
        {
            // for the time we just skip until we find the -o switch
            int i = _indexLastRead;
            while (i + 1 < _arguments.Length && _arguments[i + 1] != "-o")
            {
                i++;
            }
            _indexLastRead = i;
        }

        private void ReadOutputFilePath()
        {
            if (_indexLastRead < _arguments.Length - 2 && _arguments[_indexLastRead + 1] == "-o")
            {
                OutputPath = _arguments[_indexLastRead + 2];
            }
            _indexLastRead += 2;
        }

        private int ParseOffset(string offsetString)
        {
            _isValidOffset = true;
            var match = OffsetRegex.Match(offsetString);
            if (!match.Success)
            {
                _isValidOffset = false;
                return 0;
            }

            if (string.IsNullOrEmpty(match.Groups["hours"].Value) &&
                string.IsNullOrEmpty(match.Groups["minutes"].Value) &&
                string.IsNullOrEmpty(match.Groups["seconds"].Value) &&
                string.IsNullOrEmpty(match.Groups["milliseconds"].Value))
            {
                _isValidOffset = false;
                return 0;
            }

            int hours = string.IsNullOrEmpty(match.Groups["hours"].Value) ? 0 : Int32.Parse(match.Groups["hours"].Value);
            int minutes = string.IsNullOrEmpty(match.Groups["minutes"].Value) ? 0 : Int32.Parse(match.Groups["minutes"].Value);
            int seconds = string.IsNullOrEmpty(match.Groups["seconds"].Value) ? 0 : Int32.Parse(match.Groups["seconds"].Value);
            int milliseconds = string.IsNullOrEmpty(match.Groups["milliseconds"].Value) ? 0 : Int32.Parse(match.Groups["milliseconds"].Value);
            int sign = (match.Groups["plusminus"].Value != "-") ? 1 : (-1);

            return sign * (milliseconds + seconds * 1000 + minutes * 60 * 1000 + hours * 60 * 60 * 1000);
        }

        private int _indexLastRead = 0;
        private bool _isValidOffset;
        private readonly string[] _arguments;
        private static readonly Regex OffsetRegex = new Regex(@"^(?<plusminus>[+-]?)((?<hours>\d+)(?:h))?((?<minutes>\d+)(?:m))?((?<seconds>\d+)(?:s))?((?<milliseconds>\d+)(?:ms))?$", RegexOptions.Compiled);
    }
}