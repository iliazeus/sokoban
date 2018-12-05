using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Sokoban.WpfUI
{
	public enum PuzzleSolvedPopUpAction
	{
		ExitGame,
		LevelSelect,
	}
	
	public partial class PuzzleSolvedPopUpWindow : Window
	{
		public Game.Session Session { get; private set; }
		public PuzzleSolvedPopUpAction? Action { get; private set; }
		
		public PuzzleSolvedPopUpWindow(Game.Session session)
		{
			InitializeComponent();
			Session = session;
			Action = null;
			DataContext = this;
		}
		
		void ExitGameButton_Click(object sender, RoutedEventArgs e)
		{
			Action = PuzzleSolvedPopUpAction.ExitGame;
			DialogResult = true;
		}
		
		void LevelSelectButton_Click(object sender, RoutedEventArgs e)
		{
			Action = PuzzleSolvedPopUpAction.LevelSelect;
			DialogResult = true;
		}
		
		void SaveSolutionButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				var savePath = App.ShowSaveGameDialog(this);
				if (savePath == null) return;
				using (var stream = File.OpenWrite(savePath)) {
					Session.WriteToStream(stream);
				}
			} catch (Exception ex) {
				MessageBox.Show(
					caption: "Error",
					icon: MessageBoxImage.Error,
					messageBoxText: ex.Message,
					button: MessageBoxButton.OK
				);
			}
		}
	}
}