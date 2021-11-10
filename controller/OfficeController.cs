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
        private readonly IMapper _mapper;
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
        public ActionResult SaveOffice([FromBody] OfficeUpdateDto offices)
        {
            Office offices1 = new Office();
            offices1.Name = offices.Name;
            offices1.Status = offices.Status;
            if (offices.Name == null || !offices.Status.HasValue)
            {
                return BadRequest("hata");
            }
            else
            {
                _context.Offices.Add(offices1);
                _context.SaveChanges();
                OfficeUpdateDto dto = _mapper.Map<OfficeUpdateDto>(offices1);
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
            Employee employesearch = _context.Employees.FirstOrDefault(x => x.OfficeId == id);
            Fixture fixturesearch = _context.Fixtures.FirstOrDefault(p => p.OfficeId == id);

            if (Delete == null)
            {
                return BadRequest("silme işlemi başarısız kişi bulunmadı");
            }
            else if ((employesearch != null) || (fixturesearch != null))
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
        /// "id ye göre Ofis Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateOffice([FromBody] OfficeUpdateDto office, int id)
        {
            Office Update = _context.Offices.FirstOrDefault(p => p.Id == id);

            Update.Name = office.Name;
            Update.Status = office.Status;
            _context.SaveChanges();
            OfficeUpdateDto dto = _mapper.Map<OfficeUpdateDto>(Update);
            return Ok(dto);


        }
        /// <summary>
        /// "id ye göre ofis sorgula"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetOfficeById(int id)
        {

            Office source = _context.Offices.Where(p => p.Id == id).FirstOrDefault();
            OfficeDto sourcedto = _mapper.Map<OfficeDto>(source);
            Fixture[] fixture = _context.Fixtures.Where(p => p.OfficeId == sourcedto.Id).ToArray();
            FixtureDto[] fixturedto = _mapper.Map<FixtureDto[]>(fixture);
            Employee[] employees = _context.Employees.Where(p => p.OfficeId == sourcedto.Id).ToArray();
            EmployeeDto[] employeeDto = _mapper.Map<EmployeeDto[]>(employees);
            sourcedto.employee = employeeDto;
            sourcedto.Fixtures = fixturedto;
            return Ok(sourcedto);
        }
        /// <summary>
        /// "Ofis Adına Göre Sorgula Yoksa Tümünü Getir"
        /// </summary>
        /// <returns></returns>
        [Route("name")]
        [HttpGet]
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
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Office> name)
        {
            Office emp = _context.Offices.FirstOrDefault(e => e.Id == id);

            name.ApplyTo(emp);
            _context.SaveChanges();
            return Ok(name);
        }
    }
}
