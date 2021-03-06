﻿using System;

namespace Sokoban.WpfUI.SceneTree
{
	public class PlayerObject : SceneObject
	{
		public override ObjectType ObjectType { get { return ObjectType.Player; }}
		
		public PlayerObject(Game.Session session) : base(session)
		{
			Row = session.CurrentState.PlayerCoords.Row;
			Column = session.CurrentState.PlayerCoords.Col;
		}
		
		protected override void session_StateChanged(
			object sender, Game.Session.StateChangedEventArgs e)
		{
			Row = e.NewState.PlayerCoords.Row;
			Column = e.NewState.PlayerCoords.Col;
		}
	}
}
