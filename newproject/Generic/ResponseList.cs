using System;
using System.Collections.Generic;

namespace StudentGradeTracker.Generic
{
    public class ResponseList<T>
    {
        public List<T> Data { get; set; }
        public double AverageScore { get; set; }
        public string ScoreCategory { get; set; }
        public ResponseError Error { get; set; }
        public bool IsSuccessful { get; set; }
        public string ResponseCode { get; set; }
        public string Description { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}
