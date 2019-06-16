using SummonersWarRuneScore.Client.ViewModels.Domain;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Filtering;
using SummonersWarRuneScore.Components.RuneScoring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SummonersWarRuneScore.Client.ViewModels
{
	public class ScoresViewModel : IProfileImportListener, INotifyPropertyChanged
	{
		public IRuneRepository RuneRepository { get; }
		public IMonsterRoleRepository MonsterRoleRepository { get; }

		public IRuneScoringService RuneScoringService { get; }
		public IScoreRankingService ScoreRankingService { get; }
		public IRuneFilteringService RuneFilteringService { get; }

		public IRuneScoreCache RuneScoreCache { get; }
		public IScoreRankCache ScoreRankCache { get; }

		public List<Rune> Runes { get; private set; }

		private Rune mSelectedRune;
		public Rune SelectedRune
		{
			get => mSelectedRune;
			set
			{
				mSelectedRune = value;
				NotifyPropertyChanged("SelectedRune");
			}
		}

		public event EventHandler<EventArgs> ProfileImported;

		public ScoresViewModel(
			IRuneRepository runeRepository,
			IMonsterRoleRepository monsterRoleRepository,
			IRuneScoringService runeScoringService,
			IScoreRankingService scoreRankingService,
			IRuneFilteringService runeFilteringService,
			IRuneScoreCache runeScoreCache,
			IScoreRankCache scoreRankCache)
		{
			RuneRepository = runeRepository;
			MonsterRoleRepository = monsterRoleRepository;

			RuneScoringService = runeScoringService;
			ScoreRankingService = scoreRankingService;
			RuneFilteringService = runeFilteringService;

			RuneScoreCache = runeScoreCache;
			ScoreRankCache = scoreRankCache;
		}

		public async Task UpdateRunes()
		{
			Runes = await RuneRepository.GetAllAsync();
		}

		public async void HandleProfileImport()
		{
			await UpdateRunes();
			ProfileImported?.Invoke(this, new EventArgs());
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
