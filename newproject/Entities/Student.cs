using System;
using System.Collections.Generic;

namespace StudentGradeTracker.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string IdentityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        //relationship with Subject
        public ICollection<Subject> Subjects { get; set; }

    }
}
