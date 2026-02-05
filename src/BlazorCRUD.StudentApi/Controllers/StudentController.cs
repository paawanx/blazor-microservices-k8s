using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlazorCRUD.StudentApi.Interfaces;
using BlazorCRUD.StudentApi.Models;

namespace BlazorCRUD.StudentApi.Controllers;

[ApiController]
[Route("api/students")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();
        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> AddStudent(Student student)
    {
        // validate model
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdStudent = await _studentService.AddStudentAsync(student);
        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, Student student)
    {
        if (id != student.Id)
            return BadRequest("ID in URL must match ID in body");

        // validate model
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _studentService.UpdateStudentAsync(student);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var success = await _studentService.DeleteStudentAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    } 
}
