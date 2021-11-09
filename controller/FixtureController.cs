using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Net.Http;
using System.Net;
using ofisprojesi;
using AutoMapper;
using Ofisprojesi;
using Microsoft.AspNetCore.JsonPatch;

namespace ofisprojesi
{
    [Route("api/fixtures")]
    [ApiController]
    public class FixtureController : ControllerBase
    {

        private OfisProjesiContext _context;
        private IMapper _mapper;
        public FixtureController(OfisProjesiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// "Demirbas Kaydetme"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult SaveFixture([FromBody] FixtureDto fixture)
        {
            Office officecontrol = _context.Offices.Where(p => p.Id == fixture.Officeid).SingleOrDefault();
            Fixture fixture1 = new Fixture();
            fixture1.Name=fixture.Name;
            fixture1.OfficeId=fixture.Officeid;
            fixture1.Status=fixture.Status;

            if (fixture.Status == false)
            {
                return BadRequest("bu demirbas kaydedilemez");
            }
            else if (fixture.Name == null)
            {
                return BadRequest("demirbaş ismi boş bırakılamaz");
            }

            else if (fixture.Officeid == null || fixture.Officeid < 0 || officecontrol == null)
            {
                return BadRequest("ofis bulunamadı");
            }
            else if (fixture.Id != null)
            {
                return BadRequest("kayıt sırasında id gönderilmez");
            }
            else
            {
                _context.Fixtures.Add(fixture1);
                _context.SaveChanges();
                FixtureDto dto =_mapper.Map<FixtureDto>(fixture1);
                return Ok(dto);
            }

        }

        /// <summary>
        /// "Demirbaş Silme"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteFixtureById([FromQuery] int id)
        {

            Fixture delete = _context.Fixtures.FirstOrDefault(p => p.Id == id);
            Debit debits = _context.Debits.Where(p => p.FixtureId == id).FirstOrDefault();
            if (delete == null || delete.Id == debits.FixtureId)
            {
                return BadRequest("silmeye çalıştığınız demirbas bulunamadı");
            }
            else
            {
                _context.Fixtures.Remove(delete);
                _context.SaveChanges();
                return Ok("silme işlemi başarılı");
            }
        }
        /// <summary>
        /// "Demirbas Güncelleme"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateFixture([FromBody] FixtureDto fixturee, int id)
        {
            Fixture update = _context.Fixtures.Where(p => p.Id == id).FirstOrDefault();
            Office officecontrol = _context.Offices.Where(p => p.Id == fixturee.Officeid).FirstOrDefault();
            if (fixturee.Status == false && officecontrol == null)
            {
                return BadRequest("olmayan bir demirbaşı bir ofise kaydedemezsin");
            }
            else if (fixturee.Status == true && officecontrol == null)
            {
                return BadRequest("office bulunamadı");
            }
            else if (fixturee.Id != null)
            {
                return BadRequest("id hatası");
            }
            {
                update.Name = fixturee.Name;
                update.Status = fixturee.Status;
                update.OfficeId = fixturee.Officeid;
                _context.SaveChanges();
                FixtureDto dto =_mapper.Map<FixtureDto>(fixturee);
                return Ok(dto);
            }
        }
        /// <summary>
        /// "Demirbas id ye göre Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetFixtureById(int id)
        {
            Fixture fixtures = _context.Fixtures.Where(p => p.Id == id).FirstOrDefault();
            FixtureDto dto = _mapper.Map<FixtureDto>(fixtures);
            return Ok(dto);
        }
        /// <summary>
        /// "Tüm Demirbasları Getir"
        /// </summary>
        /// <returns></returns>
        [Route("name")]
        [HttpGet]//demibas adına göre sorgula yoksa veya nullsa tümünü getir
        public ActionResult GetFixtureByName([FromQuery] String name, bool? durum)
        {
            List<Fixture> searchfixture = _context.Fixtures.ToList();
            

            if (!string.IsNullOrWhiteSpace(name))
            {
                searchfixture = searchfixture.Where(p => p.Name.Contains(name)).ToList();
            }
            if (durum.HasValue)
            {
                searchfixture = searchfixture.Where(p => p.Status == durum).ToList();
            }
            if (searchfixture.ToList().Count > 0)
            {
                List<FixtureDto> dto =_mapper.Map<List<FixtureDto>>(searchfixture);
                return Ok(dto);
            }
            else
            {
                return BadRequest("kayıt bulunamadı");
            }
        }
        [HttpPatch]
        public ActionResult UpdatePatch(int id ,[FromBody]JsonPatchDocument<Fixture> name){
            Fixture emp = _context.Fixtures.FirstOrDefault(e=> e.Id == id);

            name.ApplyTo(emp);
           _context.SaveChanges();
           return Ok(name); 
        }
    }
}
