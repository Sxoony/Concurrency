using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace concurrency_example
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = "large_document.txt";

            Task<string> fileReadTask = ReadLargeFileAsync(filePath);

            Task task1 = Task.Run(() => PrintFullOutput(DrawSquare()));
            Task task2 = Task.Run(() => PrintFullOutput(DrawRectangle()));
            Task task3 = Task.Run(() => PrintFullOutput(DrawPyramid()));

            // Wait for shapes to finish
            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine("Extracting text from document...");
            // Get the read file result (asynchronously)
            string fileContents = await fileReadTask;

            // Display snippet otherwise it'll take way too long
            Console.WriteLine($"\nFinished reading file. Size: {fileContents.Length} characters.");
            int snippetLength = 100;
            Console.WriteLine($"\nFirst {snippetLength} characters of the file:");
            Console.WriteLine(fileContents.Substring(0, Math.Min(snippetLength, fileContents.Length)));
        }

        // Async file reading
        static async Task<string> ReadLargeFileAsync(string path)
        {
            if (!File.Exists(path))
            {
                return "File not found.";
            }

            Console.WriteLine($"Reading file asynchronously: {path}");
            string content = await File.ReadAllTextAsync(path);
            Console.WriteLine("File read completed.");
            return content;
        }
        // Output when possible to avoid jumbled output (otherwise use locks)
        static void PrintFullOutput(string output)
        {
            Console.WriteLine(output);
        }

        static string DrawSquare()
        {
            string result = "\n-- Square --\n";
            for (int i = 0; i < 10; i++)
            {
                result += "* * * * * * * * * *\n";
                Thread.Sleep(100);
            }
            return result;
        }

        static string DrawRectangle()
        {
            string result = "\n-- Rectangle --\n";
            for (int i = 0; i < 8; i++)
            {
                result += "*****\n";
                Thread.Sleep(100);
            }
            return result;
        }

        static string DrawPyramid()
        {
            string result = "\n-- Pyramid --\n";
            for (int i = 1; i <= 7; i++)
            {
                string spaces = new string(' ', 7 - i);
                string stars = new string('*', 2 * i - 1);
                result += spaces + stars + spaces + "\n";
                Thread.Sleep(100);
            }
            return result;
        }
    }
}
