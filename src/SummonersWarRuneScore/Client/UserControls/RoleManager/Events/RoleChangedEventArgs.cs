using SummonersWarRuneScore.Components.Domain;
using System;

namespace SummonersWarRuneScore.Client.UserControls.RoleManager.Events
{
	public class RoleChangedEventArgs : EventArgs
	{
		public MonsterRole ChangedRole { get; }
		public bool IsNew { get; }

		public RoleChangedEventArgs(MonsterRole changedRole, bool isNew)
		{
			ChangedRole = changedRole;
			IsNew = isNew;
		}
	}
}
