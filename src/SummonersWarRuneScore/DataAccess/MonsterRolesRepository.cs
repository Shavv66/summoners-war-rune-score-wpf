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
				new MonsterRole(0, "Support", RuneSet.Energy, 0.01m, 1, 0.03m, 0.2m, 0.18m, 0.95m, 1.5m, 0.3m, 0.2m, 0.2m, 1),
				new MonsterRole(1, "HP based damage dealer", RuneSet.Energy, 0.01m, 1, 0.03m, 0.2m, 0.04m, 0.2m, 1.5m, 1.5m, 1, 0.2m, 0.3m)
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

		public MonsterRole Add(MonsterRole monsterRole)
		{
			int id = mMonsterRoles.Select(existingMonsterRole => existingMonsterRole.Id).Max() + 1;
			MonsterRole newRole = new MonsterRole(id, monsterRole.Name, monsterRole.RuneSet, monsterRole.HpFlatWeight, monsterRole.HpPercentWeight, monsterRole.AtkFlatWeight,
				monsterRole.AtkPercentWeight, monsterRole.DefFlatWeight, monsterRole.DefPercentWeight, monsterRole.SpdWeight, monsterRole.CritRateWeight, monsterRole.CritDmgWeight,
				monsterRole.ResistanceWeight, monsterRole.AccuracyWeight);
			mMonsterRoles.Add(newRole);
			return newRole;
		}

		public MonsterRole Update(MonsterRole monsterRole)
		{
			MonsterRole roleToUpdate = mMonsterRoles.Find(existingMonsterRole => existingMonsterRole.Id == monsterRole.Id);

			if (roleToUpdate == null)
			{
				throw new System.Exception("Failed to update monster role: The given monster role was not found");
			}

			roleToUpdate.HpFlatWeight = monsterRole.HpFlatWeight;
			roleToUpdate.HpPercentWeight = monsterRole.HpPercentWeight;
			roleToUpdate.AtkFlatWeight = monsterRole.AtkFlatWeight;
			roleToUpdate.AtkPercentWeight = monsterRole.AtkPercentWeight;
			roleToUpdate.DefFlatWeight = monsterRole.DefFlatWeight;
			roleToUpdate.DefPercentWeight = monsterRole.DefPercentWeight;
			roleToUpdate.SpdWeight = monsterRole.SpdWeight;
			roleToUpdate.CritRateWeight = monsterRole.CritRateWeight;
			roleToUpdate.CritDmgWeight = monsterRole.CritDmgWeight;
			roleToUpdate.ResistanceWeight = monsterRole.ResistanceWeight;
			roleToUpdate.AccuracyWeight = monsterRole.AccuracyWeight;

			return roleToUpdate;
		}

		public void Delete(int id)
		{
			mMonsterRoles.Remove(mMonsterRoles.Find(monsterRole => monsterRole.Id == id));
		}
	}
}
