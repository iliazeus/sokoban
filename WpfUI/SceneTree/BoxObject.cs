using System;

namespace Sokoban.WpfUI.SceneTree
{
	public class BoxObject : SceneObject
	{
		public override ObjectType ObjectType { get { return ObjectType.Box; }}
		
		private readonly int index;
		
		public BoxObject(GameSession session, int index) : base(session)
		{
			this.index = index;
			Row = session.CurrentState.BoxesCoords[index].Row;
			Column = session.CurrentState.BoxesCoords[index].Col;
		}
		
		protected override void session_StateChanged(
			object sender, GameSession.StateChangedEventArgs e)
		{
			Row = e.NewState.BoxesCoords[index].Row;
			Column = e.NewState.BoxesCoords[index].Col;
		}
	}
}
