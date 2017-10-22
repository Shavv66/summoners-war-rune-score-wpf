namespace SummonersWarRuneScore.Domain
{
	public class MonsterRole
	{
		public string Name { get; private set; }
		public RuneSet RuneSet { get; private set; }
		
		public MonsterRole(string name, RuneSet runeSet)
		{
			Name = name;
			RuneSet = runeSet;
		}
	}
}
