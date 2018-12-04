using System;
using System.Collections.ObjectModel;

namespace Sokoban.WpfUI.SceneTree
{
	public class Scene
	{
		public ObservableCollection<ObservableCollection<SceneObject>> Layers
		{
			get; private set;
		}
		
		public int FieldWidth { get; private set; }
		public int FieldHeight { get; private set; }
		
		public Scene(Game.Session session)
		{
			FieldWidth = session.CurrentState.Field.Width;
			FieldHeight = session.CurrentState.Field.Height;
			
			Layers = new ObservableCollection<ObservableCollection<SceneObject>>();
			
			var tileLayer = new ObservableCollection<SceneObject>();
			for (int row = 0; row < session.CurrentState.Field.Height; row++) {
				for (int col = 0; col < session.CurrentState.Field.Width; col++) {
					tileLayer.Add(new TileObject(session, row, col));
				}
			}
			Layers.Add(tileLayer);
			
			var mainLayer = new ObservableCollection<SceneObject>();
			mainLayer.Add(new PlayerObject(session));
			for (int i = 0; i < session.CurrentState.BoxesCoords.Length; i++) {
				mainLayer.Add(new BoxObject(session, i));
			}
			Layers.Add(mainLayer);
		}
	}
}
