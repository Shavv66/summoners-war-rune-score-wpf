using SummonersWarRuneScore.Domain;
using System.Collections.Generic;

namespace SummonersWarRuneScore.DataAccess
{
	public interface IRuneRepository
	{
		List<Rune> GetAll();
	}
}
