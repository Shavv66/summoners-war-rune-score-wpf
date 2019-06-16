using SummonersWarRuneScore.Components.Domain;
using System.Threading.Tasks;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public interface ISummonerRepository
	{
		Task<Summoner> GetAsync();
	}
}
