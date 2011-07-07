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
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Framework.Services
{
	/// <summary>
	/// An IFileSaveService implementation to save files to the Amazon S3 cloud.
	/// This requires these three settings in your web.config (AWSAccessKey,
	/// AWSSecretKey, and AWSBucketName)
	///
	/// Setting a AllowPublicRead access policy on the S3 bucket is no longer
	/// needed because we're now setting it explicitly on the file item.
	/// 
	/// S3 "public" Bucket Policy Example...
	/// Everything in the braces (including the braces) goes into the policy
	/// editor when adding a new policy into Amazon S3.
	/// 
	/// Note: The admin adding this MUST replace the word "BUCKETNAME" their
	///       particular bucket name.
	///       {
	///         "Version":"2008-10-17",
	///         "Statement":[{
	///           "Sid":"AllowPublicRead",
	///               "Effect":"Allow",
	///             "Principal": {
	///                   "AWS": "*"
	///                },
	///             "Action":["s3:GetObject"],
	///             "Resource":["arn:aws:s3:::BUCKETNAME/*"
	///             ]
	///           }
	///         ]
	///       }
	/// </summary>
    [Export(typeof(IFileSaveService))]
	public class S3FileSaveService : IFileSaveService
	{
        public FileStorageType StorageMode { get { return FileStorageType.S3; } }

		public void SaveFiles( List<FileUpload> fileList )
		{
			NameValueCollection appConfig = ConfigurationManager.AppSettings;
			string accessKeyID = appConfig["AWSAccessKey"];
			string secretAccessKey = appConfig["AWSSecretKey"];
			string s3BucketName = appConfig["AWSBucketName"];

			if ( accessKeyID == null || secretAccessKey == null || s3BucketName == null )
			{
				throw new ConfigurationException( "One or more keys (AWSAccessKey, AWSSecretKey, and/or AWSBucketName) missing from web.config." );
			}

			// This is used to delete old items.
			AmazonS3 s3Client = Amazon.AWSClientFactory.CreateAmazonS3Client( accessKeyID, secretAccessKey );
			TransferUtility fileTransferUtility = new TransferUtility( accessKeyID, secretAccessKey );

			foreach ( FileUpload fileUpload in fileList )
			{
				try
				{
					string fileExtension = Path.GetExtension( fileUpload.File.FileName );
					string newFileName = string.Format( "{0}{1}", Guid.NewGuid().ToString(), fileExtension );

					using ( Stream fileToUpload = fileUpload.File.InputStream )
					{
						// We can't do this next line because I want to set the file to public read access
						// using the TransferUtilityUploadRequest.
						//fileTransferUtility.Upload( fileToUpload, s3BucketName, newFileName );
						TransferUtilityUploadRequest fileTransferUtilityRequest = 
							new TransferUtilityUploadRequest()
								.WithBucketName( s3BucketName )
								.WithStorageClass( S3StorageClass.ReducedRedundancy )
								.WithMetadata( "original", fileUpload.File.FileName )
								.WithKey( newFileName )
								.WithCannedACL( S3CannedACL.PublicRead );
						fileTransferUtilityRequest.WithInputStream( fileToUpload );

						fileTransferUtility.Upload( fileTransferUtilityRequest );
					}

					// Success!  Now let's tell the fileUpload what his new public filename will be... 
					fileUpload.NewFileName = string.Format( "https://s3.amazonaws.com/{0}/{1}", s3BucketName, newFileName );
				}
				catch ( AmazonS3Exception s3Exception )
				{
					fileUpload.IsError = true;
					fileUpload.ErrorMessage = "Unable to save file: " + s3Exception.Message;
				}

				// Try to delete the old file
				if (fileUpload.PreviousFileName != null &&
					fileUpload.PreviousFileName.ToLower().Contains( "s3.amazonaws.com" ) )
				{
					try
					{
						DeleteObjectRequest request = new DeleteObjectRequest();
						string previousKey = Path.GetFileName( fileUpload.PreviousFileName );
						request.WithBucketName( s3BucketName ).WithKey( previousKey );
						s3Client.DeleteObject( request );
					}
					catch ( AmazonS3Exception aS3Ex ) // that's ok, it's just a low priority cleanup operation
					{
						Elmah.ErrorSignal.FromCurrentContext().Raise( aS3Ex );
					} 
				
				}
			}
		}
	}
}