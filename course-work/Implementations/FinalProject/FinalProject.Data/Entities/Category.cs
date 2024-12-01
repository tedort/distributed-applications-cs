using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

namespace FinalProject.Data.Entities
{
    public class Category : BaseEntity
    {
        [StringLength(20, MinimumLength = 4)]
        [DisplayName("Име")]
        public required string CategoryName { get; set; }
        [DisplayName("Максимален брой вагони")]
        public int? WagoonsCount { get; set; }
        public virtual ICollection<Train>? Trains { get; set; }
    }
}
