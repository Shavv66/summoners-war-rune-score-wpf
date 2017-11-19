using System.Collections.Generic;
using SummonersWarRuneScore.Domain;

namespace SummonersWarRuneScore.DataAccess
{
	public interface IMonsterRolesRepository
	{
		List<MonsterRole> GetAll();
		List<MonsterRole> GetByRuneSet(RuneSet runeSet);
		MonsterRole Add(MonsterRole monsterRole);
		MonsterRole Update(MonsterRole monsterRole);
		void Delete(int id);
	}
}
