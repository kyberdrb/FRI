using System;
using System.Collections.Generic;

namespace krypto
{
	public class CryptoString
	{
		public string Text {
			get;
			private set;
		}
		public ListOfChars ListOfChars {
			get;
			private set;
		}

		public CryptoString (string normalString, ListOfChars listOfChars, bool clearFromStuff = true)
		{
			if (clearFromStuff)
				Text = Utils.CleanString (normalString, listOfChars);
			else
				Text = normalString;

			ListOfChars = listOfChars;
		}

		public List<char> GetCharFrequency() {
			return Utils.GetCharFrequencySortedDescending (Text, ListOfChars);
		}

		public double GetIndexOfCoincidence() { 
			return Utils.GetIndexOfCoincidence (Text, ListOfChars);
		}

		public override string ToString () { return Text; }
	}
}

