using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGradeTracker.IRepo
{
    public interface IStudentRepository
    {
        Task<Response<GetStudentResponseDTO>> AddAsync(StudentRegistrationRequestDTO model);
        Task<Response<GetStudentResponseDTO>> GetByIdAsync(Guid studentId);
        Task<ResponseList<GetStudentResponseDTO>> GetAllAsync();
        Task<Response<bool>> DeleteAsync(Guid studentId);
        Task<Response<bool>> UpdateAsync(Guid studentId, UpdateStudentRequestDTO model);

    }
}
