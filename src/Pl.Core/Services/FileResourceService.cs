using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Pl.Core.Services
{
    public class FileResourceService : IFileResourceService
    {
        #region Properties And Constructor

        private readonly ISystemSettings _systemSettings;

        public FileResourceService(
            ISystemSettings systemSettings)
        {
            _systemSettings = systemSettings;
        }

        #endregion Properties And Constructor

        public virtual ResourceType GetMediaType(string resourcePath)
        {
            GuardClausesParameter.NullOrEmpty(resourcePath, nameof(resourcePath));
            string fileExtentson = CoreUtility.GetFileExtension(resourcePath, false);
            if (!string.IsNullOrEmpty(fileExtentson))
            {
                if (!string.IsNullOrEmpty(_systemSettings.Upload.ImageAllowedExtensions) && _systemSettings.Upload.ImageAllowedExtensions.Split(',', ';').Contains(fileExtentson, StringComparer.OrdinalIgnoreCase))
                {
                    return ResourceType.Image;
                }

                if (!string.IsNullOrEmpty(_systemSettings.Upload.VideoAllowedExtensions) && _systemSettings.Upload.VideoAllowedExtensions.Split(',', ';').Contains(fileExtentson, StringComparer.OrdinalIgnoreCase))
                {
                    return ResourceType.Movie;
                }

                if (!string.IsNullOrEmpty(_systemSettings.Upload.AudioAllowedExtensions) && _systemSettings.Upload.AudioAllowedExtensions.Split(',', ';').Contains(fileExtentson, StringComparer.OrdinalIgnoreCase))
                {
                    return ResourceType.Audio;
                }

                if (!string.IsNullOrEmpty(_systemSettings.Upload.FileAllowedExtensions) && _systemSettings.Upload.FileAllowedExtensions.Split(',', ';').Contains(fileExtentson, StringComparer.OrdinalIgnoreCase))
                {
                    return ResourceType.None;
                }
            }
            return ResourceType.None;
        }

        public virtual void CreateThumbImage(string thumbPath, string imagePath, string resizeList)
        {
            throw new Exception("Chưa hỗ trợ.");
        }

        public virtual string GetFilePath(ResourceType fileType, string fileName)
        {
            GuardClausesParameter.NullOrEmpty(fileName, nameof(fileName));
            string filePath = string.Empty;
            switch (fileType)
            {
                case ResourceType.Image:
                    filePath = _systemSettings.Upload.UploadImgPath;
                    break;

                case ResourceType.Movie:
                    filePath = _systemSettings.Upload.UploadVideoPath;
                    break;

                case ResourceType.Audio:
                    filePath = _systemSettings.Upload.UploadAudioPath;
                    break;

                case ResourceType.Avatar:
                    filePath = _systemSettings.User.UploadAvatarPath;
                    break;

                case ResourceType.None:
                    break;

                default:
                    filePath = _systemSettings.Upload.UploadFilePath;
                    break;
            }
            return string.IsNullOrEmpty(fileName) ? filePath : Path.Combine(filePath, fileName);
        }
    }
}