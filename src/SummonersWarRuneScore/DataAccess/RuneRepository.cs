using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SummonersWarRuneScore.Domain;
using SummonersWarRuneScore.Domain.Constants;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SummonersWarRuneScore.DataAccess
{
	public class RuneRepository : IRuneRepository
	{
		private string mFilePath;

		public RuneRepository() : this(FileConstants.CURRENT_PROFILE_PATH) { }

		public RuneRepository(string filePath)
		{
			mFilePath = filePath;
		}

		public List<Rune> GetAll()
		{
			string json = File.ReadAllText(mFilePath);
			JObject profile = JObject.Parse(json);
			return profile["runes"].Select(rune => JsonConvert.DeserializeObject<Rune>(JsonConvert.SerializeObject(rune))).ToList();
		}
	}
}
