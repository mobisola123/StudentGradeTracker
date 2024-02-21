using System;

namespace StudentGradeTracker.DTO.ResponseDTO
{
    public class GetSubjectResponseDTO
    {
        public Guid SubjectId { get; set; }
        public string Code { get; set; }
        public string SubjectName { get; set; }
        public GetGradeResponseDTO Grade { get; set; }
    }
}
