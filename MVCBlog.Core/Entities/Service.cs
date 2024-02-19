using System;
using System.Collections.Generic;
using System.Linq;
using MVCBlog.Core.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
//using System.ComponentModel.DescriptionAttribute;

namespace MVCBlog.Core.Entities
{
    public class Service : EntityBase
    {
        [StringLength(150)]
        [Required]
        [Display(Name = nameof(Labels.Description), ResourceType = typeof(Labels))]
        public string Description { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Amount), ResourceType = typeof(Labels))]
        public decimal Amount { get; set; }

        public string CoachId { get; set; }
        public AspNetUser Coach { get; set; }

        [Required]
        public Guid ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        [StringLength(50)]
        [Display(Name = nameof(Labels.SchedulerDay), ResourceType = typeof(Labels))]
        public string SchedulerDay { get; set; }

        [StringLength(20)]
        [Display(Name = nameof(Labels.SchedulerTime), ResourceType = typeof(Labels))]
        public string SchedulerTime { get; set; }

        [Display(Name = nameof(Labels.NameLabel), ResourceType = typeof(Labels))]
        public string Titulo
        {
            get { return String.Format("{0} - {1} {2}", this.ServiceType != null ? this.ServiceType.Name : string.Empty, this.Description,this.Coach != null ? " - " + this.Coach.FullName : string.Empty); }
        }

        [NotMapped]
        public List<AspNetUser> ServiceUsers { get; set; }

        [NotMapped]
        public SchedulerDay Scheduler { get; set; }

    }

    public enum SchedulerDay
    {
        Lunes,
        Martes,
        Miercoles,
        Jueves,
        Viernes,
        Sabado
    }


}
