using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TicTacToe.Classes {

	static class TraceManager {

		private static readonly Process process = Process.GetCurrentProcess();
		private static readonly Stream stream = File.Create($"{process.ProcessName}-{process.StartTime:yyyy.MM.dd-HH.mm.ss}.log");

		static TraceManager() {
			Trace.Listeners.Add(new TextWriterTraceListener(stream));
			Trace.AutoFlush = true;
		}

		/// <summary>
		/// Logs a message to the trace and optionally, to a WPF TextBlock control.
		/// </summary>
		/// <param name="message">The message to log</param>
		/// <param name="control">The optional text block to log to</param>
		public static void Log(string message, TextBlock? control) {
			Trace.WriteLine($"[{DateTime.Now.ToUniversalTime()}]: {message}");
			if (control is TextBlock textBlock)
				textBlock.Text += $"[{DateTime.Now:HH:mm:ss}]: {message}\n";
		}

	}

}
