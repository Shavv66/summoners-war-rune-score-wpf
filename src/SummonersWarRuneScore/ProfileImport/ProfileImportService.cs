using SummonersWarRuneScore.Domain.Constants;
using System.IO;

namespace SummonersWarRuneScore.ProfileImport
{
	public class ProfileImportService : IProfileImportService
    {
        public void ImportFile(string filePath)
        {
			File.Copy(filePath, FileConstants.CURRENT_PROFILE_PATH);
        }
    }
}
