using System.ComponentModel.DataAnnotations;

namespace StudentGradeTracker.DTO.RequestDTO
{
    public class SubjectRegistrationRequestDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string SubjectName { get; set; }
    }
}
