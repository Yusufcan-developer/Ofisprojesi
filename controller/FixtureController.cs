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
        [Route("Save-Fixture")]
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
            else
            {
                _context.Fixtures.Add(fixture);
                _context.SaveChanges();
                return Ok("kayıt başarılı");
            }

        }

        /// <summary>
        /// "Demirbaş Silme"
        /// </summary>
        /// <returns></returns>
        [Route("Delete-Fixture-By-Id")]
        [HttpDelete]//demirbas sil
        public ActionResult DeleteFixtureById([FromQuery] int id)
        {
            Fixture delete = _context.Fixtures.SingleOrDefault(p => p.Id == id);
            if (delete == null)
            {
                return BadRequest("silmeye çalıştığınız kişi bulunamadı");
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
        [Route("Update-Fixture")]
        [HttpPut]//demirbas güncelle
        public ActionResult UpdateFixture(int id, [FromBody] Fixture fixturee)
        {
            Office officecontrol = _context.Offices.Where(p => p.Id == fixturee.OfficeId).SingleOrDefault();
            Fixture update = _context.Fixtures.FirstOrDefault(p => p.Id == id);
            if (update == null)
            {
                return BadRequest("güncellenecek kişi bulunamadı");
            }
            else if (update.Status == false)
            {
                return BadRequest("olmayan bir demirbaşı kaydedemezsiniz");
            }
            else if (officecontrol == null)
            {
                return BadRequest("ofis bulunamadı");
            }
            else
            {
                update.Name = fixturee.Name;
                update.Status = fixturee.Status;
                update.OfficeId = fixturee.OfficeId;



                _context.SaveChanges();
                return Ok("demirbas kaydedildi");
            }


        }
        /// <summary>
        /// "Demirbas id ye göre Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Id:int}")]//demirbas id ye göre sorgula
        public Fixture GetFixtureById(int Id)
        {
            return _context.Fixtures.Where(p => p.Id == Id).FirstOrDefault();
        }
        /// <summary>
        /// "Tüm Demirbasları Getir"
        /// </summary>
        /// <returns></returns>
        [Route("Get-Fixture-By-Name")]
        [HttpGet]//demibas adına göre sorgula yoksa veya nullsa tümünü getir
        public IList<Fixture> GetFixtureByName([FromQuery] string name)
        {
            List<Fixture> sorgula = _context.Fixtures.Where(p => p.Name.Contains(name)).ToList();
            List<Fixture> AllDataGet = _context.Fixtures.ToList();
            if (name == "tüm demirbaşları getir")
            {
                return AllDataGet;
            }

            return sorgula;
        }
    }
}
