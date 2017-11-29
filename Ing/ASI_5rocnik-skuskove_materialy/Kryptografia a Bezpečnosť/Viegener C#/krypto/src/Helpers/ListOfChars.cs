using System;
using System.Collections.Generic;
using System.Collections;

namespace krypto
{
	public class ListOfChars : IReadOnlyCollection<char>
	{
		public static readonly ListOfChars ListOfCharsWithoutSpace = new ListOfChars();
		public static readonly ListOfChars ListOfCharsWithSpace = new ListOfChars(true);

		List<char> _listOfChars = new List<char> ();
		Dictionary<char, int> _dictOfChars = new Dictionary<char, int>();

		private ListOfChars (bool withSpace = false)
		{
			int i;
			for (i = 0; i < 26; i++) {
				char ch = (char)(i + 65);
				_listOfChars.Add (ch); 
				_dictOfChars.Add (ch, i); 
			}
			if (withSpace) {
				char ch = ' ';
				_listOfChars.Add (ch); 
				_dictOfChars.Add (ch, i); 
			}
		}

		public override string ToString ()
		{
			string str = "";
			foreach (char ch in _listOfChars) {
				if (ch == ' ')
					str += "space" + ", ";					
				else
					str += ch + ", ";
			}
			return str.Substring(0, str.Length - 2);
		}

		public char this[int index] {
			get { return _listOfChars [index]; }
		}

		public int this[char character] {
			get {  
				if (char.IsLower (character))
					throw new ArgumentException ("HEY NOT UPPER?!");
				return _dictOfChars [character]; 
			}
		}

		public bool Contains(char ch) {
			return _dictOfChars.ContainsKey (ch);
		}

		public bool Contains(int num) {
			return 0 <= num && num < _listOfChars.Count;
		}
		
		#region IEnumerable implementation
		public IEnumerator<char> GetEnumerator ()
		{
			return _listOfChars.GetEnumerator();
		}
		#endregion
		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return _listOfChars.GetEnumerator();
		}
		#endregion
		#region IReadOnlyCollection implementation
		public int Count {
			get {
				return _listOfChars.Count;
			}
		}
		#endregion
	}
}

