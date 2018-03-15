using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using SummonersWarRuneScore.Domain;

namespace SummonersWarRuneScore.Components.DataAccess
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
