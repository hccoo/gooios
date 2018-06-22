using Gooios.ImageService.Applications.DTO;
using Aggregates = Gooios.ImageService.Domains.Aggregates;
using Gooios.ImageService.Domains.Repositories;
using Gooios.Infrastructure;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Gooios.Infrastructure.Exceptions;
using System.Collections.Generic;

namespace Gooios.ImageService.Applications.Services
{
    public interface IImageAppService : IApplicationServiceContract
    {
        ImageDTO AddImage(ImageDTO imageDTO);

        ImageDTO GetImage(string id);

        IEnumerable<ImageDTO> GetImages(IEnumerable<string> ids);
    }

    class ImageAppService : ApplicationServiceContract, IImageAppService
    {
        readonly IImageRepository _imageRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;
        readonly IHostingEnvironment _hostingEnvironment;

        public ImageAppService(IImageRepository imageRepository, IDbUnitOfWork dbUnitOfWork, IHostingEnvironment hostingEnvironment)
        {
            _imageRepository = imageRepository;
            _dbUnitOfWork = dbUnitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public ImageDTO AddImage(ImageDTO imageDTO)
        {
            byte[] imageBytes = Convert.FromBase64String(imageDTO.ImageBase64Content);

            if (imageBytes.Length / 1024 > 2048) throw new ArgumentInvalidException("图片大小不能超过2MB.");

            imageDTO.HttpPath = Base64StringToImage(imageBytes, imageDTO.HttpPath);

            var image = Aggregates.ImageFactory.CreateImage(imageDTO.Title, imageDTO.Description, imageDTO.HttpPath, imageDTO.CreatedBy);
            _imageRepository.Add(image);

            _dbUnitOfWork.Commit();

            imageDTO.Id = image.Id;
            imageDTO.ImageBase64Content = string.Empty;
            imageDTO.CreatedOn = image.CreatedOn;
            return imageDTO;
        }

        public ImageDTO GetImage(string id)
        {
            var image = _imageRepository.Get(id);
            return image == null ? null : new ImageDTO
            {
                Id = image.Id,
                CreatedBy = image.CreatedBy,
                CreatedOn = image.CreatedOn,
                Description = image.Description,
                HttpPath = image.HttpPath,
                Title = image.Title
            };
        }

        public IEnumerable<ImageDTO> GetImages(IEnumerable<string> ids)
        {
            var results = new List<ImageDTO>();
            foreach(var id in ids)
            {
                var image = _imageRepository.Get(id);
                if (image != null)
                {
                    results.Add(new ImageDTO
                    {
                        Id = image.Id,
                        CreatedBy = image.CreatedBy,
                        CreatedOn = image.CreatedOn,
                        Description = image.Description,
                        HttpPath = image.HttpPath,
                        Title = image.Title
                    });
                }
            }
            return results;
        }

        string Base64StringToImage(byte[] imageBytes, string filePath)
        {
            MemoryStream ms = new MemoryStream(imageBytes);
            var imageName = $"{Guid.NewGuid().ToString()}.jpg";
            var path = $"./uploadimages/{imageName}";
            var webPath = $"{filePath}{imageName}";
            using (FileStream fs = System.IO.File.Create(path))
            {
                ms.CopyTo(fs);
                fs.Flush();
            }
            ms.Close();
            return webPath;
        }
    }
}
