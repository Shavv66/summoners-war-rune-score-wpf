using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SummonersWarRuneScore.Components.DataAccess.Domain;
using SummonersWarRuneScore.Components.DataAccess.Services;
using SummonersWarRuneScore.Components.DataAccess.Tools;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Constants;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public class MonsterRoleRepository : IMonsterRoleRepository, IRepositoryTimestampProvider
	{
		private readonly string mFilePath;
		private readonly IAsyncGetAllService<MonsterRole> mAsyncGetAllService;
		private readonly RepositoryCache<MonsterRole> mCache;

		public MonsterRoleRepository() : this(FileConstants.MONSTER_ROLES_PATH, null)
		{
			mAsyncGetAllService = new AsyncGetAllService<MonsterRole>(GetAll);
		}

		public MonsterRoleRepository(string filePath, IAsyncGetAllService<MonsterRole> asyncGetAllService)
		{
			mFilePath = filePath;
			mAsyncGetAllService = asyncGetAllService;
			mCache = new RepositoryCache<MonsterRole>(this);
		}

		public async Task<List<MonsterRole>> GetAllAsync()
		{
			return await mAsyncGetAllService.GetTask();
		}

		private List<MonsterRole> GetAll()
		{
			if (!File.Exists(mFilePath))
			{
				return new List<MonsterRole>();
			}

			if (!mCache.CachedAllIsValid())
			{
				string json = File.ReadAllText(mFilePath);
				mCache.CacheAll(JsonConvert.DeserializeObject<List<MonsterRole>>(json));
			}

			return mCache.CachedAll;
		}

		public async Task<List<MonsterRole>> GetByRuneSetAsync(RuneSet runeSet)
		{
			return await Task.Run(() => GetByRuneSet(runeSet));
		}

		public List<MonsterRole> GetByRuneSet(RuneSet runeSet)
		{
			return GetAll().Where(monsterRole => monsterRole.RuneSets.Contains(runeSet)).ToList();
		}

		public async Task<MonsterRole> AddAsync(MonsterRole monsterRole)
		{
			return await Task.Run(() => Add(monsterRole));
		}

		private MonsterRole Add(MonsterRole monsterRole)
		{
			List<MonsterRole> allRoles = GetAll();
			int id = allRoles.Count > 0 ? allRoles.Select(existingMonsterRole => existingMonsterRole.Id).Max() + 1 : 0;

			MonsterRole newRole = new MonsterRole(id, monsterRole.Name, monsterRole.RuneSets, monsterRole.HpPercentWeight, monsterRole.AtkPercentWeight, monsterRole.DefPercentWeight, 
				monsterRole.ExpectedBaseHp, monsterRole.ExpectedBaseAtk, monsterRole.ExpectedBaseDef, monsterRole.SpdWeight, monsterRole.CritRateWeight, monsterRole.CritDmgWeight,
				monsterRole.ResistanceWeight, monsterRole.AccuracyWeight);

			allRoles.Add(newRole);
			WriteRoles(allRoles);

			return newRole;
		}

		public async Task<MonsterRole> UpdateAsync(MonsterRole monsterRole)
		{
			return await Task.Run(() => Update(monsterRole));
		}

		private MonsterRole Update(MonsterRole monsterRole)
		{
			List<MonsterRole> allRoles = GetAll();
			MonsterRole roleToUpdate = allRoles.Find(existingMonsterRole => existingMonsterRole.Id == monsterRole.Id);

			if (roleToUpdate == null)
			{
				throw new Exception("Failed to update monster role: The given monster role was not found");
			}
			
			roleToUpdate.HpPercentWeight = monsterRole.HpPercentWeight;
			roleToUpdate.AtkPercentWeight = monsterRole.AtkPercentWeight;
			roleToUpdate.DefPercentWeight = monsterRole.DefPercentWeight;
			roleToUpdate.ExpectedBaseHp = monsterRole.ExpectedBaseHp;
			roleToUpdate.ExpectedBaseAtk = monsterRole.ExpectedBaseAtk;
			roleToUpdate.ExpectedBaseDef = monsterRole.ExpectedBaseDef;
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
			Task.Run(() =>
			{
				Task<List<MonsterRole>> getAllTask = GetAllAsync();
				getAllTask.Wait();

				List<MonsterRole> allRoles = getAllTask.Result;
				allRoles.Remove(allRoles.Find(monsterRole => monsterRole.Id == id));
				WriteRoles(allRoles);
			});
		}

		public DateTime GetResourceLastWriteTime()
		{
			return File.GetLastWriteTime(mFilePath);
		}

		private void WriteRoles(List<MonsterRole> roles)
		{
			string json = JsonConvert.SerializeObject(roles);
			File.WriteAllText(mFilePath, json);
		}
	}
}
