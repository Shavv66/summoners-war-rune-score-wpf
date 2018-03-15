using System.IO;
using SummonersWarRuneScore.Components.Domain.Constants;

namespace SummonersWarRuneScore.ProfileImport
{
	public class ProfileImportService : IProfileImportService
    {
        public void ImportFile(string filePath)
        {
			File.Copy(filePath, FileConstants.CURRENT_PROFILE_PATH, true);
        }
    }
}
