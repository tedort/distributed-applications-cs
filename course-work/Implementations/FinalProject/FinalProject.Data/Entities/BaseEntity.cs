using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
