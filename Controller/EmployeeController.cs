using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Ofisprojesi;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace ofisprojesi
{
    [Authorize]
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IEmployeeService _EmployeeService;
        public EmployeeController(OfisProjesiContext context, IMapper mapper, IEmployeeService EmployeeService)
        {
            _context = context;
            _mapper = mapper;
            _EmployeeService = EmployeeService;
        }
        ///<summary>
        /// Çalışan kaydetme
        /// </summary>
        /// <return></return>
        [Route("")]
        [HttpPost]
        public ActionResult<Employee> SaveEmployee([FromBody] EmployeeUpdateDto employee)
        {
            Employee Employee = _EmployeeService.SaveEmployee(employee);
            if (Employee == null)
            {
                return BadRequest("hata var");
            }
            else
            {
                return Ok("kayıt başarılı");
            }
        }
        ///<summary>
        /// id ye göre Çalışan Silme
        /// </summary>
        /// <return></return>
        [Route("id")]
        [HttpDelete]
        public ActionResult<Employee> DeleteByIdEmployee([FromQuery] int id)
        {
            Employee employee = _EmployeeService.DeleteEmployeById(id);
            if (employee == null)
            {
                return BadRequest("kişi bulunamadı");
            }
            else
            {
                return Ok("başarı ile silindi");
            }
        }
        ///<summary>
        /// id ye göre Çalışan Güncelleme
        /// </summary>
        /// <return></return>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateEmployee([FromBody] EmployeeUpdateDto employee, int id,bool durum)
        {
            EmployeeUpdateDto Employee = _EmployeeService.UpdateEmployee(employee, id,durum);
            if (Employee == null)
            {
                return BadRequest("güncellemede sorun");
            }
            else
            {
                return Ok("güncelleme başarılı");
            }
        }
        ///<summary>
        /// Çalışanı id sine Göre Getir
        /// </summary>
        /// <return></return>
        [HttpGet("{id}", Name = "Get-Employee-By-Id")]
        public ActionResult<EmployeeDto> GetEmployeeById(int id)
        {
            EmployeeDto employee = _EmployeeService.GetEmployeeById(id);
            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                return BadRequest("id bulunamadı");
            }
        }
        ///<summary>
        /// isme ve duruma göre çalışan sorgulama
        /// </summary>
        /// <return></return>
        [Route("")]
        [HttpGet]
        public ActionResult<EmployeeDto[]> GetEmployeeByName([FromQuery] string name, bool? durum)
        {
            EmployeeDto[] employee = _EmployeeService.GetEmployeeByName(name, durum);
            if (employee == null)
            {
                return BadRequest("sorgu hatası");
            }
            else
            {
                return Ok(employee);
            }
        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult<EmployeeUpdateDto> UpdatePatch(int id, [FromBody] JsonPatchDocument<Employee> name)
        {
            Employee employee = _EmployeeService.UpdatePatch(id, name);
            if (employee == null)
            {
                return BadRequest("hata var");
            }
            else
            {
                return Ok("başarılı güncelleme");
            }
        }
    }
}









