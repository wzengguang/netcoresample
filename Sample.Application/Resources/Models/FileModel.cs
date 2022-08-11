using Sample.Domain.Resources;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Resources.Models
{
    public class MediaModel
    {
        private static readonly HashSet<string> legalVideoExtension = new HashSet<string>() { ".avi", ".wmv", ".mpeg", ".mp4", ".m4v", ".mov", ".asf", ".flv", ".f4v", ".rmvb", ".rm", ".3gp", ".vob" };

        private static readonly HashSet<string> legalPictureExtension = new HashSet<string>() { ".bmp", ".jpg", ".png", ".tif", ".gif", ".pcx", ".tga", ".exif", ".fpx", ".svg", ".psd", ".cdr", ".pcd", ".dxf", ".ufo", ".eps", ".ai", ".raw", ".WMF", ".webp", ".avif", ".apng", ".webp", ".jpeg" };

        public string UploadFileName { get; set; }

        public string ServerFileName { get; set; }

        /// <summary>
        /// without extension
        /// </summary>
        public string ServerFilePath { get; set; }

        public string ServerFullFilePath { get; set; }

        public MediaType MediaType { get; private set; }

        public string extension { get; private set; }

        public bool IsLegal { get; private set; }

        public string MD5 { get; set; }

        public IFormFile FormFile { get; private set; }

        public MediaModel(IFormFile formFile)
        {
            this.FormFile = formFile;
            this.UploadFileName = formFile.FileName;

            Init();
        }

        public MediaModel(string clientFileName)
        {
            this.UploadFileName = clientFileName;

            Init();
        }

        private void Init()
        {
            extension = !string.IsNullOrEmpty(this.UploadFileName) ? Path.GetExtension(UploadFileName) : Path.GetExtension(ServerFileName);

            if (MediaModel.legalPictureExtension.Contains(extension))
            {
                IsLegal = true;
                MediaType = MediaType.picture;
            }
            else if (MediaModel.legalVideoExtension.Contains(extension))
            {
                IsLegal = true;
                MediaType = MediaType.video;
            }
            else
            {
                MediaType = MediaType.illegal;
            }
        }

        public async Task<string> CreateMD5(IFormFile formFile = null)
        {
            if (MD5 != null)
            {
                return MD5;
            }

            if (this.FormFile == null && formFile != null)
            {
                this.FormFile = formFile;
            }

            using (var strema = new MemoryStream())
            {
                await FormFile.CopyToAsync(strema);
                using (var Md5 = System.Security.Cryptography.MD5.Create())
                {
                    strema.Position = 0;
                    var hash = Md5.ComputeHash(strema);
                    MD5 = BitConverter.ToString(hash);
                }
            }
            return MD5;
        }
    }
}
