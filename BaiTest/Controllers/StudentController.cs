using BaiTest.DTOs;
using BaiTest.Models;
using BaiTest.Services;
using BaiTest.Services.Impl;
using Microsoft.AspNetCore.Mvc;


namespace BaiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        IStudentService studentService;

        public StudentController()
        {
            studentService = new StudentServiceImpl();
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll() => studentService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            var student = studentService.Get(id);
            if(student == null) 
                return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> Add([FromBody] StudentRequest s)
        {
            if (s == null)
                return BadRequest("Thông tin không hợp lệ!");
            var newStudent =  studentService.Add(s);

            return Ok(newStudent);
        }
        
        [HttpPut("{id}")]
        public ActionResult<Student> Update(int id,[FromBody] StudentRequest s)
        {
            if (s == null)
            {
                return BadRequest("Thông tin không hợp lệ!");
            }

            var updatedStudent = studentService.Update(id, s);

            if (updatedStudent == null)
            {
                return NotFound($"Không tìm thấy sinh viên có id: {id}");
            }

            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var s = studentService.Get(id);
            if(s == null)
                return NotFound();
            studentService.Delete(id);
            return Ok("Xóa sinnh viên thành công!");
        }
    }
}
