using SummonersWarRuneScore.Components.Domain;
using System;

namespace SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Events
{
	public class RuneScoringGridSelectionChangedEventArgs : EventArgs
	{
		public Rune SelectedRune { get; }
		
		public RuneScoringGridSelectionChangedEventArgs(Rune selectedRune)
		{
			SelectedRune = selectedRune;
		}
	}
}
