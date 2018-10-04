using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using System.IO;

namespace SokobanGui
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			
			var args = Environment.GetCommandLineArgs();
			var puzzleFilePath = args[1];
			GameSession session;
			using (var stream = File.OpenRead(puzzleFilePath)) {
				session = GameSession.CreateFromStream(stream);
			}
			
			var sessionWindow = new GameSessionWindow(session);
			sessionWindow.Show();
		}
	}
}