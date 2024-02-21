using StudentGradeTracker.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentGradeTracker.DTO.RequestDTO
{
    public class AddRoleToUserRequestDto
    {
        [Required]
        public string IdentityId { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
