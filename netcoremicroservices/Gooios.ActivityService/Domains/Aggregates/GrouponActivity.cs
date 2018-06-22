using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.ActivityService.Domains.Aggregates
{
    public class GrouponActivity : Entity<string>
    {
        public string ProductId { get; set; }

        /// <summary>
        /// Goods or Service ...
        /// </summary>
        public string ProductMark { get; set; }

        public int Count { get; set; }

        public decimal UnitPrice { get; set; }

        public ActivityStatus Status { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime LastUpdOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        //[NotMapped]
        //public IEnumerable<GrouponParticipation> GrouponParticipations { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void InitStatus()
        {
            Status = ActivityStatus.InProgress;
            LastUpdOn = DateTime.Now;
        }

        public void SetCompleted()
        {
            Status = ActivityStatus.Completed;
            LastUpdOn = DateTime.Now;
        }

        public void SetFailed()
        {
            Status = ActivityStatus.Failed;
            LastUpdOn = DateTime.Now;
        }

        public bool IsInProcess()
        {
            return Status == ActivityStatus.InProgress;
        }
    }

    public enum ActivityStatus
    {
        InProgress = 1,
        Completed = 2,
        Failed = 4
    }
}
