using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

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
        [Route("SaveEmployee")]
        [HttpPost]//calisan Kaydetme
        public ActionResult SaveEmployee([FromBody] Employee employee)
        {
            Office officee = new Office();
            if (employee.Status == false || employee.Status == null)
            {
                return BadRequest("çalışana ilk kayıt sırasında negatif veya boş değer verilemez");
            }
            else if (employee.Age > 150)
            {
                return BadRequest("kayıt başarısız çalışan ölmüş lütfen gömün");
            }
            else if (employee.Name == null || employee.Lastname == null)
            {
                return BadRequest("isim veya soyisim boş bırakılamaz");
            }
            else if (employee.OfficeId == null || employee.OfficeId != officee.Id)
            {
                return BadRequest("kayıtlı office bulunamadı");

            }
            else
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();

                return Ok("kişi kayıtı başarılı");
            }




        }
        ///<summary>
        /// Çalışan Silme
        /// </summary>
        /// <return></return>
        [Route("DeleteByIdEmployee")]
        [HttpDelete]//calisan silme
        public ActionResult DeleteByIdEmployee([FromQuery] int id)
        {
            var Deleted = _context.Employees.SingleOrDefault(p => p.Id == id);
            Employee employee = new Employee();
            if (Deleted.Id != id)
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
        [Route("EmployeeUpdate")]
        [HttpPut]//calisan güncelle
        public ActionResult UpdateEmployee([FromBody] Employee employee, int id)
        {
            var update = _context.Employees.FirstOrDefault(p => p.Id == id);
            Office office = new Office();
            if (update.OfficeId == null || id != update.Id)
            {
                return BadRequest("ofis bulunumadı");
            }
            else if (employee.Age == 0 || employee.Age == null)
            {
                return BadRequest("yaşınız 0 olamaz");
            }
            else if (employee.Status == false)
            {
                return BadRequest("olmayan bir çalışanı bir ofise kayıt ettiremezsiniz");
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
                return Ok("güncelleme başarılı");
            }
        }
        ///<summary>
        /// Çalışanı id sine Göre Getir
        /// </summary>
        /// <return></return>
        [HttpGet("{userId:int}", Name = "GetEmployeeById")]//id ye göre calisan listele
        public ActionResult<Employee> GetEmployeeById(int userId)
        {
            return Ok(_context.Employees.Where(p => p.Id == userId).FirstOrDefault());




        }
        ///<summary>
        /// Tüm Çalışanları Getir
        /// </summary>
        /// <return></return>
        [Route("GetEmployeeByName")]
        [HttpGet]//aktif çalışanların adını sorgula yoksa tümünü getir
        public IList<Employee> GetEmployeeByName([FromQuery] string name)
        {


            var SearchName = (_context.Employees.Where(p => p.Name.Contains(name)).ToList());


            var GetAllData = _context.Employees.ToList();

            if (name == null)
            {
                return GetAllData;
            }
            return SearchName;
        }
        ///<summary>
        /// işten çıkarılmış çalışanları getir
        /// </summary>
        /// <return></return>
        [Route("GetFiredEmployeeByName")]
        [HttpGet]//kovulan calisan adı sorgula yoksa tümünü getir
        public ActionResult GetFiredEmployeeByName([FromQuery] string name)
        {
            Employee employee = new Employee();

                var SearchName = (_context.Employees.Where(p => p.Name.Contains(name)&&p.Status==false).ToList());
                var GetAllData =_context.Employees.Where(p=>p.Status==false).ToList();
                if (name == null)
                {

                    return Ok(GetAllData);
                }
                
                return Ok(SearchName);
            }







        }

    }








