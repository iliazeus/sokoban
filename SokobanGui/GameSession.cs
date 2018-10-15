using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SokobanGui
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
		private void OnStateChanged(Sokoban.State oldState,
		                            Sokoban.Move move,
		                            Sokoban.State newState,
		                            bool isUndo = false)
		{
			if (StateChanged != null) {
				StateChanged(this, new StateChangedEventArgs(
					oldState, move, newState, isUndo));
			}
		}
		public class StateChangedEventArgs : EventArgs
		{
			public Sokoban.State OldState { get; private set; }
			public Sokoban.Move Move { get; private set; }
			public Sokoban.State NewState { get; private set; }
			public bool IsUndo { get; private set; }
			
			public StateChangedEventArgs(Sokoban.State oldState,
										 Sokoban.Move move,
										 Sokoban.State newState,
										 bool isUndo = false)
			{
				OldState = oldState;
				Move = move;
				NewState = newState;
				IsUndo = isUndo;
			}
		}
		
		public Sokoban.Puzzle Puzzle { get; private set; }
		
		private Sokoban.State currentState;
		public Sokoban.State CurrentState
		{
			get { return currentState; }
			private set
			{
				currentState = value;
				NotifyPropertyChanged("CurrentState");
			}
		}
		
		private LinkedList<Sokoban.Move> moves, redoMoves;
		private LinkedList<Sokoban.State> undoStack, redoStack;
		private const int maxUndoStackSize = 100;
		
		public SceneTree.Scene Scene { get; private set; }
		
		public GameSession(Sokoban.Puzzle puzzle)
		{
			Puzzle = puzzle;
			CurrentState = Puzzle.InitialState;
			moves = new LinkedList<Sokoban.Move>();
			redoMoves = new LinkedList<Sokoban.Move>();
			undoStack = new LinkedList<Sokoban.State>();
			redoStack = new LinkedList<Sokoban.State>();
			
			Scene = new SceneTree.Scene(this);
		}
		
		public static GameSession CreateFromStream(Stream stream)
		{
			using (var reader = new StreamReader(stream)) {
				return new GameSession(Sokoban.Puzzle.ParseFrom(reader));
			}
		}
		
		public void Reset()
		{
			moves.Clear();
			redoMoves.Clear();
			undoStack.Clear();
			redoStack.Clear();
			CurrentState = Puzzle.InitialState;
		}
		
		public bool ValidateMove(Sokoban.Move move)
		{
			return CurrentState.ValidateMove(move);
		}
		
		public void ApplyMove(Sokoban.Move move)
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
		
		public bool CanMoveUp { get { return ValidateMove(Sokoban.Move.Up); } }
		public bool CanMoveDown { get { return ValidateMove(Sokoban.Move.Down); } }
		public bool CanMoveLeft { get { return ValidateMove(Sokoban.Move.Left); } }
		public bool CanMoveRight { get { return ValidateMove(Sokoban.Move.Right); } }
		
		public void MoveUp() { ApplyMove(Sokoban.Move.Up); }
		public void MoveDown() { ApplyMove(Sokoban.Move.Down); }
		public void MoveLeft() { ApplyMove(Sokoban.Move.Left); }
		public void MoveRight() { ApplyMove(Sokoban.Move.Right); }
		
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
		
		public Sokoban.Solution MakeSolution()
		{
			var metadata = new Sokoban.SolutionMetadata(
				creationDate: new DateTimeOffset()
			);
			return new Sokoban.Solution(metadata, moves);
		}
		
		public void WriteSolutionToStream(Stream stream)
		{
			using (var writer = new StreamWriter(stream)) {
				MakeSolution().PrintTo(writer);
			}
		}
	}
}
