using System;

namespace krypto
{
	public static class Extensions
	{
		public static char ToUpper(this char character) {
			return char.ToUpper(character);
		}
		public static char ToLower(this char character) {
			return char.ToLower(character);
		}

		public static int Mod(this int k, int n) {  return ((k %= n) < 0) ? k+n : k;  }
	}
}

