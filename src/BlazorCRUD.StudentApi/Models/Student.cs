using System.ComponentModel.DataAnnotations;

namespace BlazorCRUD.StudentApi.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Range(18, 35)]
        public int Age { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}