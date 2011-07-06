//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.Web;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Framework.Services
{
	public interface IFileSaveService
	{
		/// <summary>
		/// Interface for saving files (locally, remotely, cloudly, groundly, etcly).
		/// 
		/// Any implementation MUST do the following:
		/// 
		///   1) skip any files where IsError is already true.
		///   2) save the file somewhere that's publically addressable
		///   3) set the NewFileName to be the location (URL) of the file
		///   4) set IsError to true if it is not able to save the file and
		///      any user error message to the ErrorMessage.
		/// 
		/// Any implementation can/should do the following:
		///   5) delete the old/previous file (PreviousFileName) if it exists
		///      on the system being handled by the provider.  If an error
		///      occurs while trying to delete, it is NOT desirable to
		///      set IsError because deleting is a low priority task.
		///    
		/// The incoming file name and file stream can be found on the
		/// FileUpload.File object.
		/// </summary>
		/// <param name="fileList"></param>
		void SaveFiles( List<FileUpload> fileList );
        FileStorageType StorageMode { get; }
	}

	/// <summary>
	/// Helper class for file save services that hold the result of a file save.
	/// 
	/// </summary>
	public class FileUpload
	{
		/// <summary>
		/// The index corresponding to the file on the form
		/// </summary>
		public HttpPostedFileBase File { get; set; }
		public int Index { get; set; }
		public string NewFileName { get; set; }
		public string PreviousFileName { get; set; }
		public int Length { get; set; }
		public bool IsError { get; set; }
		public string ErrorMessage { get; set; }
	}

}
