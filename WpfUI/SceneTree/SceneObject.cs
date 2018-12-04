using System;
using System.ComponentModel;

namespace Sokoban.WpfUI.SceneTree
{
	public enum ObjectType
	{
		Player, Box, Tile
	}
	
	public abstract class SceneObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		protected Game.Session session;
		
		public abstract ObjectType ObjectType { get; }
		
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

		protected virtual void session_StateChanged(
			object sender, Game.Session.StateChangedEventArgs e) {}
		
		protected SceneObject(Game.Session session)
		{
			this.session = session;
			session.StateChanged += session_StateChanged;
		}
	}
}
