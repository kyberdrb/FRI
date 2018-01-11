using System.IO;

namespace PrudovaSifra.Helpers
{
    public class Reader
    {
        public const string text1 = @"../../ShifredText/encText1.txt";
        public const string text2 = @"../../ShifredText/encText2.txt";
        public const string text3 = @"../../ShifredText/encText3.txt";
        public const string text4 = @"../../ShifredText/encText4.txt";

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
}
