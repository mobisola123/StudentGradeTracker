using System;

namespace StudentGradeTracker.DTO.ResponseDTO
{
    public class GetAverageGradeResponseDTO
    {
        public double AverageScore { get; set; }
        public string ScoreCategory { get; set; }
    }
}
