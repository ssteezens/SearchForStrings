using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using CommandLine;

namespace SearchForStringList
{
    class Options
    {
        [Option("searchDirectory", Required = false, HelpText = "The path of the directory to search for key words in")]
        public string SearchDirectory { get; set; }

        [Option("keywordFile", Required = false, HelpText = "The file containing the newline separated list of key words to search for.")]
        public string KeywordFile { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseErrors);
        }

        static void RunOptions(Options options)
        {
            if (string.IsNullOrEmpty(options.SearchDirectory))
            {
                Console.WriteLine("searchDirectory argument not specified");
                Console.WriteLine("Please enter the search directory path: ");
                options.SearchDirectory = Console.ReadLine();
            }
            if (string.IsNullOrEmpty(options.KeywordFile))
            { 
                Console.WriteLine("keywordFile argument not specified");
                Console.WriteLine("Please enter the keyword file path: ");
                options.KeywordFile = Console.ReadLine();
            }

            Console.WriteLine("Searching for key words in directories");

            var keyWords = File.ReadAllLines(options.KeywordFile);

            keyWords = keyWords.Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();

            SearchDirectory(options.SearchDirectory, keyWords);

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();
        }

        static void HandleParseErrors(IEnumerable<Error> errs)
        {

        }

        private static void SearchDirectory(string directory, string[] searchWords)
        {
            var filesInDirectory = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);

            foreach (var word in searchWords)
            {
                Console.WriteLine($"Searching for word {word}");

                // search every file for word
                foreach (var filePath in filesInDirectory)
                {
                    // skip files in bin folder
                    if (filePath.Contains(@"\bin"))
                        continue;

                    var lines = File.ReadAllLines(filePath);

                    foreach (var line in lines)
                    {
                        if (line.Contains(word))
                        {
                            Console.WriteLine($"Key word {word} found in file {filePath} on line {Array.IndexOf(lines, line)}");
                        }
                    }
                }
            }
        }
    }
}
