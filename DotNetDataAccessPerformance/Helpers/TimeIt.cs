using System;
using System.Diagnostics;

namespace DotNetDataAccessPerformance.Helpers
{
	public class TimeIt : IDisposable
	{
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly int _total;

		public TimeIt(int total)
		{
			_total = total;
			_stopwatch.Start();
		}

		public void Dispose()
		{
			_stopwatch.Stop();
			var stackFrame = new StackFrame(1);
			var method = stackFrame.GetMethod().Name;
			Console.WriteLine("test={0}; total={1}; time={2};", method, _total, _stopwatch.ElapsedMilliseconds);
		}
	}
}