using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Constants;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public class RuneRepository : IRuneRepository
	{
		private readonly string mFilePath;

		public RuneRepository() : this(FileConstants.CURRENT_PROFILE_PATH) { }

		public RuneRepository(string filePath)
		{
			mFilePath = filePath;
		}

		public List<Rune> GetAll()
		{
			if (!File.Exists(mFilePath))
			{
				return new List<Rune>();
			}

			string json = File.ReadAllText(mFilePath);
			JObject profile = JObject.Parse(json);

			List<Rune> runes = ParseRunesJson(profile["runes"]);
			foreach (JObject monster in profile["unit_list"])
			{
				runes.AddRange(ParseRunesJson(monster["runes"]));
			}

			return runes;
		}

		private static List<Rune> ParseRunesJson(JToken jsonToken)
		{
			return jsonToken.Select(rune => JsonConvert.DeserializeObject<Rune>(JsonConvert.SerializeObject(rune))).ToList();
		}
	}
}
