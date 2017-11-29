using System;

namespace krypto
{
	public class LinearCongruentialGenerator : Random
	{
		public static readonly LinearCongruentialGenerator Cviko4Generator = new LinearCongruentialGenerator(8121, 28411, 134456);

		public int A {
			get;
			private set;
		}

		public int C {
			get;
			private set;
		}

		public int M {
			get;
			private set;
		}

		public int Seed {
			get;
			set;
		}

		public LinearCongruentialGenerator (int a, int c, int m, int seed = 0)
		{
			Seed = seed;
			A = a;
			C = c;
			M = m;
		}

		public override int Next ()
		{
			return ((Seed * A) + C).Mod (M);
		}

		public override double NextDouble ()
		{
			return this.Next () / ((double) M);
		}
	}
}

