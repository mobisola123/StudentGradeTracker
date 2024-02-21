using System;
using System.Collections.Generic;

namespace StudentGradeTracker.Entities
{
    public class Grade
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public string Category { get; set; }
        public Guid? StudentId { get; set; }

        //relationship with subject
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}