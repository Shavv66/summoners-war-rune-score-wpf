using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SummonersWarRuneScore.Components.DataAccess.Services
{
	// This service can be responsible for all the required logic to do async repository operations.
	// Currently just basic GetAll management, but the async repositories could be improved as they are currently quite unsafe with regards to e.g. multi-tenant scenarios.
	public class AsyncGetAllService<T> : IAsyncGetAllService<T>
	{
		private readonly Func<List<T>> mGetAllAction;
		private readonly object mGetAllLock;

		private Task<List<T>> mGetAllTask;

		public AsyncGetAllService(Func<List<T>> getAllAction)
		{
			mGetAllAction = getAllAction;
			mGetAllLock = new object();
		}
	
		public Task<List<T>> GetTask()
		{
			Task<List<T>> getAllTask;

			lock (mGetAllLock)
			{
				if (mGetAllTask != null)
				{
					getAllTask = mGetAllTask;
				}
				else
				{
					getAllTask = new Task<List<T>>(mGetAllAction);
					getAllTask.ContinueWith(task =>
					{
						lock (mGetAllLock)
						{
							mGetAllTask = null;
						}
					});
					getAllTask.Start();
				}
			}

			return getAllTask;
		}
	}
}
