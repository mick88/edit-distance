using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EditDistanceAlgorithm
{
    class Program
    {
        /// <summary>
        /// Prints matrix to the screen
        /// </summary>
        /// <param name="matrix">Matrix of any size</param>
        static void printMatrix(int [,] matrix)
        {
            for (int row=0; row < matrix.GetLength(0); row++)    
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write(String.Format("{0} ", matrix[row,col]));
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Gets the smallest of 3 numbers
        /// </summary>
        static int getMin(int n1, int n2, int n3)
        {
            return Math.Min(Math.Min(n1, n2), n3);
        }

        /// <summary>
        /// Returns numbe of edits required to change word1 to word2
        /// </summary>
        /// <param name="word1">Needle</param>
        /// <param name="word2">Haystack</param>
        /// <returns>Number of edits</returns>
        static int getEditDistance(string word1, string word2)
        {
            int cols = word1.Length+1,
                rows = word2.Length+1;
            int [,] matrix = new int [rows,cols];

            // fill top and left side of the matrix with 1
            for (int i = 0; i < cols; i++) matrix[0, i] = i;
            for (int i = 0; i < rows; i++) matrix[i, 0] = i;

            // fill the matrix
            for (int row = 1; row < rows; row++)
            {
                for (int col = 1; col < cols; col++)
                {
                    int value;

                    if (word1[col - 1] == word2[row - 1])
                    {
                        value = matrix[row - 1, col - 1];
                    }
                    else
                    {
                        value = getMin(matrix[row - 1, col - 1], 
                            matrix[row - 1, col],
                            matrix[row, col - 1]) + 1;
                    }

                    matrix[row, col] = value;
                }
            }

            // result is the number on the bottom-right of the matrix
            return matrix[rows-1, cols-1];            
        }

        /// <summary>
        /// Prints edit distance between 2 numbers to the screen
        /// </summary>
        static int showDistance(string word1, string word2)
        {
            int distance = getEditDistance(word1, word2);
            Console.WriteLine(String.Format("Edit distance from {0} to {1} is {2}", word1, word2, distance));
            return distance;
        }

        /// <summary>
        /// Gets nearest match from the dictionary
        /// </summary>
        /// <param name="word">Word to be matched</param>
        /// <param name="dictionary">Array of known words</param>
        /// <returns>Nearest word</returns>
        static string guessWord(string word, string[] dictionary)
        {
            string bestGuess = null;
            int bestScore = int.MaxValue;

            foreach(string s in dictionary)
            {
                int value = getEditDistance(word, s);
                if (value == 0) return word;
                if (value < bestScore)
                {
                    bestGuess = s;
                    bestScore = value;
                }
            }

            return bestGuess;
        }

        static string[] loadDictionary()
        {
            Console.WriteLine("Loading dictionary...");
            List<string> words = new List<string>();
            
            StreamReader reader = new StreamReader("dictionary.txt");
            while (reader.EndOfStream == false)
            {
                words.Add(reader.ReadLine().Trim());
            }
            reader.Close();
            string [] result = words.ToArray<string>();
            Console.WriteLine(result.Length+" words loaded from dictionary!");
            return result;
        }

        static void Main(string[] args)
        {
            string[] dictionary = loadDictionary();

            while (true)
            {
                Console.WriteLine("Please enter a word:");
                string word = Console.ReadLine();

                if (String.IsNullOrEmpty(word)) return;

                string match = guessWord(word.ToLower(), dictionary);

                if (word == match) Console.WriteLine("{0}! Yes, I know this word!", word);
                else Console.WriteLine(String.Format("{0}? Did you mean {1}?", word, match));
            }
            Console.ReadLine();
        }
    }
}
