using System.ComponentModel.DataAnnotations;

namespace StudentGradeTracker.DTO.RequestDTO
{
    public class UpdateStudentRequestDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
