using System;

namespace Hangman
{
    internal class Program
    {


        private static void Main(string[] args)
        {
            var hangman = new Hangman();
            try
            {
                hangman.Play();
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Please make sure you have a words.txt with comma separated words in the same path as this file.");
                Console.ReadKey();
            }

        }
    }
}
