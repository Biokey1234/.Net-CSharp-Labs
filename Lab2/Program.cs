using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<string> words = new List<string>();
            while (true)
            {

                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Import Words from File");
                Console.WriteLine("2. Bubble Sort Words");
                Console.WriteLine("3. LINQ/Lambda Sort Words");
                Console.WriteLine("4. Count Distinct Words");
                Console.WriteLine("5. Take First 10 Words");
                Console.WriteLine("6. Reverse Each Word and Print");
                Console.WriteLine("7. Words Ending with 'a'");
                Console.WriteLine("8. Words Starting with 'm'");
                Console.WriteLine("9. Words Longer than 5 Characters Containing 's'");
                Console.WriteLine("X. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            words = ImportWordsFromFile("C:/Users/keymu/source/DotnetLabs/Lab2/Words.txt");
                            Console.WriteLine($"Imported {words.Count} words from file.");
                            break;
                        case "2":
                            if (words.Count > 0)
                            {
                                MeasureExecutionTime(() => BubbleSort(words), "Bubble Sort");

                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }

                            break;
                        case "3":
                            if (words.Count > 0)
                            {
                                MeasureExecutionTime(() => LINQSort(words), "LINQ Sort");
                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }
                            break;
                        case "4":
                            if (words.Count > 0)
                            {
                                // Count distinct words in the list
                                CountDistinctWords(words);
                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }
                            break;
                        case "5":
                            if (words.Count > 0)
                            {
                                TakeFirst10Words(words);
                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }
                            break;
                        case "6":
                            if (words.Count > 0)
                            {
                                ReverseEachWord(words);
                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }
                            break;
                        case "7":
                            if (words.Count > 0)
                            {
                                WordsEndingWith(words, 'a');
                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }
                            break;
                        case "8":
                            if (words.Count > 0)
                            {
                                WordsStartingWith(words, 'm');
                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }
                            break;
                        case "9":
                            if (words.Count > 0)
                            {
                                WordsLongerThan5WithS(words);
                            }
                            else
                            {
                                Console.WriteLine("Please import words first.");
                            }
                            break;
                        case "x":
                            Console.WriteLine("Exiting application.");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;

                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Wrong input: {ex}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Wrong input: {ex}");
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

        }




        // Method to import words from a text file
        static IList<string> ImportWordsFromFile(string filePath)
        {
            try
            {   // Check if the file exists
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                IList<string> words = new List<string>();
                // Open the file and read line by line
                using (StreamReader reader = new StreamReader(filePath))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // If the line is not empty or whitespace, add the trimmed word to the list.
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            words.Add(line.Trim());
                        }
                    }

                }
                return words;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while importing words: {ex.Message}");
                return new List<string>();
            }
        }
        // Method to perform bubble sort on a list of strings
        static IList<string> BubbleSort(IList<string> words)
        {
            try
            {
                // Create a copy of the original list to avoid modifying it
                var sortedWords = new List<string>(words);

                // Bubble sort algorithm
                for (int i = 0; i < sortedWords.Count - 1; i++)
                {
                    for (int j = 0; j < sortedWords.Count - i - 1; j++)
                    {
                        // Compare adjacent words and swap if necessary
                        if (string.Compare(sortedWords[j], sortedWords[j + 1], StringComparison.Ordinal) > 0)
                        {
                            //(sortedWords[j], sortedWords[j + 1]) = (sortedWords[j + 1], sortedWords[j]);
                            string temp = sortedWords[j];
                            sortedWords[j] = sortedWords[j + 1];
                            sortedWords[j + 1] = temp;
                        }
                    }
                }

                return sortedWords;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sorting: {ex.Message}");
                return new List<string>();
            }
        }
        // Method to perform LINQ sort on a list of strings
        static IList<string> LINQSort(IList<string> words)
        {
            // Use LINQ to sort the words alphabetically
            return words.OrderBy(word => word).ToList();
        }
        // Method to measure and display the execution time of a sorting method
        static void MeasureExecutionTime(Func<IList<string>> sortMethod, string methodName)
        {
            // Start the stopwatch to measure execution time
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Execute the sorting method
            var sortedWords = sortMethod();

            // Stop the stopwatch
            stopwatch.Stop();

            // Display the execution time and a preview of sorted words
            Console.WriteLine($"{methodName} completed in {stopwatch.ElapsedMilliseconds} ms.");
            Console.WriteLine("Sorted words:");
            Console.WriteLine(string.Join(", ", sortedWords.Take(10)) + "...");
        }
        // Method to count and display distinct words
        static void CountDistinctWords(IList<string> words)
        {
            // Count the number of distinct words
            int distinctCount = words.Distinct().Count();
            Console.WriteLine($"Distinct words count: {distinctCount}");
        }
        // Method to display the first 10 words in the list
        static void TakeFirst10Words(IList<string> words)
        {
            // Take the first 10 words and display them
            var first10Words = words.Take(10);
            Console.WriteLine("First 10 words:");
            Console.WriteLine(string.Join(", ", first10Words));
        }
        // Method to reverse each word and display the result
        static void ReverseEachWord(IList<string> words)
        {
            // Reverse each word using LINQ
            var reversedWords = words.Select(word => new string(word.Reverse().ToArray()));
            Console.WriteLine("Reversed words:");
            Console.WriteLine(string.Join(", ", reversedWords));
        }
        // Method to find and display words ending with a specific character
        static void WordsEndingWith(IList<string> words, char endingChar)
        {
            // Find words ending with the specified character
            var matchingWords = words.Where(word => word.EndsWith(endingChar));
            Console.WriteLine($"Words ending with '{endingChar}': {string.Join(", ", matchingWords)}");
            Console.WriteLine($"Count: {matchingWords.Count()}");
        }
        // Method to find and display words starting with a specific character
        static void WordsStartingWith(IList<string> words, char startingChar)
        {
            // Find words starting with the specified character
            var matchingWords = words.Where(word => word.StartsWith(startingChar));
            Console.WriteLine($"Words starting with '{startingChar}': {string.Join(", ", matchingWords)}");
            Console.WriteLine($"Count: {matchingWords.Count()}");
        }
        // Method to find and display words longer than 5 characters and containing 's'
        static void WordsLongerThan5WithS(IList<string> words)
        {
            // Find words longer than 5 characters and containing 's'
            var matchingWords = words.Where(word => word.Length > 5 && word.Contains('s'));
            Console.WriteLine($"Words longer than 5 characters containing 's': {string.Join(", ", matchingWords)}");
            Console.WriteLine($"Count: {matchingWords.Count()}");
        }
    }




}
