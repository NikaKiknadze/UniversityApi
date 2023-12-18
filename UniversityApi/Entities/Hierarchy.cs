using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApi.Entities
{
    [Table("Hierarchy", Schema = "university")]
    public class Hierarchy
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int SortIndex { get; set; }

    }
}
