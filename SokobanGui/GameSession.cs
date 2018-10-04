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
		
		private LinkedList<Sokoban.Move> moves;
		private LinkedList<Sokoban.State> undoStack;
		private const int maxUndoStackSize = 100;
		
		public SceneTree.Scene Scene { get; private set; }
		
		public GameSession(Sokoban.Puzzle puzzle)
		{
			Puzzle = puzzle;
			CurrentState = Puzzle.InitialState;
			moves = new LinkedList<Sokoban.Move>();
			undoStack = new LinkedList<Sokoban.State>();
			
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
			undoStack.Clear();
			CurrentState = Puzzle.InitialState;
		}
		
		public bool ValidateMove(Sokoban.Move move)
		{
			return CurrentState.ValidateMove(move);
		}
		public void ApplyMove(Sokoban.Move move)
		{
			var oldState = CurrentState;
			CurrentState = CurrentState.ApplyMove(move);
			moves.AddLast(move);
			if (undoStack.Count == maxUndoStackSize) undoStack.RemoveFirst();
			undoStack.AddLast(oldState);
			OnStateChanged(oldState, move, CurrentState);
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
			var lastMove = moves.Last.Value;
			moves.RemoveLast();
			var lastState = CurrentState;
			CurrentState = undoStack.Last.Value;
			undoStack.RemoveLast();
			OnStateChanged(lastState, lastMove, CurrentState, isUndo: true);
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
