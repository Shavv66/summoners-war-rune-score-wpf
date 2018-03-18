using SummonersWarRuneScore.Components.Domain;
using System;

namespace SummonersWarRuneScore.Client.UserControls.RoleManager.Events
{
	public class RoleChangedEventArgs : EventArgs
	{
		public MonsterRole ChangedRole { get; private set; }

		public RoleChangedEventArgs(MonsterRole changedRole)
		{
			ChangedRole = changedRole;
		}
	}
}
