namespace University.Domain.Models
{
    public class HierarchyObjectPostDto
    {
        public int? Id { get; set; }

        public int? ParentId { get; set; }

        public int? SortIndex { get; set; }
    }
}
