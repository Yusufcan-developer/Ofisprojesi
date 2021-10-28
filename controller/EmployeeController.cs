using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Newtonsoft.Json;
using AutoMapper;

namespace ofisprojesi
{


    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public EmployeeController(OfisProjesiContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        ///<summary>
        /// Çalışan kaydetme
        /// </summary>
        /// <return></return>
        [Route("")]
        [HttpPost]
        public ActionResult SaveEmployee([FromBody] EmployeeDto employee)
        {
            Office officecontroller = _context.Offices.Where(p => p.Id == employee.OfficeId).SingleOrDefault();
            Employee employee1=new Employee ();
            employee1.Name=employee.Name;
            employee1.OfficeId=employee.OfficeId;
            employee1.Lastname=employee.Lastname;
            employee1.Age=employee.Age;
            employee1.RecordDate=DateTime.Now;
            employee1.UpdateDate=DateTime.Now;
            employee1.Status=employee.Status;



            if (employee.Status == false || employee.Status == null)
            {
                return BadRequest("çalışanın ilk kayıt sırasında durumunu sadece true yapabilirsiniz");
            }
            else if (employee.Age > 150 || employee.Age < 5)
            {
                return BadRequest("kayıt başarısız çalışan ölmüş lütfen en yakın mezar sağlayıcısına başvurun");
            }
            else if (String.IsNullOrWhiteSpace(employee.Name) || String.IsNullOrWhiteSpace(employee.Lastname))
            {
                return BadRequest("isim veya soyisim boş bırakılamaz");
            }
            else if (officecontroller == null)
            {
                return BadRequest("kayıtlı office bulunamadı");
            }
            else if (employee.Id!=null)
            {
                return BadRequest("id hatası");
            }
            else
            {
                
                _context.Employees.Add(employee1);
                _context.SaveChanges();
                EmployeeDto dto =_mapper.Map<EmployeeDto>(employee);
                return Ok(dto);
            }
        }
        ///<summary>
        /// Çalışan Silme
        /// </summary>
        /// <return></return>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteByIdEmployee([FromQuery] int id)
        {
            Employee Deleted = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            Debit debitcontrol= _context.Debits.FirstOrDefault(p=>p.EmployeeId==id);
            if (Deleted == null)
            {
                return BadRequest("silinmeye çalışılan kayıt bulunamadı");
            }
            else
            {
                _context.Employees.Remove(Deleted);
                _context.SaveChanges();
                return Ok("kayıt silme başarılı");
            }


        }
        ///<summary>
        /// Çalışan Güncelleme
        /// </summary>
        /// <return></return>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateEmployee([FromBody] EmployeeDto employee, int id)
        {
            Employee update = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            Office officecontrol = _context.Offices.Where(p => p.Id == employee.OfficeId).FirstOrDefault();
            if (employee.Age == 0 || employee.Age == null)
            {
                return BadRequest("yaşınız 0 olamaz");
            }
            if (employee.Status == false && officecontrol.Id != employee.OfficeId)
            {
                return BadRequest("olmayan bir çalışanı bir ofise kayıt ettiremezsiniz");
            }
            else if(employee.Id!=null){
               return BadRequest("id hatası");
            }
            else
            {

                update.Name = employee.Name;
                update.Lastname = employee.Lastname;
                update.Status = employee.Status;
                update.OfficeId = employee.OfficeId;
                update.Age = employee.Age;
                update.RecordDate = update.RecordDate;
                update.UpdateDate = DateTime.Now;
                _context.SaveChanges();
                EmployeeDto dto =_mapper.Map<EmployeeDto>(employee);
                return Ok(dto);
            }
        }
        ///<summary>
        /// Çalışanı id sine Göre Getir
        /// </summary>
        /// <return></return>
        [HttpGet("{id}", Name = "Get-Employee-By-Id")]
        public ActionResult GetEmployeeById(int id)
        {
            Employee employees =_context.Employees.Where(p => p.Id == id).FirstOrDefault();
            EmployeeDto employe=_mapper.Map<EmployeeDto>(employees);
            return Ok(employe);
        }
        ///<summary>
        /// Tüm Çalışanları Getir
        /// </summary>
        /// <return></return>
        [Route("name")]
        [HttpGet]
        public ActionResult GetEmployeeByName([FromQuery] string name, bool? durum)
        {
            List<Employee> searchEmploye = _context.Employees.ToList();
            

            if (!string.IsNullOrWhiteSpace(name))
            {
                searchEmploye = searchEmploye.Where(p => p.Name.Contains(name)).ToList();
            }
            if (durum.HasValue)
            {
                searchEmploye = searchEmploye.Where(p => p.Status == durum).ToList();
            }
            if (searchEmploye.ToList().Count > 0)
            {
                List<EmployeeDto> employeeDtos=_mapper.Map<List<EmployeeDto>>(searchEmploye);
                return Ok(employeeDtos);
            }
            else
            {

                return BadRequest("kayıt bulunamadı");
            }
        }
    }
}









