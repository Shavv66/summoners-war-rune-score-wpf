using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SummonersWarRuneScore.Components.DataAccess.Domain;
using SummonersWarRuneScore.Components.DataAccess.Services;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public class SummonerRepository : ISummonerRepository, IRepositoryTimestampProvider
	{
		private readonly string mFilePath;
		private readonly RepositoryCache<Summoner> mCache;

		public SummonerRepository() : this(FileConstants.CURRENT_PROFILE_PATH) { }

		public SummonerRepository(string filePath)
		{
			mFilePath = filePath;
			mCache = new RepositoryCache<Summoner>(this);
		}

		public Summoner Get()
		{
			return GetAll().SingleOrDefault();
		}

		private List<Summoner> GetAll()
		{
			if (!File.Exists(mFilePath))
			{
				return new List<Summoner>();
			}

			if (!mCache.CachedAllIsValid())
			{
				string json = File.ReadAllText(mFilePath);
				JObject profile = JObject.Parse(json);

				Summoner summoner = JsonConvert.DeserializeObject<Summoner>(JsonConvert.SerializeObject(profile["wizard_info"]));

				mCache.CacheAll(new List<Summoner> { summoner });
			}

			return mCache.CachedAll;
		}

		public DateTime GetResourceLastWriteTime()
		{
			return File.GetLastWriteTime(mFilePath);
		}
	}
}
