using StudentGradeTracker.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentGradeTracker.DTO.RequestDTO
{
    public class AddRoleRequestDto
    {
        [Required]
        public Role Role { get; set; }
    }
}
