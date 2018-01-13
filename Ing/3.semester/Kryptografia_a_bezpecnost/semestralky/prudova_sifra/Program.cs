using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PrudovaSifra
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2) {
                Console.WriteLine("Bad number of arguments!");
                Console.WriteLine("Usage:");
                Console.WriteLine("mono Program.exe " + 
                    "<encrypted_text_file> [decrypted_text_file]");
                Console.WriteLine("The argument [decrypted_text_file] is " +
                    "optional. If [decrypted_text_file] is not provided, " +
                    "decrypted file will be located in the same directory "+
                    "with the same name as the encrypted file, only with " +
                    "appended '.decrypted' extension.");
                return;
            }

            var encryptedTextFile = args[0];
            var decryptedTextFile = string.Empty;
            if (args.Length == 1) {
                decryptedTextFile = encryptedTextFile + ".decrypted";
            } else if (args.Length == 2) {
                decryptedTextFile = args[1];
            }

            var encryptedText = read(encryptedTextFile);
            var charProbabilitiesInOrigLang = 
                new CharProbabilitiesOfLang("english");
            var variance = 0.02;

            // for (int key = 0; key <= 100000; key++) {
            //     var streamCipher = new StreamCipher();
            //     var decryptedText = 
            //         streamCipher.DecryptText(
            //             key,
            //             encryptedText
            //         );

            //     var freqAnalysisOfText = 
            //         new FreqAnalysisOfText(
            //             charProbabilitiesInOrigLang);
                
            //     if  (freqAnalysisOfText.ComputeDifference(
            //             decryptedText) <= variance
            //         )
            //         Console.WriteLine(
            //             "Key: "+ key + " -> " + 
            //             decryptedText.Substring(0, 60));
            // }

            Parallel.For(0, 100000, key => {
                var streamCipher = new StreamCipher();
                var decryptedText = 
                    streamCipher.DecryptText(
                        key,
                        encryptedText
                    );

                var freqAnalysisOfText = 
                    new FreqAnalysisOfText(
                        charProbabilitiesInOrigLang);
                
                if  (freqAnalysisOfText.ComputeDifference(
                        decryptedText) <= variance
                    )
                    Console.WriteLine(
                        "Key: "+ key + " -> " + 
                        decryptedText.Substring(0, 60));
            });

            Console.WriteLine("Brute-force attack done!");
            Console.WriteLine("Enter the KEY to show the entire text");

            var finalKey = int.Parse(Console.ReadLine());
            var finalStreamCipher = new StreamCipher();
            var finalDecryptedText = 
                finalStreamCipher.DecryptText(
                    finalKey, 
                    encryptedText
                );
            Console.WriteLine(finalDecryptedText);

            saveDecryptedTextToFile(
                encryptedTextFile,
                decryptedTextFile,
                finalKey,
                finalDecryptedText);
            
            Console.WriteLine();
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("Decrypted text is located in:");
            Console.WriteLine(decryptedTextFile);
            Console.WriteLine("********************************************************************************");
        }

        private static string read(string paPath) {
            string readContents;
            using (var streamReader = new StreamReader(paPath))
            {
                readContents = streamReader.ReadToEnd();
            }
            return readContents;
        }

        private static void saveDecryptedTextToFile(
            string paEncryptedTextFile,
            string paDecryptedTextFile,
            long paFinalKey,
            string paFinalDecryptedText)
        {
            System.IO.File.WriteAllText(paDecryptedTextFile, "File:\t" + paEncryptedTextFile);
            using (System.IO.StreamWriter file = 
                new System.IO.StreamWriter(paDecryptedTextFile, true))
            {
                file.WriteLine();
                file.WriteLine("Key:\t" + paFinalKey);
                file.WriteLine();
                file.WriteLine();
                file.WriteLine(paFinalDecryptedText);
            }
        }
    }

    public class StreamCipher {
        private long _My_randx;

        private double My_rand()
        {
            // prechod do dalsieho stavu generatora
            _My_randx = (84589* _My_randx + 45989) % 217728;
            return (double)_My_randx / 217728.0;
        }

        private void my_seed(long paS)
        {
            _My_randx = paS;
        }

        public string DecryptText(
            long paKey,
            string paEncryptedText)
        {
            // nastav random seed - toto je hodnota kluca
            my_seed(paKey);

            int indexOfEncryptedChar, indexOfStreamCipherChar, indexOfDecryptedChar;
            char currChar;
            double myRandNum;
            StringBuilder decryptedText = new StringBuilder(String.Empty);
            foreach (var ch in paEncryptedText)
            {
                currChar = char.ToUpper(ch);
                if (currChar >= 'A' && currChar <= 'Z')
                {
                    indexOfEncryptedChar = currChar - 'A';
                    myRandNum = My_rand();
                    indexOfStreamCipherChar = (int) (26 * myRandNum);

                    // aby sme zasifrovany znak odsifrovali,
                    // musime k indexu sifrovaneho znaku pripocitat
                    // opacny prvok znaku sifry "streamCipherChar"
                    // vzhladom na modulo 26
                    indexOfDecryptedChar = 
                        (   indexOfEncryptedChar
                            + 
                            (26 - indexOfStreamCipherChar)
                        ) % 26;
                    decryptedText.Append((char) ('A' + indexOfDecryptedChar));
                }
                else
                {
                    decryptedText.Append(currChar);
                }
            }
            return decryptedText.ToString();
        }
    }

    public class FreqAnalysisOfText {
        CharProbabilitiesOfLang freqHistogram;

        public FreqAnalysisOfText(
            CharProbabilitiesOfLang paFreqHistogram) {
            freqHistogram = paFreqHistogram;
        }

        public double ComputeDifference(string paText) {
            int charSum = 0;
            var charCounts = new Dictionary<char, int>();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                    charCounts.Add(c, 0);
            }

            foreach (var currentChar in paText)
            {
                if (currentChar >= 'A' && currentChar <= 'Z')
                {
                    charSum++;
                    charCounts[currentChar]++;
                }
            }

            var diff =
                charCounts.Sum(charCount => 
                    Math.Abs(
                        (charCount.Value / (double) charSum) - 
                        freqHistogram.ProbValueOfCharKey(charCount.Key)
                    )
                );

            return (diff / freqHistogram.AlphabetSize());
        }
    }

    public class CharProbabilitiesOfLang {
        private Dictionary<char, double> FreqBase;

        public CharProbabilitiesOfLang(string paLang)
        {
            switch (paLang)
            {
                case "slovak":
                    generateForSlovak();
                    break;

                case "english":
                    generateForEnglish();
                    break;
                    
                default:
                    generateForEnglish();
                    break;
            }
        }

        public double ProbValueOfCharKey(char paChar) {
            return FreqBase[paChar];
        }

        public int AlphabetSize() {
            return FreqBase.Count;
        }

        private void generateForSlovak() {
            FreqBase = new Dictionary<char, double>()  {
                        {'A', 0.1116},
                        {'B', 0.0178},
                        {'C', 0.0246},
                        {'D', 0.0376},
                        {'E', 0.0931},
                        {'F', 0.0017},
                        {'G', 0.0018},
                        {'H', 0.0248},
                        {'I', 0.0575},
                        {'J', 0.0216},
                        {'K', 0.0396},
                        {'L', 0.0438},
                        {'M', 0.0358},
                        {'N', 0.0595},
                        {'O', 0.0954},
                        {'P', 0.0301},
                        {'Q', 0.0000},
                        {'R', 0.0471},
                        {'S', 0.0612},
                        {'T', 0.0572},
                        {'U', 0.0331},
                        {'V', 0.0460},
                        {'W', 0.0000},
                        {'X', 0.0003},
                        {'Y', 0.0267},
                        {'Z', 0.0306}
                    };
        }

        private void generateForEnglish() {
            FreqBase = new Dictionary<char, double>()  {
                        {'A', 0.0804 },
                        {'B', 0.0154 },
                        {'C', 0.0306 },
                        {'D', 0.0399 },
                        {'E', 0.1251},
                        {'F', 0.0230},
                        {'G', 0.0196},
                        {'H', 0.0549},
                        {'I', 0.0726},
                        {'J', 0.0016},
                        {'K', 0.0067},
                        {'L', 0.0414},
                        {'M', 0.0253},
                        {'N', 0.0709},
                        {'O', 0.0760},
                        {'P', 0.0200},
                        {'Q', 0.0011},
                        {'R', 0.0612},
                        {'S', 0.0654},
                        {'T', 0.0925},
                        {'U', 0.0271},
                        {'V', 0.0099},
                        {'W', 0.0192},
                        {'X', 0.0019},
                        {'Y', 0.0173},
                        {'Z', 0.0009}
                    };
        }
    }
}

// Sources:
// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/main-and-command-args/
// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/command-line-building-with-csc-exe
// https://www.dotnetperls.com/stringbuilder
// https://msdn.microsoft.com/en-us/library/system.text.stringbuilder(v=vs.110).aspx
// https://msdn.microsoft.com/en-us/library/system.string(v=vs.110).aspx
// https://www.dotnetperls.com/string-switch
// https://msdn.microsoft.com/en-us/library/xfhwa508(v=vs.110).aspx
// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/main-and-command-args/command-line-arguments