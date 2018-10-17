using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using Core = Sokoban.Core;

namespace Sokoban.WpfGui
{
	public class GameSession : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		public event EventHandler<StateChangedEventArgs> StateChanged;
		private void OnStateChanged(Core.State oldState,
		                            Core.Move move,
		                            Core.State newState,
		                            bool isUndo = false)
		{
			if (StateChanged != null) {
				StateChanged(this, new StateChangedEventArgs(
					oldState, move, newState, isUndo));
			}
		}
		public class StateChangedEventArgs : EventArgs
		{
			public Core.State OldState { get; private set; }
			public Core.Move Move { get; private set; }
			public Core.State NewState { get; private set; }
			public bool IsUndo { get; private set; }
			
			public StateChangedEventArgs(Core.State oldState,
										 Core.Move move,
										 Core.State newState,
										 bool isUndo = false)
			{
				OldState = oldState;
				Move = move;
				NewState = newState;
				IsUndo = isUndo;
			}
		}
		
		public Core.Puzzle Puzzle { get; private set; }
		
		private Core.State currentState;
		public Core.State CurrentState
		{
			get { return currentState; }
			private set
			{
				currentState = value;
				NotifyPropertyChanged("CurrentState");
			}
		}
		
		private LinkedList<Core.Move> moves, redoMoves;
		private LinkedList<Core.State> undoStack, redoStack;
		private const int maxUndoStackSize = 1000;
		
		public SceneTree.Scene Scene { get; private set; }
		
		public GameSession(Core.Puzzle puzzle)
		{
			Puzzle = puzzle;
			CurrentState = Puzzle.InitialState;
			moves = new LinkedList<Core.Move>();
			redoMoves = new LinkedList<Core.Move>();
			undoStack = new LinkedList<Core.State>();
			redoStack = new LinkedList<Core.State>();
			
			Scene = new SceneTree.Scene(this);
		}
		
		public static GameSession CreateFromStream(Stream stream)
		{
			using (var reader = new StreamReader(stream)) {
				return new GameSession(Core.Puzzle.ParseFrom(reader));
			}
		}
		
		public void Reset()
		{
			moves.Clear();
			redoMoves.Clear();
			undoStack.Clear();
			redoStack.Clear();
			CurrentState = Puzzle.InitialState;
			OnStateChanged(null, Core.Move.Down, Puzzle.InitialState);
		}
		
		public bool ValidateMove(Core.Move move)
		{
			return CurrentState.ValidateMove(move);
		}
		
		public void ApplyMove(Core.Move move)
		{
			var oldState = CurrentState;
			var newState = oldState.ApplyMove(move);
			
			redoStack.Clear();
			redoMoves.Clear();
			if (undoStack.Count == maxUndoStackSize) undoStack.RemoveFirst();
			undoStack.AddLast(oldState);
			moves.AddLast(move);
			
			CurrentState = newState;
			OnStateChanged(oldState, move, newState);
		}
		
		public bool CanMoveUp { get { return ValidateMove(Core.Move.Up); } }
		public bool CanMoveDown { get { return ValidateMove(Core.Move.Down); } }
		public bool CanMoveLeft { get { return ValidateMove(Core.Move.Left); } }
		public bool CanMoveRight { get { return ValidateMove(Core.Move.Right); } }
		
		public void MoveUp() { ApplyMove(Core.Move.Up); }
		public void MoveDown() { ApplyMove(Core.Move.Down); }
		public void MoveLeft() { ApplyMove(Core.Move.Left); }
		public void MoveRight() { ApplyMove(Core.Move.Right); }
		
		public bool CanUndo { get { return undoStack.Count > 0; } }
		public void Undo()
		{
			var oldState = CurrentState;
			var newState = undoStack.Last.Value;
			var move = moves.Last.Value;
			
			undoStack.RemoveLast();
			redoStack.AddLast(oldState);
			moves.RemoveLast();
			redoMoves.AddLast(move);
			
			CurrentState = newState;
			OnStateChanged(oldState, move, newState, isUndo: true);
		}
		
		public bool CanRedo { get { return redoStack.Count > 0; } }
		public void Redo()
		{
			var oldState = CurrentState;
			var newState = redoStack.Last.Value;
			var move = redoMoves.Last.Value;
			
			redoStack.RemoveLast();
			if (undoStack.Count == maxUndoStackSize) undoStack.RemoveFirst();
			undoStack.AddLast(oldState);
			redoMoves.RemoveLast();
			moves.AddLast(move);
			
			CurrentState = newState;
			OnStateChanged(oldState, move, newState);
		}
		
		public Core.Solution MakeSolution()
		{
			var metadata = new Core.SolutionMetadata(
				creationDate: new DateTimeOffset()
			);
			return new Core.Solution(metadata, moves);
		}
		
		public void WriteSolutionToStream(Stream stream)
		{
			using (var writer = new StreamWriter(stream)) {
				MakeSolution().PrintTo(writer);
			}
		}
	}
}
