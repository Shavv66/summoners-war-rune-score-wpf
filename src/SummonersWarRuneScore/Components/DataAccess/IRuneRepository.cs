using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;

namespace SummonersWarRuneScore.Components.DataAccess
{
	public interface IRuneRepository
	{
		List<Rune> GetAll();
	}
}
