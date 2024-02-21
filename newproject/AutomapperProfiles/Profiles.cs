using AutoMapper;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Entities;

namespace StudentGradeTracker.AutomapperProfiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Student, GetStudentResponseDTO>().ReverseMap();

            CreateMap<Subject, GetSubjectResponseDTO>().ReverseMap();

            CreateMap<Grade, GetGradeResponseDTO>().ReverseMap();
        }
    }
}
