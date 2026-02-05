using BlazorCRUD.StudentApi.Models;
using BlazorCRUD.StudentApi.Interfaces;
using BlazorCRUD.StudentApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUD.StudentApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _context;

        public StudentService(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            return student; // will be null if not found, which is fine for our purposes
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.Id);
            if (existingStudent == null)
                return false;

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;
            existingStudent.BirthDate = student.BirthDate;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
