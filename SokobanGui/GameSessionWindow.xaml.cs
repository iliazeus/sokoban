using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SokobanGui
{
	public partial class GameSessionWindow : Window
	{
		public GameSession Session { get; private set; }
		
		public GameSessionWindow(GameSession session)
		{
			Session = session;
			InitializeComponent();
			DataContext = session;
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
	}
}