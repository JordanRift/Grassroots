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
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Framework.Services
{
	public class LocalFileSaveService : IFileSaveService
	{
        public FileStorageType StorageMode { get { return FileStorageType.FileSystem; } }

		/// <summary>
		/// Saves each file in the FileUpload to the local "Content\\UserContent\\" folder
		/// and will set the IsErrors on each FileUpload if it is not possible to
		/// save the file.  It will also attempt to delete the previous file name, but
		/// will be silent if the delete fails (file in use, permissions, etc.).
		/// </summary>
		/// <param name="fileUploadList">A list of files to save.</param>
		public void SaveFiles( List<FileUpload> fileUploadList )
		{
			foreach ( FileUpload fileUpload in fileUploadList )
			{
				// skip any that are errors
				if ( fileUpload.IsError )
				{
					continue;
				}

				// Try to save
				try
				{
					string fileExtension = Path.GetExtension( fileUpload.File.FileName );
					string newFileName = string.Format( "{0}{1}", Guid.NewGuid().ToString(), fileExtension );
					string relativePathFileName = Path.Combine( "Content", "UserContent", newFileName );
					string physicalPathFileName = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, relativePathFileName );
					fileUpload.File.SaveAs( physicalPathFileName );
					fileUpload.NewFileName = relativePathFileName;
				}
				catch ( HttpException ex )
				{
					fileUpload.IsError = true;
					fileUpload.ErrorMessage = string.Format( "Unable to save file. {0}", ex.Message );
				}

				// Try to delete
				try
				{

					// Delete the previous file if it's a local file (non http:) stored here.
					// Map the previous file name to a physical file name (if necessary)
					if ( fileUpload.PreviousFileName != null &&
						fileUpload.PreviousFileName != string.Empty &&
						!fileUpload.PreviousFileName.ToLower().StartsWith( "http" ) )
					{
						string deleteFile = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, fileUpload.PreviousFileName );
						if ( System.IO.File.Exists( deleteFile ) )
						{
							System.IO.File.Delete( deleteFile );
						}
					}
				}
				catch ( Exception ex ) // don't care if this fails, it's just a low priority clean-up task
				{
					Elmah.ErrorSignal.FromCurrentContext().Raise( ex );
				}
			}
		}
	}
}
