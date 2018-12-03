﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Sokoban.WpfGui
{
	public enum PuzzleSolvedPopUpAction
	{
		ExitGame,
		LevelSelect,
	}
	
	public partial class PuzzleSolvedPopUpWindow : Window
	{
		public GameSession Session { get; private set; }
		public PuzzleSolvedPopUpAction? Action { get; private set; }
		
		public PuzzleSolvedPopUpWindow(GameSession session)
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
	}
}