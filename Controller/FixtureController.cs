using System;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Ofisprojesi;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace ofisprojesi
{[Authorize]
    [Route("api/fixtures")]
    [ApiController]
    public class FixtureController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IFixtureServices _fixtureservices;
        public FixtureController(OfisProjesiContext context, IMapper mapper, IFixtureServices fixtureServices)
        {
            _fixtureservices = fixtureServices;
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
            Fixture Fixture = _fixtureservices.SaveFixture(fixture);

            if (fixture == null) return BadRequest("Bir hata oluştu, lütfen gereklilikleri yerine getirerek tekrar deneyiniz.");
            
            return Ok("Demirbaş kayıt etme işlemi başarılı!");
        }
        /// <summary>
        /// "id ye göre Demirbaş Silme"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]
        public ActionResult<Fixture> DeleteFixtureById([FromQuery] int id)
        {
            Fixture fixture = _fixtureservices.DeleteFixtureById(id);

            if (fixture == null) return BadRequest("aranan kişi bulunamadı");
            
            return Ok("başarı ile silindi");
        }
        /// <summary>
        /// "id ye göre Demirbas Güncelleme"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateFixture([FromBody] FixtureDto fixture, int id)
        {
            FixtureDto Fixture = _fixtureservices.UpdateFixture(fixture, id);
            if (Fixture == null) return BadRequest("hata var");
      
            return Ok("güncelleme başarılı");
        }
        /// <summary>
        /// "Demirbas id ye göre Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<FixtureDto> GetFixtureById(int id)
        {
            FixtureDto fixture = _fixtureservices.GetFixtureById(id);
            if (fixture == null) return BadRequest("hata");
    
            return Ok(fixture);
        }
        /// <summary>
        /// "isme ve duruma göre çalışan sorgulama"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public ActionResult GetFixtureByName([FromQuery] String name, bool? status)
        {
            if(name == null && status == null) return BadRequest("İsim kısmı boş bırakılamaz.");

            FixtureDto[] fixture = _fixtureservices.GetFixtureByName(name, status);
            
            if (fixture == null) return BadRequest("Böyle bir çalışan bulunamadı.");
            
            return Ok(fixture);
        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Fixture> name)
        {
            Fixture fixture = _fixtureservices.UpdatePatch(id, name);

            if (fixture == null) return BadRequest("hata var");

            return Ok("güncelleme başarılı");
        }
    }
}
