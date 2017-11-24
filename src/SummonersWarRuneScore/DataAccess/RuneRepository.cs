using SummonersWarRuneScore.Domain;
using SummonersWarRuneScore.Domain.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersWarRuneScore.DataAccess
{
	public class RuneRepository : IRuneRepository
	{
		private string mFilePath;

		public RuneRepository() : this(FileConstants.CURRENT_PROFILE_PATH) { }

		public RuneRepository(string filePath)
		{
			mFilePath = filePath;
		}

		public List<Rune> GetAll()
		{
			string json = File.ReadAllText(mFilePath);
			return new List<Rune>();
		}
	}
}
