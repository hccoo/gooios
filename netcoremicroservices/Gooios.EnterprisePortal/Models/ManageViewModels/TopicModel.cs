using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Models.ManageViewModels
{
    public class TopicModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public TopicCategory? Category { get; set; } = null;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string FaceImageUrl { get; set; }
        
        public IEnumerable<TopicImageModel> ContentImages { get; set; }

        public string Introduction { get; set; }

        public bool IsCustom { get; set; } = false;

        public string CustomTopicUrl { get; set; } = "";

        /// <summary>
        /// 所属的企业或商家Id
        /// </summary>
        public string OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        #region Address
        
        public Address Address { get; set; }

        #endregion

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string LastUpdBy { get; set; }

        public DateTime? LastUpdOn { get; set; } = DateTime.Now;

        public bool IsSuspend { get; set; } = false;

        public string Distance { get; set; }

        public string CreatorName { get; set; }

        public string CreatorPortraitUrl { get; set; }
    }

    public class TopicImageModel
    {
        public int Id { get; set; }

        public string TopicId { get; set; }

        public string ImageId { get; set; }

        public string ImageUrl { get; set; }
    }

    public enum TopicCategory
    {
        EnterprisePage = 1,
        ActivityPage = 2
    }
    

}
