using Gooios.ActivityService.Domains.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Applications.DTOs
{
    public class TopicDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public TopicCategory? Category { get; set; } = null;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string FaceImageUrl { get; set; }
        
        public IEnumerable<TopicImageDTO> ContentImages { get; set; }

        public string Introduction { get; set; }

        public bool IsCustom { get; set; } = false;

        public string CustomTopicUrl { get; set; } = "";

        /// <summary>
        /// 所属的企业或商家Id
        /// </summary>
        public string OrganizationId { get; set; }

        public string ApplicationId { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationLogoUrl { get; set; }

        public string CreatorName { get; set; }

        public string CreatorPortraitUrl { get; set; }

        #region Address

        public Address Address { get; set; }

        #endregion

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string LastUpdBy { get; set; }

        public DateTime? LastUpdOn { get; set; } = DateTime.Now;

        public bool IsSuspend { get; set; } = false;

        public string Distance { get; set; }
    }

    public class TopicImageDTO
    {
        public int Id { get; set; }

        public string TopicId { get; set; }

        public string ImageId { get; set; }

        public string ImageUrl { get; set; }

        public int Order { get; set; }
    }
}
