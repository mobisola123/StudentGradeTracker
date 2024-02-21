using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
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
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace StudentGradeTracker.Repo
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository;
        public GradeRepository(AppDbContext context, ISubjectRepository subjectRepository, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _subjectRepository = subjectRepository;
        }
        private string CalculateGrade(double score)
        {
            if (score >= 70 && score <= 100)
            {
                return "A";
            }
            else if (score >= 60 && score <= 69)
            {
                return "B";
            }
            else if (score >= 50 && score <= 59)
            {
                return "C";
            }
            else if (score >= 45 && score <= 49)
            {
                return "D";
            }
            else if (score >= 40 && score <= 44)
            {
                return "E";
            }
            else if (score >= 0 && score <= 39)
            {
                return "F";
            }
            else
            {
                return "G";
            }
        }

        public async Task<Response<GetGradeResponseDTO>> AddAsync(Guid SubjectId, GradeRegistrationRequestDTO model)
        {
            Response<GetGradeResponseDTO> response = new();
            //check the subject first
            try
            {
                var subject = await _context.Subjects
                .Where(x => x.SubjectId == SubjectId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                if (subject == null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 20,
                        ErrorMessage = "subjectId not exist"
                    };
                    return response;
                }

                var existingGrade = await _context.Grades
                    .Where(x => x.SubjectId == SubjectId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (existingGrade != null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 19,
                        ErrorMessage = "Grade already exist"
                    };
                    return response;
                }

                string gradeCategory = CalculateGrade(model.Score);

                if (gradeCategory == "G")
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 00,
                        ErrorMessage = "grade has exceeded"

                    };
                    return response;
                }

                Grade grade = new()
                {
                    StudentId = model.StudentId,
                    Score = model.Score,
                    Category = gradeCategory,

                    SubjectId = SubjectId
                };
                var addGrade = await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();

                //GetGradeResponseDTO responseDTO = new()
                //{
                //    Id = addGrade.Entity.Id,
                //    StudentId = model.StudentId,
                //    Category = addGrade.Entity.Category,
                //    Score = addGrade.Entity.Score
                //};

                response.IsSuccessful = true;
                response.Data = _mapper.Map<GetGradeResponseDTO>(grade);
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

        public async Task<ResponseList<GetGradeResponseDTO>> GetGrades(Guid Id)
        {
            var response = new ResponseList<GetGradeResponseDTO>();
            try
            {
                var Grade = await _context.Grades.ToListAsync();

                if (!Grade.Any())
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "No grade record"
                    };
                    return response;
                }

                //var GradeResponseList = new List<GetGradeResponseDTO>();

                //var res = await (from g in _context.Grades where g.StudentId == Id
                //select new GetGradeResponseDTO
                //                 {
                //                     Category = g.Category,
                //                     Score = g.Score,
                //                     Id = g.Id,
                //                     StudentId = g.StudentId.Value
                //                 }).ToListAsync();

                response.Data = _mapper.Map<List<GetGradeResponseDTO>>(Grade);
                response.IsSuccessful = true;
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
                response.Description = "Please try again later";
                return response;
            }
        }

        public async Task<ResponseList<GetAverageGradeResponseDTO>> GetAverageGrade(Guid Id)
        {
            var response = new ResponseList<GetAverageGradeResponseDTO>();
            try
            {
                var existingGrade = await (from g in _context.Grades
                                           where g.StudentId == Id
                                           select new GetGradeResponseDTO
                                           {
                                               Category = g.Category,
                                               Score = g.Score,
                                               Id = g.Id,
                                               StudentId = g.StudentId.Value
                                           }).ToListAsync();

                if (existingGrade.Any())
                {
                    //var GradeResponseList = new List<GetAverageGradeResponseDTO>();
                    var avgScore = existingGrade.Average(x => x.Score);
                    string gradeCategory = CalculateGrade(avgScore);
                    response.AverageScore = avgScore;
                    response.ScoreCategory = gradeCategory;
                    return response;
                }

                return response;
                //throw new Exception();
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
    }
}