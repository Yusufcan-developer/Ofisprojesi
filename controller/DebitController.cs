using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using AutoMapper;
using Ofisprojesi;
using Microsoft.AspNetCore.JsonPatch;


namespace ofisprojesi
{
    [Route("api/debits")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public DebitController(OfisProjesiContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        /// <summary>
        /// "Tüm Zimmet Verilerini Getir"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]

        public ActionResult GetAllDebit(bool? status)

        {
            List<Debit> controller = _context.Debits.Where(p => p.Status == status).ToList();
            if (status == null)
            {
                return BadRequest("kayıt bulunamadı");
            }
            if (status == true)
            {
                List<DebitDto> dto = _mapper.Map<List<DebitDto>>(controller);
                return Ok(dto);
            }
            if (status == false)
            {
                List<DebitDto> dto = _mapper.Map<List<DebitDto>>(controller);
                return Ok(dto);
            }
            else
            {
                return BadRequest("seçim yapılmadı");
            }

        }
        /// <summary>
        /// "id ye Göre Zimmet Verisi sil"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]

        public ActionResult DeleteDebitById([FromQuery] int id)

        {
            Debit deleted = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
            if (deleted == null)
            {
                return BadRequest("ARANAN KİŞİ BULUNAMADI");
            }
            else
            {
                _context.Debits.Remove(deleted);
                _context.SaveChanges();
                return Ok("başarı ile silindi");

            }

        }
        /// <summary>
        /// "Zimmet Verisini Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]

        public ActionResult UpdateAllDebit(int id, [FromBody] DebitDto debit)
        {
            Employee employecont = _context.Employees.Where(p => p.Id == debit.EmployeeId).FirstOrDefault();
            Fixture fixturecont = _context.Fixtures.Where(p => p.Id == debit.FixtureId).FirstOrDefault();
            Debit update = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
            if (update == null)
            {
                return BadRequest("hata");
            }
            else if (employecont == null || fixturecont == null)
            {
                return BadRequest("çalışan veya demirbaş yok");
            }
            else if (debit.Id != null)
            {
                return BadRequest(" id hatası");
            }
            else
            {
                update.EmployeeId = debit.EmployeeId;
                update.Status = debit.status;
                update.FixtureId = debit.FixtureId;
                update.Date = DateTime.Now;
                _context.SaveChanges();
                DebitDto dto = _mapper.Map<DebitDto>(update);
                return Ok(dto);

            }
        }
        /// <summary>
        /// "id ye Göre Zimmet Verisi Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetDebitById(int id)
        {

            Debit debit = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
            DebitDto dto = _mapper.Map<DebitDto>(debit);
            return Ok(dto);
        }
        /// <summary>
        /// "Zimmet Verisi kaydet"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]//zimmet verisi kaydet

        public ActionResult SaveDebitById([FromBody] DebitDto debit)
        {
            Debit debit1 = new Debit();
            debit1.EmployeeId=debit.EmployeeId;
            debit1.FixtureId=debit.FixtureId;
            debit1.Date=debit.date;
            debit1.Status=debit.status;
            Fixture fixturess = _context.Fixtures.Where(p => p.Id == debit.FixtureId).FirstOrDefault();
            Employee employeess = _context.Employees.Where(p => p.Id == debit.EmployeeId).FirstOrDefault();

            if (employeess.Id != debit.EmployeeId || employeess.Status == false)
            {
                return BadRequest("hata var");
            }
            else if (debit.Id != null)
            {
                return BadRequest("id hatası");
            }
            if (fixturess.Id != debit.FixtureId || fixturess.Status == false)
            {
                return BadRequest("hata var");
            }
            else
            {
                _context.Debits.Add(debit1);
                _context.SaveChanges();
                DebitDto dto = _mapper.Map<DebitDto>(debit1);
                return Ok(dto);
            }
        }
        [Route("{id}")]
        [HttpPatch]
        public ActionResult UpdatePatch(int id ,[FromBody]JsonPatchDocument<Debit> name){
            Debit emp = _context.Debits.FirstOrDefault(e=> e.Id == id);

            name.ApplyTo(emp);
           _context.SaveChanges();
           return Ok(name); 
        }
    }
}









