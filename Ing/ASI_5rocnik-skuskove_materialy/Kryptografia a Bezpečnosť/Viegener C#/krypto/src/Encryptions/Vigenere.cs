using System;
using System.Collections.Generic;

namespace krypto
{
	public static class Vigenere
	{
		public static CipherType Type = CipherType.Polyalphabetic;
		
		public static string Encrypt(CryptoString str, string password) {
			password = password.ToUpper ();
			foreach (var ch in password) {
				if (!str.ListOfChars.Contains (ch))
					throw new ArgumentException ("Cant use that password");
			}

			string result = "";
			int passwordIndex = 0;
			foreach (var ch in str.Text) {
				result += Caesar.ShiftChar (ch, str.ListOfChars[password [passwordIndex]], str.ListOfChars);

				passwordIndex++;

				if (passwordIndex == password.Length)
					passwordIndex = 0;
			}

			return result;
		}

		public static string Decrypt(CryptoString str, string password) {
			password = password.ToUpper ();
			foreach (var ch in password) {
				if (!str.ListOfChars.Contains (ch))
					throw new ArgumentException ("Cant use that password");
			}

			string result = "";
			int passwordIndex = 0;
			foreach (var ch in str.Text) {
				result += Caesar.ShiftChar (ch, -str.ListOfChars[password [passwordIndex]], str.ListOfChars);

				passwordIndex++;

				if (passwordIndex == password.Length)
					passwordIndex = 0;
			}

			return result;
		}

		public static string BreakWithPasswordLengthReturnPassword(CryptoString str, int passwordLength, string comparisonTextInCorrectLang) {
			if (passwordLength == 0)
				throw new ArgumentException ("Cant be zero length password..");
			var charGroups = SplitIntoSmallerStrings (str.Text, passwordLength);
			var password = "";
			foreach (var item in charGroups) {
				password += str.ListOfChars[Caesar.BreakBruteForceReturnShift(new CryptoString(item, str.ListOfChars), comparisonTextInCorrectLang)];
			}

			return password;
		}

		private static string[] SplitIntoSmallerStrings(string str, int passwordLength) {
			var result = new string[passwordLength];
			for (int i = 0; i < result.Length; i++) {
				result [i] = "";
			}
			for (int i = 0; i < str.Length; i += passwordLength) {
				for (int j = 0; j < passwordLength && i + j < str.Length; j++) {
					result [j] += str [i + j];
				}				
			}
			return result;
		}

		public static List<string> BreakBruteForceWithCoincidenceReturnPassword(CryptoString str, double coincidenceTarget, int minPasswordLengthInclusive = 1, int maxPasswordLengthInclusive = 40) {
			int passwordsCount = maxPasswordLengthInclusive - minPasswordLengthInclusive + 1;

			if (passwordsCount < 1)
				throw new ArgumentException ("Min and max pass length is not quite right...");

			Console.WriteLine ("Vigenere decrypting with coincidence...");
			List<string> results = new List<string> ();

			for (int i = 0; i < passwordsCount; i++) {
				Console.Write (".");
				string password = Vigenere.BreakWithPasswordLengthReturnPassword (str, i + minPasswordLengthInclusive, Consts.sk_ascii);
				if (Utils.CompareIsCloseEnough (Utils.GetIndexOfCoincidence (Vigenere.Decrypt (str, password), str.ListOfChars, Consts.DefaultCharCountForBreaking), coincidenceTarget, Consts.CoincidenceToleranceDefault))
					results.Add (password);
			}
			Console.WriteLine ();
			Console.WriteLine ("DONE");

			return results;
		}

		public static string BreakBruteForceReturnPassword(CryptoString str, string comparisonTextInCorrectLang, int minPasswordLengthInclusive = 1, int maxPasswordLengthInclusive = 40) {
			int passwordsCount = maxPasswordLengthInclusive - minPasswordLengthInclusive + 1;

			if (passwordsCount < 1)
				throw new ArgumentException ("Min and max pass length is not quite right...");

			Console.WriteLine ("Vigenere decrypting...");

			Dictionary<char, double> charFreqLang = Utils.GetCharFreqLang (comparisonTextInCorrectLang, str.ListOfChars);					

			double[] distances = new double[passwordsCount];
			string[] passwords = new string[passwordsCount];

			for (int i = 0; i < passwordsCount; i++) {
				Console.Write (".");
				passwords [i] = Vigenere.BreakWithPasswordLengthReturnPassword (str, i + minPasswordLengthInclusive, comparisonTextInCorrectLang);

				var charFreqGuess = Utils.GetCharFrequencyWithRelativeCounts (Vigenere.Decrypt (str, passwords [i]), str.ListOfChars);

				distances [i] = Utils.CalculateDistance (charFreqLang, charFreqGuess);
			}
			Console.WriteLine ();
			Console.WriteLine ("DONE");

			double min = double.MaxValue;
			int minI = -1;
			for (int i = 0; i < distances.Length; i++) {
				if (distances [i] < min) {
					min = distances [i];
					minI = i;
				}
			}


			return passwords[minI];
		}


		public static string FixPassword(string password, CryptoString sourceText, CryptoString targetText)
		{
			if (sourceText.ListOfChars != targetText.ListOfChars)
				throw new ArgumentException ("source and target has to have the same listOfChars");

			password = password.ToUpper ();
			foreach (var ch in password) {
				if (!sourceText.ListOfChars.Contains (ch))
					throw new ArgumentException ("Cant use that password");
			}

			int minTextLength = Math.Min (sourceText.Text.Length, targetText.Text.Length);

			string resultPassword = "";
			int passwordIndex = 0;

			for (int i = 0; i < minTextLength; i++) {
				if (sourceText.Text [i] != targetText.Text [i]) 
					resultPassword += sourceText.ListOfChars [ ( - Caesar.GetShiftFromChars (sourceText.Text [i], targetText.Text [i], sourceText.ListOfChars)).Mod(sourceText.ListOfChars.Count)];
				else
					resultPassword += password[passwordIndex];
				passwordIndex++;
				passwordIndex %= password.Length;
			}

			return resultPassword.Substring (0, password.Length);
		}

	}
}

