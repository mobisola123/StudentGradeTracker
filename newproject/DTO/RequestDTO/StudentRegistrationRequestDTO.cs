using System.ComponentModel.DataAnnotations;

namespace StudentGradeTracker.DTO.RequestDTO
{
    public class StudentRegistrationRequestDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
