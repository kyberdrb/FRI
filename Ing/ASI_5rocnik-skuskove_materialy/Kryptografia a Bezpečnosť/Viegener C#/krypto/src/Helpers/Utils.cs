using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace krypto
{
	public static class Utils
	{
		public static string CleanString(string str, ListOfChars listOfChars) {
			string result = "";
			foreach (char ch in str) {
				char chara = ch.ToUpper ();
				if (listOfChars.Contains (chara)) {
					result += chara;
				}
			}
			return result;
		}

		public static string ReadFileASCII(string path) {
			return File.ReadAllText(path, System.Text.Encoding.ASCII);
		}

		public static bool CompareIsCloseEnough(double value, double targetValue, double tolerance) {
			return value > (targetValue - tolerance) && value < targetValue + tolerance;
		}

		public static double GetIndexOfCoincidence(string str, ListOfChars listOfChars, int numberOfCharsLimit = int.MaxValue) {
			Dictionary<char, int> charFrequency = new Dictionary<char, int> ();
			double total = 0.0;
			numberOfCharsLimit = Math.Min (str.Length, numberOfCharsLimit);

			foreach (char ch in listOfChars) {
				charFrequency.Add(ch, 0);
			}

			for (int i = 0; i < numberOfCharsLimit; i++) {
				char ch = str[i].ToUpper ();

				if (charFrequency.ContainsKey (ch)) {
					charFrequency [ch]++;
					total++;
				}
			}

			double result = 0.0;

			foreach (var item in charFrequency) {
				result += item.Value / total * (item.Value - 1) / (total - 1);
			}

			return result;
		}

		public static Dictionary<char, double> GetCharFrequencyWithRelativeCounts(string str, ListOfChars listOfChars) {
			Dictionary<char, double> charFrequency = new Dictionary<char, double> ();
			double total = 0.0;

			foreach (char ch in listOfChars) {
				charFrequency.Add(ch, 0.0);
			}

			foreach (char character in str) {
				char ch = character.ToUpper ();

				if (charFrequency.ContainsKey (ch)) {
					charFrequency [ch]++;
					total++;
				}
			}

			var result = new Dictionary<char, double> ();
			foreach (var item in charFrequency) {
				result.Add (item.Key, item.Value / total);
			}

			return result;
		}

		public static List<char> GetCharFrequencySortedDescending(string str, ListOfChars listOfChars) {
			Dictionary<char, int> charFrequency = new Dictionary<char, int> ();

			foreach (char ch in listOfChars) {
				charFrequency.Add(ch, 0);
			}

			foreach (char character in str) {
				char ch = character.ToUpper ();

				if (charFrequency.ContainsKey (ch)) {
					charFrequency [ch]++;
				}
			}


			var result = new List<char> ();
			foreach (var keyVal in SortDict (charFrequency)) {
				if (keyVal.Value > 0) {
					result.Add (keyVal.Key);
				}
			}

			return result;
		}

		public static List<KeyValuePair<T1, T2>> SortDict<T1, T2>(Dictionary<T1, T2> dict) where T2 : IComparable{
			var myList = dict.ToList();

			myList.Sort((pair1,pair2) => pair2.Value.CompareTo(pair1.Value));

			return myList;
		}

		public static List<int> Factor(int number) {
			List<int> factors = new List<int>();
			int max = (int)Math.Sqrt(number);  //round down
			for(int factor = 1; factor <= max; ++factor) { //test from 1 to the square root, or the int below it, inclusive.
				if(number % factor == 0) {
					factors.Add(factor);
					if(factor != number/factor) { // Don't add the square root twice!  Thanks Jon
						factors.Add(number/factor);
					}
				}
			}
			return factors;
		}


		public static double CalculateDistance (Dictionary<char, double> charFreq1WithRelativeCounts, Dictionary<char, double> charFreq2WithRelativeCounts)
		{
			double distance = 0.0;
			foreach (var item in charFreq1WithRelativeCounts) {
				distance += Math.Pow (item.Value - charFreq2WithRelativeCounts [item.Key], 2);
			}
			return distance;
		}


		public static Dictionary<char, double> GetCharFreqLang (string comparisonTextInCorrectLang, ListOfChars listOfChars)
		{
			Dictionary<char, double> charFreqLang;
			if (comparisonTextInCorrectLang == Consts.sk_ascii) {
				if (listOfChars == ListOfChars.ListOfCharsWithoutSpace)
					charFreqLang = Consts.SkAsciiFreqLangWithoutSpace;
				else
					charFreqLang = Consts.SkAsciiFreqLangWithSpace;
			}
			else
				charFreqLang = Utils.GetCharFrequencyWithRelativeCounts (comparisonTextInCorrectLang, listOfChars);
			return charFreqLang;
		}

//		return (g, x, y) a*x + b*y = gcd(x, y)
//		public static Egcd(int a, int b)
	}
}

