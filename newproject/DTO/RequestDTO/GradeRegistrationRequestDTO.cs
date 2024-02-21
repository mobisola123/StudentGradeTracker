using System;
using System.ComponentModel.DataAnnotations;

namespace StudentGradeTracker.DTO.RequestDTO
{
    public class GradeRegistrationRequestDTO
    {
        [Required]
        public int Score { get; set; }
        [Required]
        public Guid StudentId { get; set; }
    }
}
