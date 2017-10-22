using System.Collections.Generic;
using System.Linq;
using SummonersWarRuneScore.Domain;

namespace SummonersWarRuneScore.DataAccess
{
	public class MonsterRolesRepository : IMonsterRolesRepository
	{
		private List<MonsterRole> mMonsterRoles;

		public MonsterRolesRepository()
		{
			mMonsterRoles = new List<MonsterRole>
			{
				new MonsterRole("Support", RuneSet.Energy),
				new MonsterRole("HP based damage dealer", RuneSet.Energy)
			};
		}

		public MonsterRolesRepository(List<MonsterRole> monsterRoles)
		{
			mMonsterRoles = monsterRoles;
		}

		public List<MonsterRole> GetAll()
		{
			return mMonsterRoles;
		}

		public List<MonsterRole> GetByRuneSet(RuneSet runeSet)
		{
			return mMonsterRoles.Where(monsterRole => monsterRole.RuneSet == runeSet).ToList();
		}

		public void Add(MonsterRole monsterRole)
		{
			mMonsterRoles.Add(monsterRole);
		}

		public void Delete(MonsterRole monsterRole)
		{
			mMonsterRoles.Remove(monsterRole);
		}
	}
}
