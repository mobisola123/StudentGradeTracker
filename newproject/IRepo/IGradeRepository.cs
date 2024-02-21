using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGradeTracker.IRepo
{
    public interface IGradeRepository
    {
        Task<Response<GetGradeResponseDTO>> AddAsync(Guid SubjectId, GradeRegistrationRequestDTO model);
        Task<ResponseList<GetGradeResponseDTO>> GetGrades(Guid Id);
        Task<ResponseList<GetAverageGradeResponseDTO>> GetAverageGrade(Guid Id);
    }
}