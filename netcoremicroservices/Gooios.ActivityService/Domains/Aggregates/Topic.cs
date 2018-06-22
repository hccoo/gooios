using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Domains.Aggregates
{
    public class Topic : Entity<string>
    {
        public string Title { get; set; }

        public TopicCategory? Category { get; set; } = null;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string FaceImageUrl { get; set; }

        [NotMapped]
        public IEnumerable<TopicImage> ContentImages { get; set; }

        public string Introduction { get; set; }

        public bool IsCustom { get; set; } = false;

        public string CustomTopicUrl { get; set; } = "";

        /// <summary>
        /// 所属的企业或商家Id
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// 后期用于支持个人发布
        /// </summary>
        public string CreatorName { get; set; }

        public string CreatorPortraitUrl { get; set; }

        #region Address

        [NotMapped]
        public Address Address { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string StreetAddress { get; set; }

        public string Postcode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        #endregion

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string LastUpdBy { get; set; }

        public DateTime? LastUpdOn { get; set; } = DateTime.Now;

        public bool IsSuspend { get; set; } = false;

        public void InitAddress()
        {
            Area = Address.Area;
            City = Address.City;
            Postcode = Address.Postcode;
            Province = Address.Province;
            StreetAddress = Address.StreetAddress;
            Latitude = Address.Latitude;
            Longitude = Address.Longitude;
        }

        public void ResolveAddress()
        {
            if (Address == null) Address = new Address(Province, City, Area, StreetAddress, Postcode, Latitude, Longitude);
        }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void SetSuspend()
        {
            IsSuspend = true;
        }
    }

    public enum TopicCategory
    {
        EnterprisePage = 1,
        ActivityPage = 2
    }

    public class TopicImage : Entity<int>
    {
        public string TopicId { get; set; }

        public string ImageId { get; set; }

        public string ImageUrl { get; set; }

        public int Order { get; set; }
    }

    public class Address : ValueObject<Address>
    {
        public Address() { }

        public Address(string province, string city, string area, string streetAddress, string postcode, double latitude, double longitude)
        {
            Province = province;
            City = city;
            Area = area;
            StreetAddress = streetAddress;
            Postcode = postcode;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string StreetAddress { get; set; }

        public string Postcode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class TopicFactory
    {
        public static Topic CreateInstance(
            string title,
            string faceImageUrl,
            string introduction,
            bool isCustom,
            string customTopicUrl,
            string organizationId,
            string province,
            string city,
            string area,
            string streetAddress,
            string postCode,
            double latitude,
            double longitude,
            string createdBy,
            string creatorName,
            string creatorPortraitUrl,
            DateTime? startDate = null,
            DateTime? endDate = null,
            TopicCategory? category = null
            )
        {
            var result = new Topic
            {
                Area = area,
                Category = category,
                City = city,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                CustomTopicUrl = customTopicUrl,
                EndDate = endDate,
                FaceImageUrl = faceImageUrl,
                Introduction = introduction,
                IsCustom = isCustom,
                IsSuspend = false,
                LastUpdBy = createdBy,
                LastUpdOn = DateTime.Now,
                Latitude = latitude,
                Longitude = longitude,
                OrganizationId = organizationId,
                Postcode = postCode,
                Province = province,
                StartDate = startDate,
                StreetAddress = streetAddress,
                Title = title,
                CreatorName = creatorName,
                CreatorPortraitUrl = creatorPortraitUrl
            };

            result.GenerateId();
            result.ResolveAddress();

            return result;
        }
    }
}
