using System.ComponentModel;

namespace FinalProject.Data.Entities
{
    public class Train : BaseEntity
    {
        [DisplayName("Номер на влак")]
        public required string TrainNumber { get; set; }

        public int? CategoryId { get; set; }

        [DisplayName("Категория")]

        public virtual Category? Category { get; set; }

        public virtual ICollection<Timetable>? Timetables { get; set; }
    }
}
