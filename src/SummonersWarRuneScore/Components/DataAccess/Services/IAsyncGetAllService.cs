using System.Collections.Generic;
using System.Threading.Tasks;

namespace SummonersWarRuneScore.Components.DataAccess.Services
{
	public interface IAsyncGetAllService<T>
	{
		Task<List<T>> GetTask();
	}
}
