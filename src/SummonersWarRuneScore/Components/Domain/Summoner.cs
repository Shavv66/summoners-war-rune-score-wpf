using Newtonsoft.Json;
using System;

namespace SummonersWarRuneScore.Components.Domain
{
	public class Summoner
	{
		public long Id { get; }
		public string Name { get; }
		public int ManaStones { get; }
		public int Crystals { get; }
		public int WizardCrystalPaid { get; }
		public DateTime LastLogin { get; }
		public string Country { get; }
		public string Language { get; }
		public byte Level { get; }
		public int Experience { get; }
		public int Energy { get; }
		public short MaxEnergy { get; }
		public decimal EnergyPerMinute { get; }
		public short NextEnergyGainSeconds { get; }
		public short ArenaWings { get; }
		public byte MaxArenaWings { get; }
		public short NextArenaWingGainSeconds { get; }
		public short MonsterInventorySlots { get; }
		public long RepMonsterId { get; }
		public bool RepAssigned { get; }
		public int PvpEvent { get; }
		public int MailBoxEvent { get; }
		public short SocialPoints { get; }
		public short MaxSocialPoints { get; }
		public int ArenaPoints { get; }
		public int GuildPoints { get; }
		public short DimensionalCrystals { get; }
		public byte MaxDimensionalCrystals { get; }
		public int TransmogStones { get; }
		public int MaxTransmogStones { get; }
		public int WorldArenaPoints { get; }
		public int SpecialLeaguePoints { get; }
		public int AncientCoins { get; }
		public int LobbyMasterId { get; }
		public int EmblemMasterId { get; }
		public int PeriodEnergyMax { get; }

		[JsonConstructor]
		public Summoner(long wizard_id, string wizard_name, int wizard_mana, int wizard_crystal, int wizard_crystal_paid, DateTime wizard_last_login, string wizard_last_country, string wizard_last_lang,
			byte wizard_level, int experience, int wizard_energy, short energy_max, decimal energy_per_min, short next_energy_gain, short arena_energy, byte arena_energy_max, short arena_energy_next_gain,
			(short number, byte _) unit_slots, long rep_unit_id, bool rep_assigned, int pvp_event, int mail_box_event, short social_point_current, short social_point_max, int honor_point, int guild_point,
			short darkportal_energy, byte darkportal_energy_max, int costume_point, int costume_point_max, int honor_medal, int honor_mark, int event_coin, int lobby_master_id, int emblem_master_id, int period_energy_max)
		{
			Id = wizard_id;
			Name = wizard_name;
			ManaStones = wizard_mana;
			Crystals = wizard_crystal;
			WizardCrystalPaid = wizard_crystal_paid;
			LastLogin = wizard_last_login;
			Country = wizard_last_country;
			Language = wizard_last_lang;
			Level = wizard_level;
			Experience = experience;
			Energy = wizard_energy;
			MaxEnergy = energy_max;
			EnergyPerMinute = energy_per_min;
			NextEnergyGainSeconds = arena_energy_next_gain;
			ArenaWings = arena_energy;
			MaxArenaWings = arena_energy_max;
			NextArenaWingGainSeconds = arena_energy_next_gain;
			MonsterInventorySlots = unit_slots.number;
			RepMonsterId = rep_unit_id;
			RepAssigned = rep_assigned;
			PvpEvent = pvp_event;
			MailBoxEvent = mail_box_event;
			SocialPoints = social_point_current;
			MaxSocialPoints = social_point_max;
			ArenaPoints = honor_point;
			GuildPoints = guild_point;
			DimensionalCrystals = darkportal_energy;
			MaxDimensionalCrystals = darkportal_energy_max;
			TransmogStones = costume_point;
			MaxTransmogStones = costume_point_max;
			WorldArenaPoints = honor_medal;
			SpecialLeaguePoints = honor_mark;
			AncientCoins = event_coin;
			LobbyMasterId = lobby_master_id;
			EmblemMasterId = emblem_master_id;
			PeriodEnergyMax = period_energy_max;
		}
	}
}
