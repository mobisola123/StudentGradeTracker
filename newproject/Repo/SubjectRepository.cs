using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using newproject;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Entities;
using StudentGradeTracker.Generic;
using StudentGradeTracker.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeTracker.Repo
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public SubjectRepository(AppDbContext context, IStudentRepository studentRepository, IMapper mapper)
        {
            _context = context;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<Response<GetSubjectResponseDTO>> AddAsync(Guid studentId, SubjectRegistrationRequestDTO model)
        {
            Response<GetSubjectResponseDTO> response = new();
            try
            {
                //get the student first
                var student = await _studentRepository.GetByIdAsync(studentId);

                if (student.Data == null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "Student does not exixt"
                    };

                }
                //add subject to Db
                Subject subject = new()
                {
                    Code = model.Code,
                    SubjectName = model.SubjectName,

                    StudentId = student.Data.Id
                };

                var addSubject = await _context.Subjects.AddAsync(subject);

                int isSaved = await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<GetSubjectResponseDTO>(addSubject.Entity);

                if (isSaved > 0)
                {
                    //GetSubjectResponseDTO responseDto = new()
                    //{
                    //    SubjectId = addSubject.Entity.SubjectId,
                    //    Code = addSubject.Entity.Code,
                    //    SubjectName = addSubject.Entity.SubjectName
                    //};

                    response.Data = responseDto;
                    response.IsSuccessful = true;
                    response.ResponseCode = "00";
                    response.Description = "successful";
                    return response;
                }
                response.IsSuccessful = false;
                response.Error = new ResponseError()
                {
                    ErrorCode = 21,
                    ErrorMessage = "Something went wrong"
                };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "internal server error"
                };
                response.Description = "please try again later";
                return response;
            }
        }


        public async Task<ResponseList<GetSubjectResponseDTO>> GetAllAsync()
        {
            var response = new ResponseList<GetSubjectResponseDTO>();

            try
            {
                var subjects = await _context.Subjects
                    .ProjectTo<GetSubjectResponseDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

            if (!subjects.Any())
            {
                response.IsSuccessful = false;
                response.Error = new ResponseError()
                {
                    ErrorCode = 21,
                    ErrorMessage = "No record found"
                };
                return response;
            }

            //var SubjectResponseList = new List<GetSubjectResponseDTO>();

            //foreach (var subject in subjects)
            //{
            //    var res = new GetSubjectResponseDTO()
            //    {
            //        SubjectId = subject.SubjectId,
            //        Code = subject.Code,
            //        SubjectName = subject.SubjectName
            //    };

            //    SubjectResponseList.Add(res);
            //}
            response.IsSuccessful = true;
            response.Data = subjects;
            response.ResponseCode = "00";
            response.Description = "successful";
            return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "internal server error"
                };
                return response;
            }
        }

        public async Task<Response<GetSubjectResponseDTO>> GetByIdAsync(Guid id)
        {
            Response<GetSubjectResponseDTO> response = new();
            try
            {
                var subject = await GetSubject(id);

                if (subject is null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 22,
                        ErrorMessage = "SubjectId does not exist"
                    };
                    return response;
                }

                //GetSubjectResponseDTO responseDTO = new();
                //return new Response<GetSubjectResponseDTO>()

                //{
                //    SubjectId = subject.SubjectId,
                //    Code = subject.Code,
                //    SubjectName = subject.SubjectName
                //};
                response.IsSuccessful = true;
                response.Data = _mapper.Map<GetSubjectResponseDTO>(subject);
                response.ResponseCode = "00";
                response.Description = "successful";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "internal server error"
                };
                return response;
            }
        }
        public async Task<Response<bool>> DeleteAsync(Guid id)
        {
            Response<bool> response = new();
            try
            {
                var subject = await GetSubject(id);

                if (subject is null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 23,
                        ErrorMessage = "StudentId does not exist"
                    };
                    return response;
                }

                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();

                response.IsSuccessful = true;
                response.ResponseCode = "00";
                response.Description = "successful";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "internal server error"
                };
                return response;
            }
        }

        private async Task<Subject> GetSubject(Guid id)
        {
            return await _context.Subjects
                .Where(x => x.SubjectId == id)
                .FirstOrDefaultAsync();
        }
    }
}
