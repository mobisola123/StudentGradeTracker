using System;

namespace StudentGradeTracker.Entities
{
    public class Subject
    {
        public Guid SubjectId { get; set; }
        public string Code { get; set; }
        public string SubjectName { get; set; }

        //relationship with student
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        //relationship with grade
        public Grade? Grade { get; set; }
    }
}
