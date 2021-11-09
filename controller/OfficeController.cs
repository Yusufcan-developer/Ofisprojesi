using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Ofisprojesi;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace ofisprojesi
{
    [Route("api/offices")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private OfisProjesiContext _context;
        private readonly  IMapper _mapper;
        public OfficeController(OfisProjesiContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }/// <summary>
         /// "Ofis Kaydetme"
         /// </summary>
         /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult SaveOffice([FromBody] OfficeDto offices)
        {
            Office offices1 = new Office();
            offices1.Name=offices.Name;
            offices1.Status=offices.Status;
            if (offices.Name == null || !offices.Status.HasValue || offices.Id != null)
            {
                return BadRequest("hata");
            }
            else
            {
                _context.Offices.Add(offices1);
                _context.SaveChanges();
                OfficeDto dto =_mapper.Map<OfficeDto>(offices1);
                return Ok(dto);
            }
        }
        /// <summary>
        /// "id ye göre Ofis Silme"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteOfficeById([FromQuery] int id)
        {

            Office Delete = _context.Offices.SingleOrDefault(p => p.Id == id);
            Employee calisanSorgula = _context.Employees.FirstOrDefault(x => x.OfficeId == id);
            Fixture demirbasSorgula = _context.Fixtures.FirstOrDefault(p => p.OfficeId == id);

            if (Delete == null)
            {
                return BadRequest("silme işlemi başarısız kişi bulunmadı");
            }
            else if ((calisanSorgula != null) || (demirbasSorgula != null))
            {
                return BadRequest("Ofisin kullanıcı ve demirbaşta tanımlandığı için silemezsiniz");
            }
            else
            {
                _context.Offices.Remove(Delete);
                _context.SaveChanges();
                return BadRequest("silme başarılı");
            }

        }
        /// <summary>
        /// "Ofis Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]//ofis güncelle
        public ActionResult UpdateOffice([FromBody] OfficeDto office, int id)
        {
            Office Update = _context.Offices.FirstOrDefault(p => p.Id == id);
            if (office.Id != null)
            {
                return BadRequest("hata");
            }
            else
            {
                Update.Name = office.Name;
                Update.Status = office.Status;
                _context.SaveChanges();
                OfficeDto dto =_mapper.Map<OfficeDto>(Update);
                return Ok(dto);
            }

        }
        /// <summary>
        /// "Ofis id ye göre sorgula"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]//ofis id ye sorgulama
        public ActionResult GetOfficeById(int id)
        {

            Office source = _context.Offices.Where(p => p.Id == id).FirstOrDefault();
            OfficeDto dto =_mapper.Map<OfficeDto>(source);
            Fixture[] fixture = _context.Fixtures.Where(p=>p.OfficeId==dto.Id).ToArray();
            FixtureDto[] dtos = _mapper.Map<FixtureDto[]>(fixture);
            Employee[] employees = _context.Employees.Where(p=>p.OfficeId==dto.Id).ToArray();
            EmployeeDto[] dtos1 = _mapper.Map<EmployeeDto[]>(employees);
            dto.employee=dtos1;
            dto.Fixtures=dtos;
            return Ok(dto);
        }
        /// <summary>
        /// "Ofis Adına Göre Sorgula Yoksa Tümünü Getir"
        /// </summary>
        /// <returns></returns>
        [Route("name")]
        [HttpGet]//ofis adına göre sorgula yoksa tümünü getir
        public ActionResult GetOfisByName([FromQuery] string name, bool? status)
        {
            List<Office> OfficeNameSearch = _context.Offices.ToList();
            
            if (!string.IsNullOrWhiteSpace(name))
            {
                OfficeNameSearch = OfficeNameSearch.Where(p => p.Name.Contains(name)).ToList();
            }
            if (status.HasValue)
            {
                OfficeNameSearch = OfficeNameSearch.Where(p => p.Status == status).ToList();
            }
            if (OfficeNameSearch.ToList().Count > 0)
            {
                List<OfficeDto> dto = _mapper.Map<List<OfficeDto>>(OfficeNameSearch);
                return Ok(dto);
            }
            else
            {
                return BadRequest("kayıt bulunamadı");
            }
        }
        [HttpPatch]
        public ActionResult UpdatePatch(int id ,[FromBody]JsonPatchDocument<Office> name){
            Office emp = _context.Offices.FirstOrDefault(e=> e.Id == id);

            name.ApplyTo(emp);
           _context.SaveChanges();
           return Ok(name); 
        }
    }
}
