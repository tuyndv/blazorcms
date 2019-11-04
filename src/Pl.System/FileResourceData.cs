using Microsoft.EntityFrameworkCore;
using Pl.Core;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pl.System
{
    public class FileResourceData : EfRepository<FileResource>, IFileResourceData
    {
        private readonly ISystemSettings _systemSettings;

        public FileResourceData(ISystemSettings systemSettings, SystemDbContext systemDbContext) : base(systemDbContext)
        {
            _systemSettings = systemSettings;
        }

        /// <summary>
        /// Lấy danh sách thư viên tệp tin theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="description">Mô tả cho file</param>
        /// <param name="type">Loại resource</param>
        /// <param name="userId">Người tạo tệp tin</param>
        /// <param name="startDate">Bắt đầu ngày tạo</param>
        /// <param name="endDate">Kết thúc ngày tạo</param>
        /// <returns>IDataSourceResult Media</returns>
        public async Task<IDataSourceResult<FileResource>> GetFileResourcesAsync(
            int skip,
            int take,
            string description = "",
            ResourceType? type = null,
            string userId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            BaseSpecification<FileResource> resourceSpecification = new BaseSpecification<FileResource>(q =>
            (string.IsNullOrEmpty(description) || EF.Functions.Contains(q.Description, description))
            && (string.IsNullOrEmpty(userId) || q.UserId == userId)
            && (!startDate.HasValue || q.CreatedTime >= startDate)
            && (!endDate.HasValue || q.CreatedTime <= endDate)
            && (!type.HasValue || q.Type == type));
            resourceSpecification.ApplyOrderByDescending(q => q.Id);
            resourceSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(resourceSpecification);
        }

        /// <summary>
        /// Delete FileResource file
        /// </summary>
        /// <param name="fileResource">FileResource to delete</param>
        public async Task DeleteFileAsync(FileResource fileResource)
        {
            GuardClausesParameter.Null(fileResource, nameof(fileResource));

            if (fileResource.Type == ResourceType.Image)
            {
                await DeleteImageFileAsync(fileResource.Path);
            }

            if (fileResource.Type == ResourceType.Movie)
            {
                await DeleteFileAsync(fileResource.Path);
            }

            if (fileResource.Type == ResourceType.Audio)
            {
                await DeleteFileAsync(fileResource.Path);
            }

            if (fileResource.Type == ResourceType.None)
            {
                await DeleteFileAsync(fileResource.Path);
            }
        }

        /// <summary>
        /// Hàm xóa ảnh được upload lên
        /// </summary>
        /// <param name="imageFileName">Tên file ảnh cần xóa</param>
        public async Task DeleteImageFileAsync(string imageFileName)
        {
            if (!string.IsNullOrWhiteSpace(imageFileName))
            {
                List<string> listDeletePath = new List<string>();
                string uploadPath = _systemSettings.Upload.UploadImgPath;

                if (!string.IsNullOrEmpty(_systemSettings.Upload.ImgResizeList))
                {
                    foreach (string item in _systemSettings.Upload.ImgResizeList.Split(','))
                    {
                        string[] imgSize = item.Split('*');
                        if (imgSize.Length <= 1)
                        {
                            listDeletePath.Add(uploadPath + "\\" + imgSize[0] + "." + imageFileName.Replace('/', '\\'));
                        }
                        else
                        {
                            listDeletePath.Add(uploadPath + "\\" + imgSize[0] + "." + imgSize[1] + "." + imageFileName.Replace('/', '\\'));
                        }
                    }
                }

                listDeletePath.Add(uploadPath + "\\" + imageFileName.Replace('/', '\\'));

                foreach (string item in listDeletePath)
                {
                    await Task.Run(() =>
                    {
                        CoreUtility.DeleteFile(item);
                    });
                }
            }
        }

        /// <summary>
        /// Hàm xóa video được upload lên
        /// </summary>
        /// <param name="fileName">Tên file cần xóa</param>
        public async Task DeleteFileAsync(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                await Task.Run(() =>
                {
                    CoreUtility.DeleteFile(_systemSettings.Upload.UploadFilePath + "\\" + fileName.Replace('/', '\\'));
                });
            }
        }

    }
}