using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SearchForStringList
{
    class Program
    {
        static void Main(string[] args)
        {
            var searchDirectory = string.Empty;
            var keyWordFile = string.Empty;

            var arguments = ReadArguments(args);

            if (arguments.ContainsKey("searchDirectory"))
                searchDirectory = arguments["searchDirectory"];
            else
            {
                Console.WriteLine("Please enter the search directory path: ");
                searchDirectory = Console.ReadLine();
            }
            if (arguments.ContainsKey("keywordFile"))
                keyWordFile = arguments["keywordFile"];
            else
            {
                Console.WriteLine("Please enter the keyword file path: ");
                keyWordFile = Console.ReadLine();
            }

            Console.WriteLine("Searching for key words in directories");

            var keyWords = File.ReadAllLines(keyWordFile);

            keyWords = keyWords.Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();

            SearchDirectory(searchDirectory, keyWords);

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();
        }

        private static Dictionary<string, string> ReadArguments(string[] args)
        {
            var arguments = new Dictionary<string, string>();

            foreach (string argument in args)
            {
                var keyValues = argument.Split('=');

                if (keyValues.Length == 2)
                {
                    arguments[keyValues[0]] = keyValues[1];
                }
            }

            return arguments;
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
