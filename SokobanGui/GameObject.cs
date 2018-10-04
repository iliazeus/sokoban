using System;
using System.ComponentModel;

namespace SokobanGui
{
	public abstract class GameObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private Sokoban.State state;
		public virtual Sokoban.State State
		{
			get { return state; }
			set { state = value; NotifyPropertyChanged("State"); }
		}
		
		private int row;
		public int Row
		{
			get { return row; }
			protected set { row = value; NotifyPropertyChanged("Row"); }
		}
		
		private int column;
		public int Column
		{
			get { return column; }
			protected set { column = value; NotifyPropertyChanged("Column"); }
		}
		
		public GameObject(Sokoban.State state, int row = 0, int column = 0)
		{
			Row = row;
			Column = column;
			State = state;
		}
	}
}
