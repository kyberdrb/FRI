using System;
using System.IO;
using System.Collections.Generic;

namespace krypto
{
	public static class Consts
	{
		public static readonly string sk_ascii = Utils.ReadFileASCII ("externalFiles/sk_ascii.txt");
		public static readonly string text1 = Utils.ReadFileASCII ("externalFiles/text1.txt");
		public static readonly string text2 = Utils.ReadFileASCII ("externalFiles/text2.txt");


		public static readonly Dictionary<char, double> SkAsciiFreqLangWithSpace = Utils.GetCharFrequencyWithRelativeCounts (sk_ascii, ListOfChars.ListOfCharsWithSpace);
		public static readonly Dictionary<char, double> SkAsciiFreqLangWithoutSpace = Utils.GetCharFrequencyWithRelativeCounts (sk_ascii, ListOfChars.ListOfCharsWithoutSpace);

		public const double CoincidenceIndexSvkWithoutSpace = 0.06027;
//		public const double CoincidenceToleranceDefault = 0.005;
		public const double CoincidenceToleranceDefault = 0.01;

		public const int DefaultCharCountForBreaking = 200;
	}
}

