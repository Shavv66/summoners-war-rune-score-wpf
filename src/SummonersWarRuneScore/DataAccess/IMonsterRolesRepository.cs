using System.Collections.Generic;
using SummonersWarRuneScore.Domain;

namespace SummonersWarRuneScore.DataAccess
{
	public interface IMonsterRolesRepository
	{
		List<MonsterRole> GetAll();
		List<MonsterRole> GetByRuneSet(RuneSet runeSet);
		void Add(MonsterRole monsterRole);
		void Delete(MonsterRole monsterRole);
	}
}
