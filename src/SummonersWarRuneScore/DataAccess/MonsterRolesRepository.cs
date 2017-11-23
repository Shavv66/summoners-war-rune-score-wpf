using System.Collections.Generic;
using System.Linq;
using SummonersWarRuneScore.Domain;
using Newtonsoft.Json;
using System.IO;
using SummonersWarRuneScore.Domain.Constants;

namespace SummonersWarRuneScore.DataAccess
{
	public class MonsterRolesRepository : IMonsterRolesRepository
	{
		public List<MonsterRole> GetAll()
		{
			string json = File.ReadAllText(FileConstants.MONSTER_ROLES_PATH);
			return JsonConvert.DeserializeObject<List<MonsterRole>>(json);
		}

		public List<MonsterRole> GetByRuneSet(RuneSet runeSet)
		{
			return GetAll().Where(monsterRole => monsterRole.RuneSet == runeSet).ToList();
		}

		public MonsterRole Add(MonsterRole monsterRole)
		{
			List<MonsterRole> allRoles = GetAll();
			int id = allRoles.Select(existingMonsterRole => existingMonsterRole.Id).Max() + 1;

			MonsterRole newRole = new MonsterRole(id, monsterRole.Name, monsterRole.RuneSet, monsterRole.HpFlatWeight, monsterRole.HpPercentWeight, monsterRole.AtkFlatWeight,
				monsterRole.AtkPercentWeight, monsterRole.DefFlatWeight, monsterRole.DefPercentWeight, monsterRole.SpdWeight, monsterRole.CritRateWeight, monsterRole.CritDmgWeight,
				monsterRole.ResistanceWeight, monsterRole.AccuracyWeight);

			allRoles.Add(newRole);
			WriteRoles(allRoles);

			return newRole;
		}

		public MonsterRole Update(MonsterRole monsterRole)
		{
			List<MonsterRole> allRoles = GetAll();
			MonsterRole roleToUpdate = allRoles.Find(existingMonsterRole => existingMonsterRole.Id == monsterRole.Id);

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

			WriteRoles(allRoles);

			return roleToUpdate;
		}

		public void Delete(int id)
		{
			List<MonsterRole> allRoles = GetAll();
			allRoles.Remove(allRoles.Find(monsterRole => monsterRole.Id == id));
			WriteRoles(allRoles);
		}

		private void WriteRoles(List<MonsterRole> roles)
		{
			string json = JsonConvert.SerializeObject(roles);
			File.WriteAllText(FileConstants.MONSTER_ROLES_PATH, json);
		}
	}
}
