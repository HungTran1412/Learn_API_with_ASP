using BaiTest.DTOs;
using BaiTest.Models;
using BaiTest.Services;
using Microsoft.AspNetCore.Mvc;


namespace BaiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        public StudentController()
        {
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll() => StudentService.GetAll;

        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            var student = StudentService.Get(id);
            if(student == null) 
                return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> Add([FromBody] StudentRequest s)
        {
            if (s == null)
                return BadRequest("Thông tin không hợp lệ!");
            var newStudent =  StudentService.Add(s);

            return Ok(newStudent);
        }
        
        [HttpPut("{id}")]
        public ActionResult<Student> Update(int id,[FromBody] StudentRequest s)
        {
            if (s == null)
            {
                return BadRequest("Thông tin không hợp lệ!");
            }

            var updatedStudent = StudentService.Update(id, s);

            if (updatedStudent == null)
            {
                return NotFound($"Không tìm thấy sinh viên có id: {id}");
            }

            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var s = StudentService.Get(id);
            if(s == null)
                return NotFound();
            StudentService.Remove(id);
            return Ok("Xóa sinnh viên thành công!");
        }
    }
}
