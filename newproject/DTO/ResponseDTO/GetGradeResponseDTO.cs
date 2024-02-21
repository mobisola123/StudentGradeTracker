using StudentGradeTracker.Entities;
using System;

namespace StudentGradeTracker.DTO.ResponseDTO
{
    public class GetGradeResponseDTO
    {

        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        
        public int Score { get; set; }
        public string Category { get; set; }
    }
}
