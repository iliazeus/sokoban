using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		
		public Sokoban.Puzzle Puzzle { get; private set; }
		
		private Sokoban.State currentState;
		public Sokoban.State CurrentState
		{
			get { return currentState; }
			private set
			{
				currentState = value;
				foreach (var gameObject in GameObjects) {
					gameObject.State = value;
				}
				NotifyPropertyChanged("CurrentState");
			}
		}
		
		private LinkedList<Sokoban.Move> moves;
		private LinkedList<Sokoban.State> undoStack;
		private const int maxUndoStackSize = 100;
		
		public ObservableCollection<GameObject> GameObjects { get; private set; }
		public ObservableCollection<TileObject> Tiles { get; private set; }
		
		public GameSession(Sokoban.Puzzle puzzle)
		{
			GameObjects = new ObservableCollection<GameObject>();
			Tiles = new ObservableCollection<TileObject>();
			Puzzle = puzzle;
			CurrentState = Puzzle.InitialState;
			moves = new LinkedList<Sokoban.Move>();
			undoStack = new LinkedList<Sokoban.State>();
			
			for (int row = 0; row < CurrentState.Field.Height; row++) {
				for (int col = 0; col < CurrentState.Field.Width; col++) {
					Tiles.Add(new TileObject(CurrentState, row, col));
				}
			}
			
			GameObjects.Add(new PlayerObject(CurrentState));
			for (int i = 0; i < CurrentState.BoxesCoords.Length; i++) {
				GameObjects.Add(new BoxObject(CurrentState, i));
			}
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
			moves.AddLast(move);
			if (undoStack.Count == maxUndoStackSize) undoStack.RemoveFirst();
			undoStack.AddLast(CurrentState);
			CurrentState = CurrentState.ApplyMove(move);
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
			moves.RemoveLast();
			CurrentState = undoStack.Last.Value;
			undoStack.RemoveLast();
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
