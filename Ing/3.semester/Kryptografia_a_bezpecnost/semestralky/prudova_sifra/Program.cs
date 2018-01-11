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
           var text = Reader.Read(Reader.text1);
           Parallel.For(0, 100000, i =>
           {
               Console.WriteLine(i);
               var decryptor = new StreamCipher();
               var freqA = new Freq(false);
               decryptor.AttackTry(text, i, freqA, 0.02);
           });

            // for (int i = 0; i <= 100000; i++)
            // {
            //     Console.WriteLine(i);
            //     var decryptor = new StreamCipher();
            //     var freqA = new Freq(false);
            //     decryptor.AttackTry(text, i, freqA, 0.02);
            // }

            Console.WriteLine("Brute-force attack done!");
            Console.WriteLine("Enter the KEY to show all text");
            var finalKey = int.Parse(Console.ReadLine());
            var d = new StreamCipher();
            d.WriteDecText(text, finalKey);
            var decryptedTextPath = @"./decrypted_text.txt";
            System.IO.File.WriteAllText(decryptedTextPath, "File:\t" + Reader.text1);
            using (System.IO.StreamWriter file = 
                new System.IO.StreamWriter(decryptedTextPath, true))
            {
                file.WriteLine();
                file.WriteLine("Key:\t" + finalKey);
                file.WriteLine();
                file.WriteLine();
                file.WriteLine(d.Decrypt(finalKey, text));
            }
        }
    }

    public class Reader
    {
        public const string text1 = @"./EncryptedTexts/encText1.txt";
        public const string text2 = @"./EncryptedTexts/encText2.txt";
        public const string text3 = @"./EncryptedTexts/encText3.txt";
        public const string text4 = @"./EncryptedTexts/encText4.txt";

        public static string Read(string paPath)
        {
            string readContents;
            using (var streamReader = new StreamReader(paPath))
            {
                readContents = streamReader.ReadToEnd();
            }
            return readContents;
        }
    }

    public class Freq
    {
        public Dictionary<char, double> FreqBase { get; }

        public Freq(bool paIsInSlovak)
        {
            if (paIsInSlovak)
            {
                FreqBase = new Dictionary<char, double>()
                {
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
            else
            {
                FreqBase = new Dictionary<char, double>()
                {
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

        public bool FreqTest(string paText, double paAverageDiff)
        {
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

            var diff = charCounts.Sum(charCount => Math.Abs((charCount.Value / (double) charSum) - FreqBase[charCount.Key]));

            return (diff / FreqBase.Count) <= paAverageDiff;
        }
    }

    public class StreamCipher
    {
        private long _My_randx;

        private void my_seed(long paS)
        {
            _My_randx = paS;
        }

        private double My_rand()
            /* generuje nahodne cislo v intervale <0,1) */
        {
            /* prechod do dalsieho stavu generatora (LL na konci cisla znamena typ long long)*/
            _My_randx = (84589* _My_randx + 45989) % 217728;
            /* vypocet navratovej hodnoty */
            return (double)_My_randx / 217728.0;
        }

        public string Decrypt(long paKey, string paCipherText)
        {
            // nastav random seed - toto je hodnota kluca
            my_seed(paKey);

            int c, k, p;
            char currChar;
            var result = string.Empty;
            foreach (var ch in paCipherText)
            {
                currChar = char.ToUpper(ch);
                if (currChar >= 'A' && currChar <= 'Z')
                {
                    c = currChar - 'A';
                    k = (int) (26 * My_rand());
                    p = (c + (26 - k)) % 26;
                    result += (char) ('A' + p);
                }
                else
                {
                    result += currChar;
                }
            }
            return result;
        }

        public void AttackTry(string paText, long paKey, Freq paActFreq, double paMaxAwgDiff)
        {
            var resStr = Decrypt(paKey, paText);
            if (paActFreq.FreqTest(resStr, paMaxAwgDiff))
                Console.WriteLine("Key:"+ paKey + " -> " + resStr.Substring(0, 60));
        }

        public void WriteDecText(string paText, long paKey)
        {
            Console.WriteLine(Decrypt(paKey, paText));
        }
    }   
}

