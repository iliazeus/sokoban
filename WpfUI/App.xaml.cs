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
			dialog.InitialDirectory = Environment.CurrentDirectory;
			if (! dialog.ShowDialog(owner).GetValueOrDefault(false)) return null;
			return dialog.FileName;
		}
		
		public static Core.Puzzle ReadPuzzle(string path)
		{
			try {
				using (var reader = File.OpenText(path)) {
					return Core.Puzzle.ParseFrom(reader);
				}
			} catch (Exception e) {
				MessageBox.Show(
					caption: "Error",
					icon: MessageBoxImage.Error,
					messageBoxText: e.Message,
					button: MessageBoxButton.OK
				);
				return null;
			}
		}
		
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			
			string puzzleFilePath;
			
			var args = Environment.GetCommandLineArgs();
			if (args.Length >= 2) puzzleFilePath = args[1];
			else puzzleFilePath = ShowOpenPuzzleDialog();
			
			if (puzzleFilePath == null) Environment.Exit(0);
			var puzzle = ReadPuzzle(puzzleFilePath);
			if (puzzle == null) Environment.Exit(1);
			new GameSessionWindow(new Game.Session(puzzle)).Show();
		}
	}
}