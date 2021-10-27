using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Newtonsoft.Json;


namespace ofisprojesi
{


    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private OfisProjesiContext _context;
        public EmployeeController(OfisProjesiContext context)
        {
            _context = context;
        }
        ///<summary>
        /// Çalışan kaydetme
        /// </summary>
        /// <return></return>
        [Route("")]
        [HttpPost]
        public ActionResult SaveEmployee([FromBody] Employee employee)
        {
            Office officecontroller = _context.Offices.Where(p => p.Id == employee.OfficeId).SingleOrDefault();
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
                _context.Employees.Add(employee);
                _context.SaveChanges();

                
                return Ok(employee);
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
            if (debitcontrol.EmployeeId!=null)
            {
                return BadRequest("KRİTİK HATA");
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
        public ActionResult UpdateEmployee([FromBody] Employee employee, int id)
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
                update.RecordDate = employee.RecordDate;
                update.UpdateDate = DateTime.Now;
                _context.SaveChanges();
                return Ok(update);
            }
        }
        ///<summary>
        /// Çalışanı id sine Göre Getir
        /// </summary>
        /// <return></return>
        [HttpGet("{id}", Name = "Get-Employee-By-Id")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            return Ok(_context.Employees.Where(p => p.Id == id).FirstOrDefault());

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
                return Ok(searchEmploye);
            }
            else
            {
                return BadRequest("kayıt bulunamadı");
            }
        }
    }
}









