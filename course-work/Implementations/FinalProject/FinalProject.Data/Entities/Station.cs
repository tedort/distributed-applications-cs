using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

namespace FinalProject.Data.Entities
{
    public class Station : BaseEntity
    {
        [DisplayName("Име")]
        [StringLength(150, MinimumLength = 3)]
        public required string Name { get; set; }

        [DisplayName("Брой коловози")]
        public required int TracksCount { get; set; }

        [DisplayName("Адрес")]
        [StringLength(300, MinimumLength = 10)]
        public string? Address { get; set; }

        [DisplayName("Брой перони")]
        public int? PlatfromsCount { get; set; }

        [DisplayName("Има ли каса")]
        public bool HasCashDesk { get; set; }
        public virtual ICollection<Train>? Trains { get; set; }
    }
}
