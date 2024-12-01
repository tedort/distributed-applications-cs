using System.ComponentModel;
using System.Diagnostics;

namespace FinalProject.Data.Entities
{
    public class Timetable : BaseEntity
    {
        public int TrainId { get; set; }

        [DisplayName("Влак")]
        public virtual Train? Train { get; set; }

        public int StationId { get; set; }

        [DisplayName("Гара")]
        public virtual Station? Station { get; set; }

        [DisplayName("Пристига")]
        public DateTime? ArrivalTime { get; set; } // Made optional

        [DisplayName("Заминава")]
        public DateTime? DepartureTime { get; set; } // Made optional

        [DisplayName("Коловоз")]
        public int PlatformNumber { get; set; }

        [DisplayName("Начална гара")]
        public bool IsStartStation { get; set; } = false; // New flag

        [DisplayName("Крайна гара")]
        public bool IsFinalStation { get; set; } = false; // New flag
    }
}
