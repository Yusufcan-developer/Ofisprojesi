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
        [Route("Save-Employee")]
        [HttpPost]
        public ActionResult SaveEmployee([FromBody] Employee employee)
        {
            Office officecontroller = _context.Offices.Where(p => p.Id == employee.OfficeId).SingleOrDefault();
            if (employee.Status == false || employee.Status == null)
            {
                return BadRequest("çalışana ilk kayıt sırasında negatif veya boş değer verilemez");
            }
            else if (employee.Age > 150)
            {
                return BadRequest("kayıt başarısız çalışan ölmüş lütfen en yakın mezar sağlayıcısına başvurun");
            }
            else if (employee.Name == null || employee.Lastname == null)
            {
                return BadRequest("isim veya soyisim boş bırakılamaz");
            }
            else if (employee.OfficeId == null || officecontroller == null)
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
        [Route("Delete-Employee-By-Id")]
        [HttpDelete]
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
        [Route("Update-Employee")]
        [HttpPut]
        public ActionResult UpdateEmployee([FromBody] Employee employee, int id)
        {
            Employee update = _context.Employees.FirstOrDefault(p => p.Id == id);
            Office officecontrol = _context.Offices.Where(p => p.Id == id).SingleOrDefault();
            if (update.OfficeId == null || id != officecontrol.Id)
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
        [HttpGet("{userId:int}", Name = "Get-Employee-By-Id")]
        public ActionResult<Employee> GetEmployeeById(int userId)
        {
            return Ok(_context.Employees.Where(p => p.Id == userId).FirstOrDefault());




        }
        ///<summary>
        /// Tüm Çalışanları Getir
        /// </summary>
        /// <return></return>
        [Route("Get-Employee-By-Name")]
        [HttpGet]
        public ActionResult GetEmployeeByName([FromQuery] string name, bool durum, bool names)
        {


            List<Employee> SearchName = _context.Employees.Where(p => p.Name.Contains(name)).ToList();
            List<Employee> durumcontroltrue = _context.Employees.Where(p => p.Status == durum).ToList();
            List<Employee> durumcontrolfalse = _context.Employees.Where(p => p.Status == durum).ToList();

            List<Employee> GetAllData = _context.Employees.ToList();


            if (names == true)
            {
                return Ok(GetAllData);
            }
            else if (durum == true)
            {
                return Ok(durumcontroltrue);
            }
            else if (durum == false)
            {
                return Ok(durumcontrolfalse);
            }
            else if (name!=null){
                return Ok(SearchName);
            }
            else{
                return BadRequest("arama sırasında hata");

            }
        }
    }
}









