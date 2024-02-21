using System;
using System.Collections.Generic;

namespace StudentGradeTracker.DTO.ResponseDTO
{
    public class GetStudentResponseDTO
    {
        public Guid Id { get; set; }
        public string IdentityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public List<GetSubjectResponseDTO> Subjects { get; set; }
    }
}
