using System;
using System.Collections.Generic;

namespace krypto
{
	public class OneTimePad
	{
		public static string Encrypt(CryptoString str, Random generator) {
			string result = "";
			foreach (var ch in str.Text) {
				result += Caesar.ShiftChar (ch, (int)(generator.NextDouble () * str.ListOfChars.Count), str.ListOfChars);
			}
			return result;
		}

		public static string Decrypt(CryptoString str, Random generator) {
			string result = "";
			foreach (var ch in str.Text) {
				result += Caesar.ShiftChar (ch, - (int) (generator.NextDouble () * str.ListOfChars.Count), str.ListOfChars);
			}
			return result;
		}

		public static List<int> BreakBruteForceReturnSeeds(CryptoString str, LinearCongruentialGenerator generator, bool firstWin = false, int minSeedVal = 0, int maxSeedVal = int.MaxValue) {
			List<int> results = new List<int> ();

			Console.WriteLine ("OneTimePad decrypting...");

			for (int seed = minSeedVal; seed <= maxSeedVal; seed++) {
				Console.Write (".");
				generator.Seed = seed;
				if (Utils.CompareIsCloseEnough (Utils.GetIndexOfCoincidence (Decrypt (str, generator), str.ListOfChars, Consts.DefaultCharCountForBreaking), Consts.CoincidenceIndexSvkWithoutSpace, Consts.CoincidenceToleranceDefault)) {
					results.Add (seed);
					if (firstWin)
						break;
				}
				
			}

			return results;
		}
	}
}

