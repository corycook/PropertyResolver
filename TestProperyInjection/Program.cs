using System;
using PropertyInjection;
using System.Linq.Expressions;
using System.Dynamic;

namespace TestProperyInjection
{
	interface RC 
	{
		int Double { get; set; }
		string String { get; set; }
	}

	interface RD
	{
		int Double { get; }
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			Resolver<RC>.Register<string, int> (n => n.Double, n => n.Length);
			Resolver<RC>.Register<int, int> (n => n.Double, n => n * 2);
			Resolver<RC>.Register<double, int> (n => n.Double, n => (int)(n * n));
			Resolver<RC>.Register<string, string> (n => n.String, n => string.Format("string({0})", n));
			Resolver<RC>.Register<int, string> (n => n.String, n => string.Format ("int({0})", n));
			Resolver<RC>.Register<double, string> (n => n.String, n => string.Format ("double({0})", n));

			Resolver<RD>.Register<string, int> (n => n.Double, n => 0);
			Resolver<RD>.Register<int, int> (n => n.Double, n => 1);
			Resolver<RD>.Register<double, int> (n => n.Double, n => 2);

			var results = new object[] { 3, 1.8, "Test", 6, "Hello World!", 15, 6.5, 4.0 };
			foreach (var i in results) {
				var value = Resolver<RC>.Resolve (i, n => n.String);
				Console.WriteLine("{0}: {1}", i.GetType(), value);
			}
		}
	}
}
