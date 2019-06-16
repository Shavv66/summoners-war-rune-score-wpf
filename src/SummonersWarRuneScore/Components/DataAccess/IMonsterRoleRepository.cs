using System.Collections.Generic;
using System.Threading.Tasks;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public interface IMonsterRoleRepository
	{
		Task<List<MonsterRole>> GetAllAsync();
		Task<List<MonsterRole>> GetByRuneSetAsync(RuneSet runeSet);
		Task<MonsterRole> AddAsync(MonsterRole monsterRole);
		Task<MonsterRole> UpdateAsync(MonsterRole monsterRole);
		void Delete(int id);
	}
}
