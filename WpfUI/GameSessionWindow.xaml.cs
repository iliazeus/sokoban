﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

using Microsoft.Win32;

namespace Sokoban.WpfUI
{
	public partial class GameSessionWindow : Window
	{
		public static readonly DependencyProperty SessionProperty = 
			DependencyProperty.Register(
				"Session", typeof(Game.Session),
				typeof(GameSessionWindow),
				new PropertyMetadata(
					propertyChangedCallback: Session_PropertyChanged
				)
			);
		public Game.Session Session
		{
			get { return (Game.Session) GetValue(SessionProperty); }
			set { SetValue(SessionProperty, value); }
		}
		
		public static void Session_PropertyChanged(
			DependencyObject d,
		    DependencyPropertyChangedEventArgs e)
		{
			var self = d as GameSessionWindow;
			var oldSession = e.OldValue as Game.Session;
			var newSession = e.NewValue as Game.Session;
			
			if (oldSession != null) {
				oldSession.PuzzleSolved -= self.Session_PuzzleSolved;
			}
			if (newSession != null) {
				newSession.PuzzleSolved += self.Session_PuzzleSolved;
			}
		}

		public static readonly DependencyProperty SceneProperty =
			DependencyProperty.Register(
				"Scene", typeof(SceneTree.Scene),
				typeof(GameSessionWindow)
			);
		public SceneTree.Scene Scene
		{
			get { return (SceneTree.Scene) GetValue(SceneProperty); }
			set { SetValue(SceneProperty, value); }
		}
		
		public GameSessionWindow(Game.Session session)
		{
			Session = session;
			Scene = new SceneTree.Scene(Session);
			InitializeComponent();
			DataContext = this;
		}
		
		void Session_PuzzleSolved(object sender, EventArgs e)
		{
			var popup = new PuzzleSolvedPopUpWindow(Session);
			var result = popup.ShowDialog();
			if (result != true) return;
			var action = popup.Action;
			switch (action) {
				case PuzzleSolvedPopUpAction.ExitGame:
					Environment.Exit(0);
					break;
				case PuzzleSolvedPopUpAction.LevelSelect:
					ApplicationCommands.Open.Execute(null, this);
					break;
			}
		}
		
		void MoveUpCommandBinding_CanExecute(object sender,
		                                     CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Session.CanMoveUp;
		}
		void MoveUpCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Session.MoveUp();
		}
		void MoveDownCommandBinding_CanExecute(object sender,
		                                     CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Session.CanMoveDown;
		}
		void MoveDownCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Session.MoveDown();
		}
		void MoveLeftCommandBinding_CanExecute(object sender,
		                                     CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Session.CanMoveLeft;
		}
		void MoveLeftCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Session.MoveLeft();
		}
		void MoveRightCommandBinding_CanExecute(object sender,
		                                     CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Session.CanMoveRight;
		}
		void MoveRightCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Session.MoveRight();
		}
		void UndoCommandBinding_CanExecute(object sender,
		                                     CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Session.CanUndo;
		}
		void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Session.Undo();
		}
		void RedoCommandBinding_CanExecute(object sender,
		                                     CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Session.CanRedo;
		}
		void RedoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Session.Redo();
		}
		void RefreshCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void RefreshCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var result = MessageBox.Show(
				caption: "Warning!",
				messageBoxText: "All progress will be lost.",
				icon: MessageBoxImage.Warning,
				button: MessageBoxButton.OKCancel
			);
			if (result == MessageBoxResult.OK) {
				Session.Reset();
			}
		}
		void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try {
				var puzzleFilePath = App.ShowOpenPuzzleDialog();
				if (puzzleFilePath == null) return;
				using (var stream = File.OpenRead(puzzleFilePath)) {
					Session = Game.Session.ReadFromStream(stream);
				}
			} catch (Exception ex) {
				MessageBox.Show(
					caption: "Error",
					icon: MessageBoxImage.Error,
					messageBoxText: ex.Message,
					button: MessageBoxButton.OK
				);
			}
			Scene = new SceneTree.Scene(Session);
		}
		void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
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
		void OpenCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try {
				var savePath = App.ShowLoadGameDialog(this);
				if (savePath == null) return;
				using (var stream = File.OpenRead(savePath)) {
					Session = Game.Session.ReadFromStream(stream);
				}
			} catch (Exception ex) {
				MessageBox.Show(
					caption: "Error",
					icon: MessageBoxImage.Error,
					messageBoxText: ex.Message,
					button: MessageBoxButton.OK
				);
			}
			Scene = new SceneTree.Scene(Session);
		}
		void HelpCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void HelpCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			new AboutWindow().ShowDialog();
		}
		
	}
}
