using System;
using System.Linq;
using System.Collections.Generic;

namespace krypto
{
	public static class Caesar
	{
		public static CipherType Type = CipherType.Monoalphabetic;

		public static char ShiftChar(char ch, int shift, ListOfChars listOfChars) {
			int charInt = listOfChars[ch];
			charInt += shift;
			charInt = charInt.Mod(listOfChars.Count);
			return listOfChars[charInt];
		}

		public static string Encrypt(CryptoString str, int shift) {
			string result = "";
			foreach (var ch in str.Text) {
				result += ShiftChar (ch, shift, str.ListOfChars);
			}
			return result;
		}

		public static string Decrypt(CryptoString str, int shift) {
			string result = "";
			foreach (var ch in str.Text) {
				result += ShiftChar (ch, -shift, str.ListOfChars);
			}
			return result;
		}

		public static int BreakBruteForceReturnShift(CryptoString str, string comparisonTextInCorrectLang){
			Dictionary<char, double> charFreqLang = Utils.GetCharFreqLang (comparisonTextInCorrectLang, str.ListOfChars);					

			double[] distances = new double[str.ListOfChars.Count];
			for (int shift = 0; shift < str.ListOfChars.Count; shift++) {
				var charFreqGuess = Utils.GetCharFrequencyWithRelativeCounts (Caesar.Decrypt (str, shift), str.ListOfChars);

				distances [shift] = Utils.CalculateDistance (charFreqLang, charFreqGuess);
			}

			double min = double.MaxValue;
			int minI = -1;
			for (int i = 0; i < distances.Length; i++) {
				if (distances [i] < min) {
					min = distances [i];
					minI = i;
				}
			}

			return minI;
		}

		public static int GetShiftFromChars(char source, char target, ListOfChars listOfChars)
		{
			int charIntSource = listOfChars[source];
			int charIntTarget = listOfChars[target];
			return (charIntTarget - charIntSource).Mod (listOfChars.Count);
		}
	}
}

