using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SummonersWarRuneScore.Client.UserControls.RoleManager;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using SummonersWarRuneScore.Components.Filtering;
using SummonersWarRuneScore.Components.RuneScoring;

namespace SummonersWarRuneScoreWebsite.Controllers
{
	[Route("api/[controller]")]
	public class ScoredRunesController
	{
		[HttpGet]
		public string Get(RuneSet runeSet, Filter filter)
		{
			var monsterRoleRepository = new MonsterRoleRepository();
			var runeRepository = new RuneRepository();

			var runeFilteringService = new RuneFilteringService();
			var runeScoringService = new RuneScoringService();
			var scoreRankingService = new ScoreRankingService();

			var runeScoreCache = new RuneScoreCache();
			var scoreRankCache = new ScoreRankCache();

			var table = new DataTable();

			table.Columns.Add("Rune ID", typeof(long));
			table.Columns.Add("Location", typeof(string));
			table.Columns.Add("Slot", typeof(int));
			table.Columns.Add("Grade", typeof(int));
			table.Columns.Add("Set", typeof(string));
			table.Columns.Add("Level", typeof(int));
			table.Columns.Add("Primary Stat", typeof(RuneStat));
			table.Columns.Add("In-built Stat", typeof(RuneStat));
			table.Columns.Add("HP%", typeof(int));
			table.Columns.Add("HP", typeof(int));
			table.Columns.Add("ATK%", typeof(int));
			table.Columns.Add("ATK", typeof(int));
			table.Columns.Add("DEF%", typeof(int));
			table.Columns.Add("DEF", typeof(int));
			table.Columns.Add("SPD", typeof(int));
			table.Columns.Add("CRate", typeof(int));
			table.Columns.Add("CDmg", typeof(int));
			table.Columns.Add("RES", typeof(int));
			table.Columns.Add("ACC", typeof(int));

			List<MonsterRole> roles = monsterRoleRepository.GetByRuneSet(runeSet);
			foreach (MonsterRole role in roles)
			{
				table.Columns.Add(role.Name, typeof(decimal));
				table.Columns.Add($"{role.Name} Rank", typeof(int));
			}

			List<Rune> filteredRunes = runeFilteringService.FilterRunes(runeRepository.GetAll(), filter);
			List<RuneScoringResult> scores = runeScoringService.CalculateScores(filteredRunes, roles);
			runeScoreCache.SetScores(scores);
			scoreRankCache.SetRanks(scoreRankingService.CalculateRanks(scores));

			foreach (Rune rune in filteredRunes)
			{
				DataRow row = table.NewRow();
				row["Rune ID"] = rune.Id;
				row["Location"] = rune.Location;
				row["Slot"] = rune.Slot;
				row["Grade"] = rune.Stars;
				row["Set"] = rune.Set;
				row["Level"] = rune.Level;
				row["Primary Stat"] = rune.PrimaryStat;
				row["In-built Stat"] = rune.PrefixStat;

				foreach (RuneStat stat in rune.Substats)
				{
					switch (stat.Type)
					{
				  		case RuneStatType.HpPercent: row["HP%"] = stat.Amount; break;
				  		case RuneStatType.HpFlat: row["HP"] = stat.Amount; break;
				  		case RuneStatType.AtkPercent: row["ATK%"] = stat.Amount; break;
				  		case RuneStatType.AtkFlat: row["ATK"] = stat.Amount; break;
				  		case RuneStatType.DefPercent: row["DEF%"] = stat.Amount; break;
				  		case RuneStatType.DefFlat: row["DEF"] = stat.Amount; break;
				  		case RuneStatType.Spd: row["SPD"] = stat.Amount; break;
				  		case RuneStatType.CriRate: row["CRate"] = stat.Amount; break;
				  		case RuneStatType.CriDmg: row["CDmg"] = stat.Amount; break;
				  		case RuneStatType.Resistance: row["RES"] = stat.Amount; break;
				  		case RuneStatType.Accuracy: row["ACC"] = stat.Amount; break;
					}
				}

				foreach (MonsterRole role in roles)
				{
					RuneScoringResult runeScore = runeScoreCache.GetScore(role.Id, rune.Id);
					ScoreRankingResult scoreRank = scoreRankCache.GetRank(role.Id, rune.Id, ScoreType.Current);

					decimal score = decimal.Round(runeScore.GetScore(ScoreType.Current), 2);
					int rank = scoreRank.Rank;

					row[role.Name] = score;
					row[$"{role.Name} Rank"] = rank;
				}

				table.Rows.Add(row);
			}

			return JsonConvert.SerializeObject(table);
		}
	}
}
