/*You are probably going to have to change the namespace
or something for this to work, I made it linux using
mono.*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Dela.Mono.Examples
{
	public class WordMaker
	{
		public static void Main(string[] args)
		{
			//Initialize a streamReader to read data file
			StreamReader reader = new StreamReader("wordstats.txt");
			Random rand = new Random();
			
			//Create dictionary relating array indices to aphabet
			Dictionary<int, string> alphabet = new Dictionary<int, string>();
			alphabet.Add(0, "a");
			alphabet.Add(1, "b");
			alphabet.Add(2, "c");
			alphabet.Add(3, "d");
			alphabet.Add(4, "e");
			alphabet.Add(5, "f");
			alphabet.Add(6, "g");
			alphabet.Add(7, "h");
			alphabet.Add(8, "i");
			alphabet.Add(9, "j");
			alphabet.Add(10, "k");
			alphabet.Add(11, "l");
			alphabet.Add(12, "m");
			alphabet.Add(13, "n");
			alphabet.Add(14, "o");
			alphabet.Add(15, "p");
			alphabet.Add(16, "q");
			alphabet.Add(17, "r");
			alphabet.Add(18, "s");
			alphabet.Add(19, "t");
			alphabet.Add(20, "u");
			alphabet.Add(21, "v");
			alphabet.Add(22, "w");
			alphabet.Add(23, "x");
			alphabet.Add(24, "y");
			alphabet.Add(25, "z");


			//Create 2D array of doubles to store letter data
			double[][] data = new double[26][];
			for(int i = 0; i < 26; i++)
			{
				data[i] = new double[26];
			}
			
			for(int i = 0; i < 26; i++)
			{
				//Get the letter out of the way
				reader.ReadLine();

				//get a line of doubles seperated by spaces
				//as a string
				string line = reader.ReadLine();

				//split the line of doubles into a string array
				string[] nums = line.Split(' ');

				for(int j = 0; j < 26; j++)
				{
					
					//add the double to it's spot in the array
					data[i][j] = double.Parse(nums[j]);


				}//end second for

			}//end first for

			//This pile of poop loops through each array so the value
			//of each index is the sum of all the preceding indices.
			//It makes it easier to choose letters based on random
			//numbers.
			for(int i = 0; i < 26; i++)
			{
				for(int j = 0; j < 26; j++)
				{
					if(j > 0)
					{
						data[i][j] += data[i][j-1];

					}//end if

				}//end second for

			}//end first for

			
			Random rnd = new Random();
			int start = rnd.Next(26);
			int prev = 0;
			string word = "";
			
			//Loop for each letter of the word
			for(int i = 0; i < 10; i++)
			{

				//If the loop has just started, then go to the
				//index of the randomly selected first letter
				if(i == 0)
				{
					word += alphabet[start];
					prev = FindLetter(data[start], rand);
				}
				//Otherwise, go to the arrray of the previous 
				//letter
				else
				{
					//Use the dictionnary to get the letter for the
					//corresponding index and add it to the word
					word += alphabet[prev];
					prev = FindLetter(data[prev], rand);
				}
				
			}//end first for

			Console.WriteLine(word);

		}//end Main

		//Helper method to randomly select the next letter
		public static int FindLetter(double[] arr, Random r)
		{
			
			
			double val = r.Next(10000)*.0001;
			for(int i = 0; i < arr.Length; i++)
			{
				if(val < arr[i])
				{
					return i;
				}//end if

			}//end for
			return 4;
		}//end FindLetter

	}//end Class
}//end namespace
