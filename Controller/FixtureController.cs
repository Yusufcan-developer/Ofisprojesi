using System;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Ofisprojesi;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using PagedList.Core;
using System.Collections.Generic;

namespace ofisprojesi
{

    [Route("api/fixtures")]
    [ApiController]
    public class FixtureController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IFixtureServices _fixtureservices;
        private readonly ILogger<FixtureController> _logger;
        private IDebitServices _debitservice;
        public FixtureController(OfisProjesiContext context, IMapper mapper, IFixtureServices fixtureServices, ILogger<FixtureController> logger, IDebitServices debitservices)
        {
            _fixtureservices = fixtureServices;
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _debitservice = debitservices;
        }
        /// <summary>
        /// "Demirbas Kaydetme"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult SaveFixture([FromBody] FixtureUpdateDto fixture)
        {
            try
            {
                if (fixture == null)
                {
                    return BadRequest(new { isSucces = false, message = "lütfen bilgileri eksiksiz ve doğru girdiğinize emin olun" });
                }
                ActionResult result = null;
                DbActionResult Fixture = _fixtureservices.SaveFixture(fixture);
                switch (Fixture)
                {
                    case DbActionResult.UnknownError:
                        result = BadRequest(new { isSucces = false, message = "Bir hata oluştu, lütfen gereklilikleri yerine getirerek tekrar deneyiniz." });
                        break;
                    case DbActionResult.OfficeNotFound:
                        result = BadRequest(new { isSucces = false, message = "offis alanı boş bırakılamaz" });
                        break;
                    case DbActionResult.NameOrLastNameError:
                        result = BadRequest(new { isSucces = false, message = "isim alanı boş bırakılamaz" });
                        break;
                    case DbActionResult.Successful:
                        result = Ok(new { isSucces = true, message = "kayıt başarılı" });
                        break;
                    default:
                        result = BadRequest(new { isSucces = false, message = "bir sorunla karşılaşıldı" });
                        break;
                }
                return result;
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("SaveFixture metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "id ye göre Demirbaş Silme"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]
        public ActionResult<Fixture> DeleteFixtureById([FromQuery] int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("id boş bırakılamaz");
                }
                DbActionResult fixture = _fixtureservices.DeleteFixtureById(id);
                ActionResult Result = null;
                switch (fixture)
                {
                    case DbActionResult.UnknownError:
                        Result = BadRequest(new { isSucces = false, message = "aranan demirbaş bulunamadı" });
                        break;
                    case DbActionResult.HaveDebitError:
                        Result = BadRequest(new { isSucces = false, message = "silmeye çalıştığınız demirbaş bir veya birden çok kişiye zimmetlidir lütfen önce zimmeti kaldırın" });
                        break;
                    case DbActionResult.Successful:
                        Result = BadRequest(new { isSucces = true, message = "başarı ile silindi" });
                        break;
                    default:
                        Result = BadRequest(new { isSucces = false, message = "bir sorun oluştu" });
                        break;
                }
                return Result;
            }
            catch (System.Exception)
            {
                _logger.LogError(string.Format("DeleteFixtureById metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "id ye göre Demirbas Güncelleme"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateFixture([FromBody] FixtureUpdateDto fixture, int? id, bool? durum)
        {
            try
            {
                if (fixture == null || id == null || durum == null)
                {
                    return BadRequest(new { isSucces = false, message = "update bilgileri hatalı" });
                }
                DbActionResult Fixture = _fixtureservices.UpdateFixture(fixture, id, durum);
                ActionResult result = null;
                switch (Fixture)
                {
                    case DbActionResult.NameOrLastNameError:
                        result = BadRequest(new { isSucces = false, message = "demirbaşın ismi boş bırakılamaz" });
                        break;
                    case DbActionResult.OfficeNotFound:
                        result = BadRequest(new { isSucces = false, message = "office bulunamadı" });
                        break;
                    case DbActionResult.UnknownError:
                        result = BadRequest(new { isSucces = false, message = "demirbaş bulunamadı" });
                        break;
                    case DbActionResult.Successful:
                        result = Ok(new { isSucces = true, message = "güncelleme başarılı" });
                        break;
                    default:
                        result = BadRequest(new { isSucces = false, message = "hatalı güncelleme" });
                        break;
                }
                return result;
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("UpdateFixture metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "Demirbas id ye göre Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<FixtureDto> GetFixtureById(int id)
        {
            try
            {
                FixtureDto fixture = _fixtureservices.GetFixtureById(id);
                if (fixture == null) return BadRequest(new { isSucces = false, message = "aranan kritere uygun demirbaş bulunamadı" });
        
                return Ok(fixture);
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("GetFixtureById metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "isme ve duruma göre çalışan sorgulama"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public ActionResult GetFixtureByName([FromQuery] string fixturename, fixtureservicesenum? status, [FromQuery] int pagecount, [FromQuery] int pageindex,[FromQuery] criteria criteria)
        {
            try
            {
                ResultDto fixture = _fixtureservices.GetFixtureByName(fixturename, status,pagecount,pageindex, criteria);
                if (fixture == null) return BadRequest("Böyle bir çalışan bulunamadı.");
                
                return Ok(fixture);

            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("GetFixtureByName metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı"); ;
            }

        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Fixture> name)
        {
            try
            {
                Fixture fixture = _fixtureservices.UpdatePatch(id, name);

                if (fixture == null) return BadRequest("hata var");

                return Ok("güncelleme başarılı");
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("UpdatePatch metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
    }
}
