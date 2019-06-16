using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SummonersWarRuneScore.Components.DataAccess.Domain;
using SummonersWarRuneScore.Components.DataAccess.Services;
using SummonersWarRuneScore.Components.DataAccess.Tools;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Constants;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public class RuneRepository : IRuneRepository, IRepositoryTimestampProvider
	{
		private readonly string mFilePath;
		private readonly IAsyncGetAllService<Rune> mAsyncGetAllService;
		private readonly RepositoryCache<Rune> mCache;

		public RuneRepository() : this(FileConstants.CURRENT_PROFILE_PATH, null)
		{
			mAsyncGetAllService = new AsyncGetAllService<Rune>(GetAll);
		}

		public RuneRepository(string filePath, IAsyncGetAllService<Rune> asyncGetAllService)
		{
			mFilePath = filePath;
			mAsyncGetAllService = asyncGetAllService;
			mCache = new RepositoryCache<Rune>(this);
		}

		public async Task<List<Rune>> GetAllAsync()
		{
			return await mAsyncGetAllService.GetTask();
		}

		public List<Rune> GetAll()
		{
			if (!File.Exists(mFilePath))
			{
				return new List<Rune>();
			}

			if (!mCache.CachedAllIsValid())
			{
				string json = File.ReadAllText(mFilePath);
				JObject profile = JObject.Parse(json);

				List<Rune> runes = ParseRunesJson(profile["runes"]);
				foreach (JObject monster in profile["unit_list"])
				{
					runes.AddRange(ParseRunesJson(monster["runes"]));
				}

				mCache.CacheAll(runes);
			}

			return mCache.CachedAll;
		}

		public DateTime GetResourceLastWriteTime()
		{
			return File.GetLastWriteTime(mFilePath);
		}

		private static List<Rune> ParseRunesJson(JToken jsonToken)
		{
			return jsonToken.Select(rune => JsonConvert.DeserializeObject<Rune>(JsonConvert.SerializeObject(rune))).ToList();
		}
	}
}
