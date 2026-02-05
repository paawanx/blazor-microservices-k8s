using BlazorCRUD.StudentApi.Models;

namespace BlazorCRUD.StudentApi.Interfaces;

public interface IStudentService
{
    Task<List<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student> AddStudentAsync(Student student);
    Task<bool> UpdateStudentAsync(Student student);
    Task<bool> DeleteStudentAsync(int id);
}