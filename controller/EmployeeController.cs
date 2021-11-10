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

namespace ofisprojesi
{


    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public EmployeeController(OfisProjesiContext context, IMapper mapper)
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
        public ActionResult SaveEmployee([FromBody] EmployeeUpdateDto employee)
        {
            Office officecontroller = _context.Offices.Where(p => p.Id == employee.OfficeId).SingleOrDefault();
            Employee employee1 = new Employee();
            employee1.Name = employee.Name;
            employee1.OfficeId = employee.OfficeId;
            employee1.Lastname = employee.Lastname;
            employee1.Age = employee.Age;
            employee1.RecordDate = DateTime.Now;
            employee1.UpdateDate = DateTime.Now;
            employee1.Status = employee.Status;




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
            else
            {
                _context.Employees.Add(employee1);
                _context.SaveChanges();
                EmployeeUpdateDto dto = _mapper.Map<EmployeeUpdateDto>(employee);
                return Ok(dto);
            }
        }
        ///<summary>
        /// id ye göre Çalışan Silme
        /// </summary>
        /// <return></return>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteByIdEmployee([FromQuery] int id)
        {
            Employee Deleted = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            Debit debitcontrol = _context.Debits.FirstOrDefault(p => p.EmployeeId == id);
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
        /// id ye göre Çalışan Güncelleme
        /// </summary>
        /// <return></return>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateEmployee([FromBody] EmployeeUpdateDto employees, int id)
        {
            Employee update = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            Office officecontrol = _context.Offices.Where(p => p.Id == employees.OfficeId).FirstOrDefault();
            if (employees.Age == 0 || employees.Age == null)
            {
                return BadRequest("yaşınız 0 olamaz");
            }
            if (employees.Status == false && officecontrol.Id != employees.OfficeId)
            {
                return BadRequest("olmayan bir çalışanı bir ofise kayıt ettiremezsiniz");
            }
            else
            {

                update.Name = employees.Name;
                update.Lastname = employees.Lastname;
                update.Status = employees.Status;
                update.OfficeId = employees.OfficeId;
                update.Age = employees.Age;
                update.RecordDate = update.RecordDate;
                update.UpdateDate = DateTime.Now;
                _context.SaveChanges();
                EmployeeUpdateDto dto = _mapper.Map<EmployeeUpdateDto>(employees);
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

            Employee employees = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            EmployeeDto employe = _mapper.Map<EmployeeDto>(employees);
            Debit[] debit = _context.Debits.Where(p => p.EmployeeId == employe.Id).ToArray();
            DebitDto[] debit1 = _mapper.Map<DebitDto[]>(debit);
            employe.Debit = debit1;


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
            Employee[] searchEmploye = _context.Employees.ToArray();
            EmployeeDto[] employeeDtos = _mapper.Map<EmployeeDto[]>(searchEmploye);
            foreach (EmployeeDto employelist in employeeDtos)
            {
                Debit[] debits = _context.Debits.Where(p => p.EmployeeId == employelist.Id).ToArray();
                DebitDto[] debits1 = _mapper.Map<DebitDto[]>(debits);
                employelist.Debit = debits1;
            }


            if (!string.IsNullOrWhiteSpace(name))
            {
                searchEmploye = searchEmploye.Where(p => p.Name.Contains(name)).ToArray();
            }
            if (durum.HasValue)
            {
                searchEmploye = searchEmploye.Where(p => p.Status == durum).ToArray();
            }
            if (searchEmploye.ToList().Count > 0)
            {

                return Ok(employeeDtos);
            }
            else
            {

                return BadRequest("kayıt bulunamadı");
            }

        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Employee> name)
        {
            Employee emp = _context.Employees.FirstOrDefault(e => e.Id == id);

            name.ApplyTo(emp);
            _context.SaveChanges();
            return Ok(name);
        }
    }

}









