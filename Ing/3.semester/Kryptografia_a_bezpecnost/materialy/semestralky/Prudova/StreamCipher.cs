using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrudovaSifra.Helpers;

namespace PrudovaSifra
{
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

        private string Decrypt(long paKey, string paCipherText)
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