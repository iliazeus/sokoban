using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using System.IO;

using Microsoft.Win32;

namespace Sokoban.WpfUI
{
	public partial class App : Application
	{
		public static string ShowOpenPuzzleDialog(Window owner = null)
		{
			var dialog = new OpenFileDialog();
			dialog.Title = "Open a puzzle file";
			dialog.InitialDirectory = Environment.CurrentDirectory;
			if (! dialog.ShowDialog(owner).GetValueOrDefault(false)) return null;
			return dialog.FileName;
		}
		
		public static string ShowLoadGameDialog(Window owner = null)
		{
			var dialog = new OpenFileDialog();
			dialog.Title = "Load game";
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			dialog.InitialDirectory = Path.Combine(appDataPath, "Sokoban");
			if (! dialog.ShowDialog(owner).GetValueOrDefault(false)) return null;
			return dialog.FileName;
		}
		
		public static string ShowSaveGameDialog(Window owner = null)
		{
			var dialog = new SaveFileDialog();
			dialog.Title = "Save game";
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			dialog.InitialDirectory = Path.Combine(appDataPath, "Sokoban");
			dialog.FileName = "sokoban.txt";
			dialog.Filter = "All files|*.*";
			if (! dialog.ShowDialog(owner).GetValueOrDefault(false)) return null;
			return dialog.FileName;
		}
		
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			
			try {
				string puzzleFilePath;
				var args = Environment.GetCommandLineArgs();
				if (args.Length >= 2) puzzleFilePath = args[1];
				else puzzleFilePath = ShowOpenPuzzleDialog();
				if (puzzleFilePath == null) Environment.Exit(0);
				
				Game.Session session;
				using (var stream = File.OpenRead(puzzleFilePath)) {
					session = Game.Session.ReadFromStream(stream);
				}
				
				new GameSessionWindow(session).Show();
				
			} catch (Exception ex) {
				MessageBox.Show(
					caption: "Error",
					icon: MessageBoxImage.Error,
					messageBoxText: ex.Message,
					button: MessageBoxButton.OK
				);
				Environment.Exit(1);
			}
		}
	}
}