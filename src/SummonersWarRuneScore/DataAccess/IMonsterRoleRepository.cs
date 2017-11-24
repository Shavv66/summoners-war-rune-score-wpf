using System.Collections.Generic;
using SummonersWarRuneScore.Domain;
using SummonersWarRuneScore.Domain.Enumerations;

namespace SummonersWarRuneScore.DataAccess
{
	public interface IMonsterRoleRepository
	{
		List<MonsterRole> GetAll();
		List<MonsterRole> GetByRuneSet(RuneSet runeSet);
		MonsterRole Add(MonsterRole monsterRole);
		MonsterRole Update(MonsterRole monsterRole);
		void Delete(int id);
	}
}
