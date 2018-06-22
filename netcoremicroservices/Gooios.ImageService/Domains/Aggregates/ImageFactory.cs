using System;

namespace Gooios.ImageService.Domains.Aggregates
{
    public static class ImageFactory
    {
        public static Image CreateImage(string title, string description, string httpPath, string createdBy)
        {
            var result = new Image();
            result.GenerateId();

            var now = DateTime.Now;

            result.CreatedBy = createdBy;
            result.CreatedOn = now;
            result.Description = description;
            result.HttpPath = httpPath;
            result.Title = title;

            return result;
        }
    }
}
