using System;
using System.IO;

namespace krypto
{
	class MainClass
	{
		public static void Print(object str) {
			Console.WriteLine (str.ToString ());
		}

		public static void Print() {
			Console.WriteLine ();
		}
			
		public static void Main (string[] args)
		{
//			RunVigenereTestNumber (4);
//			VigenereAutomatic (); 

//			Print (Utils.GetIndexOfCoincidence (Consts.text2, ListOfChars.ListOfCharsWithoutSpace));
//			Print (Utils.GetIndexOfCoincidence (Consts.text2, ListOfChars.ListOfCharsWithSpace));

//			VigenereAutomatic ();
//			VigenereAutomaticCoincidence ();
//			OneTimePadAutomatic ();

//			VigenereAutomatic (@"externalFiles/test4.txt");
//			VigenereAutomaticCoincidence (@"externalFiles/test1.txt");


			VigenereSemestralka ();

//			FixPassword (@"externalFiles/text3_enc.txt");


		}

		private static void FixPassword(string path) {
			var test = new CryptoString (File.ReadAllText (path), ListOfChars.ListOfCharsWithoutSpace);
			var fixedPass = Vigenere.FixPassword ("KOKVMIRTEUSWECLAUXBFWWM", new CryptoString ("AKTIVIOTIZOBCIANSKEHKZDRUZENIWTRIBLAVINA", ListOfChars.ListOfCharsWithoutSpace), new CryptoString ("AKTIVISTIZOBCIANSKEHOZDRUZENIA", ListOfChars.ListOfCharsWithoutSpace));
			Print (Vigenere.Decrypt (test, fixedPass));



		}

		private static void VigenereSemestralka() {

			VigenereAutomatic (@"externalFiles/text1_enc.txt");
			VigenereAutomatic (@"externalFiles/text2_enc.txt");
			VigenereAutomatic (@"externalFiles/text3_enc.txt");

			Console.WriteLine ("==============================");

			VigenereAutomaticCoincidence (@"externalFiles/text1_enc.txt");
			VigenereAutomaticCoincidence (@"externalFiles/text2_enc.txt");
			VigenereAutomaticCoincidence (@"externalFiles/text3_enc.txt");
		}

		private static void OneTimePadAutomatic() {
			var test = new CryptoString (File.ReadAllText (@"externalFiles/sprava_enc_otp.txt"), ListOfChars.ListOfCharsWithoutSpace);
//			var passwords = Vigenere.BreakBruteForceWithCoincidenceReturnPassword (test, Consts.CoincidenceIndexSvkWithoutSpace, 20, 30);
			var seeds = OneTimePad.BreakBruteForceReturnSeeds (test, LinearCongruentialGenerator.Cviko4Generator, true, 0, LinearCongruentialGenerator.Cviko4Generator.M + 1);
			foreach (var seed in seeds) {
				Print ();
				var generator = LinearCongruentialGenerator.Cviko4Generator;
				generator.Seed = seed;
				Print (OneTimePad.Decrypt (test, generator));
				Print ();
				Print ();
			}
		}



		private static void VigenereAutomaticCoincidence (string path)
		{
			var test = new CryptoString (File.ReadAllText (path), ListOfChars.ListOfCharsWithoutSpace);
			var passwords = Vigenere.BreakBruteForceWithCoincidenceReturnPassword (test, Consts.CoincidenceIndexSvkWithoutSpace, 20, 30);
			if (passwords.Count == 0)
				throw new Exception ("ZERO PASSWORDS");
			foreach (var pass in passwords) {
				Print (pass);
				Print ("pass length: " + pass.Length);
				Print("-----------------------");
				Print (Vigenere.Decrypt (test, pass));
				Print ();
				Print ();
			}
		}

		private static void VigenereAutomatic (string path)
		{
			var test = new CryptoString (File.ReadAllText (path), ListOfChars.ListOfCharsWithoutSpace);
			var pass = Vigenere.BreakBruteForceReturnPassword (test, Consts.sk_ascii, 20, 29);
			Print (pass);
			Print ("pass length: " + pass.Length);
			Print("-----------------------");
			Print (Vigenere.Decrypt (test, pass));
		}

		private static void RunVigenereTestNumber(int number){
			CryptoString test = null;
			int passLength = 0;

			switch(number) {
			case 1:
				test = new CryptoString (File.ReadAllText (@"externalFiles/test1.txt"), ListOfChars.ListOfCharsWithoutSpace);
				passLength = 26;
				break;
			case 2:
				test = new CryptoString (File.ReadAllText (@"externalFiles/test2.txt"), ListOfChars.ListOfCharsWithoutSpace);
				passLength = 29;
				break;
			case 3:
				test = new CryptoString (File.ReadAllText (@"externalFiles/test3.txt"), ListOfChars.ListOfCharsWithoutSpace);
				passLength = 25;
				break;
			case 4:
				test = new CryptoString (File.ReadAllText (@"externalFiles/test4.txt"), ListOfChars.ListOfCharsWithoutSpace);
				passLength = 27;
				break;

			}
				
			Print (Vigenere.Decrypt (test, Vigenere.BreakWithPasswordLengthReturnPassword (test, passLength, Consts.sk_ascii)));
		}	
	}
}
