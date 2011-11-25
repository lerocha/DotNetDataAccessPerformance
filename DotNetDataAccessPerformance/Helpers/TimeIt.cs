using System;
using System.Diagnostics;

namespace DotNetDataAccessPerformance.Helpers
{
	public class TimeIt : IDisposable
	{
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly int _total;
		private readonly string _testName;

		public TimeIt(int total, string testName)
		{
			_total = total;
			_testName = testName;
			_stopwatch.Start();
		}

		public void Dispose()
		{
			_stopwatch.Stop();
			Console.WriteLine("test={0}; total={1}; time={2};", _testName, _total, _stopwatch.ElapsedMilliseconds);
		}
	}
}