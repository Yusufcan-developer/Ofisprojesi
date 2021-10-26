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

namespace ofisprojesi
{
    [Route("api/fixtures")]
    [ApiController]
    public class FixtureController : ControllerBase
    {

        private OfisProjesiContext _context;
        public FixtureController(OfisProjesiContext context)
        {
            _context = context;
        }
        /// <summary>
        /// "Demirbas Kaydetme"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]//demirbas ekle
        public ActionResult<Fixture> SaveFixture([FromBody] Fixture fixture)
        {
            Office officecontrol = _context.Offices.Where(p => p.Id == fixture.OfficeId).SingleOrDefault();

            if (fixture.Status == false)
            {
                return BadRequest("bu demirbas kaydedilemez");
            }
            else if (fixture.Name == null)
            {
                return BadRequest("demirbaş ismi boş bırakılamaz");
            }

            else if (fixture.OfficeId == null || fixture.OfficeId < 0 || officecontrol == null)
            {
                return BadRequest("ofis bulunamadı");
            }
            else if (fixture.Id != null)
            {
                return BadRequest("kayıt sırasında id gönderilmez");
            }
            else
            {
                _context.Fixtures.Add(fixture);
                _context.SaveChanges();
                return Ok(fixture);
            }

        }

        /// <summary>
        /// "Demirbaş Silme"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]//demirbas sil
        public ActionResult DeleteFixtureById([FromQuery] int id)
        {
            
            Fixture delete = _context.Fixtures.FirstOrDefault(p => p.Id == id);
            Debit debits=_context.Debits.Where(p=>p.FixtureId==id).FirstOrDefault();
            if (delete == null || delete.Id==debits.FixtureId)
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
        [HttpPut]//demirbas güncelle
        public ActionResult UpdateFixture([FromBody] Fixture fixturee, int id)
        {
            Fixture update = _context.Fixtures.Where(p => p.Id == id).FirstOrDefault();
            Office officecontrol = _context.Offices.Where(p => p.Id == fixturee.OfficeId).FirstOrDefault();
            if (fixturee.Status == false && officecontrol == null)
            {
                return BadRequest("olmayan bir demirbaşı bir ofise kaydedemezsin");
            }
            else if(fixturee.Status==true&&officecontrol==null)
            {
                return BadRequest("office bulunamadı");
            }
            else if (fixturee.Id!=null)
            {
                return BadRequest("id hatası");
            }
            {
                update.Name = fixturee.Name;
                update.Status = fixturee.Status;
                update.OfficeId = fixturee.OfficeId;
                _context.SaveChanges();
                return Ok(update);
            }
        }
        /// <summary>
        /// "Demirbas id ye göre Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]//demirbas id ye göre sorgula
        public Fixture GetFixtureById(int id)
        {
            return _context.Fixtures.Where(p => p.Id == id).FirstOrDefault();
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
                return Ok(searchfixture);
            }
            else
            {
                return BadRequest("kayıt bulunamadı");
            }
        }
    }
}
