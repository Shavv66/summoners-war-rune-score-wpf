using System.Collections.Generic;
using System.Threading.Tasks;
using SummonersWarRuneScore.Components.Domain;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public interface IRuneRepository
	{
		Task<List<Rune>> GetAllAsync();
	}
}
