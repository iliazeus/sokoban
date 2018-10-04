using System;

namespace SokobanGui.SceneTree
{
	public class PlayerObject : SceneObject
	{
		public override ObjectType ObjectType { get { return ObjectType.Player; }}
		
		public PlayerObject(GameSession session) : base(session)
		{
			Row = session.CurrentState.PlayerCoords.Row;
			Column = session.CurrentState.PlayerCoords.Col;
		}
		
		protected override void session_StateChanged(
			object sender, GameSession.StateChangedEventArgs e)
		{
			Row = e.NewState.PlayerCoords.Row;
			Column = e.NewState.PlayerCoords.Col;
		}
	}
}
