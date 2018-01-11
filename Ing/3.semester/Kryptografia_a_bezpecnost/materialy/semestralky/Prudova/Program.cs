using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PrudovaSifra.Helpers;

namespace PrudovaSifra
{
    public class Program
    {
        static void Main(string[] args)
        {
            var text = Reader.Read(Reader.text1);
            Parallel.For(0, 100000, i =>
            {
                var decryptor = new StreamCipher();
                var freqA = new Freq(false);
                decryptor.AttackTry(text, i, freqA, 0.02);
            });
            Console.WriteLine("Brutal force attack done -> now you can show all text");
            var finalKey = int.Parse(Console.ReadLine());
            var d = new StreamCipher();
            d.WriteDecText(text, finalKey);
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();

        }
    }
}

