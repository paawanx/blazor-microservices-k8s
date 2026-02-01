using BlazorCRUD.Models;

namespace BlazorCRUD.Services
{
    public class StudentService : IStudentService
    {
        private readonly List<Student> _students = new();
        private int _nextId = 1;

        public StudentService()
        {
            // Seed with some sample data
            _students.Add(new Student { Id = _nextId++, Name = "John Doe", Age = 20, BirthDate = new DateTime(2006, 3, 15) });
            _students.Add(new Student { Id = _nextId++, Name = "Jane Smith", Age = 22, BirthDate = new DateTime(2004, 7, 22) });
            _students.Add(new Student { Id = _nextId++, Name = "Bob Johnson", Age = 19, BirthDate = new DateTime(2007, 11, 5) });
        }

        public Task<List<Student>> GetAllStudentsAsync()
        {
            return Task.FromResult(_students.ToList());
        }

        public Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(student);
        }

        public Task<Student> AddStudentAsync(Student student)
        {
            student.Id = _nextId++;
            _students.Add(student);
            return Task.FromResult(student);
        }

        public Task<bool> UpdateStudentAsync(Student student)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent == null)
                return Task.FromResult(false);

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;
            existingStudent.BirthDate = student.BirthDate;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteStudentAsync(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return Task.FromResult(false);

            _students.Remove(student);
            return Task.FromResult(true);
        }
    }
}
