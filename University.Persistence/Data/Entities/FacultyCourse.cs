using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("FacultyCourse", Schema = "university")]
public class FacultyCourse
{
    public int FacultyId { get; set; }
    public int CourseId { get; set; }
    public Faculty Faculty { get; set; }
    public Course Course { get; set; }
}