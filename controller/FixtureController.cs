using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Net.Http;
using System.Net;

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
        [Route("SaveFixture")]
        [HttpPost]//demirbas ekle
        public ActionResult SaveFixture([FromBody] Fixture fixture)
        {
            Office office = new Office();

            if (fixture.Status == false)
            {
                return BadRequest("bu demirbas kaydedilemez");
            }
            else if (fixture.Name == null)
            {
                return BadRequest("demirbaş ismi boş bırakılamaz");
            }
            else if (fixture.OfficeId == office.Id)
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
        [Route("DeleteFixtureById")]
        [HttpDelete]//demirbas sil
        public void DeleteFixtureById([FromQuery] int id)
        {
            var delete = _context.Fixtures.SingleOrDefault(p => p.Id == id);
            _context.Fixtures.Remove(delete);
            _context.SaveChanges();
        }
        /// <summary>
        /// "Demirbas Güncelleme"
        /// </summary>
        /// <returns></returns>
        [Route("UpdateFixture")]
        [HttpPut]//demirbas güncelle
        public void UpdateFixture(int id, [FromBody] Fixture fixturee)
        {
            var update = _context.Fixtures.FirstOrDefault(p => p.Id == id);
            update.Name = fixturee.Name;
            update.Status = fixturee.Status;
            update.OfficeId = fixturee.OfficeId;



            _context.SaveChanges();
            return;

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
        [Route("GetFixtureByName")]
        [HttpGet]//demibas adına göre sorgula yoksa veya nullsa tümünü getir
        public IList<Fixture> GetFixtureByName([FromQuery] string name)
        {

            var sorgula = _context.Fixtures.Where(p => p.Name.Contains(name)).ToList();


            var AllDataGet = _context.Fixtures.ToList();

            if (name == null)
            {
                return AllDataGet;
            }
            return sorgula;
        }
    }
}
