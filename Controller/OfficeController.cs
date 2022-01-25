using Microsoft.AspNetCore.Mvc;
using Ofisprojesi;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ofisprojesi
{
    [Authorize]
    [Route("api/offices")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IOfficeServices _officeservices;
        private readonly ILogger<OfficeController> _logger;
        public OfficeController(OfisProjesiContext context, IMapper mapper, IOfficeServices officeServices, ILogger<OfficeController> logger)
        {
            _officeservices = officeServices;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }/// <summary>
         /// "Ofis Kaydetme"
         /// </summary>
         /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult SaveOffice([FromBody] OfficeUpdateDto offices)
        {
            try
            {
                if (offices == null)
                {
                    return BadRequest("hatalı giriş kayıt başarısız");
                }

                DbActionResult office = _officeservices.SaveOffice(offices);
                ActionResult result = null;
                switch (office)
                {
                    case DbActionResult.OfficeNotFound:
                        result = BadRequest(new { isSucces = false, message = "kayıt bilgileri hatalı lütfen tekrar kontrol edin" });
                        break;
                    case DbActionResult.Successful:
                        result = Ok(new { isSucces = true, message = "kayıt başarılı" });
                        break;
                    default:
                        result = BadRequest(new { isSucces = false, message = "bir hata ile karşılaşıldı" });
                        break;
                }
                {
                    return (result);
                }
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("SaveOffice metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }


        }
        /// <summary>
        /// "id ye göre Ofis Silme"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteOfficeById([FromQuery] int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("lütfen geçerli bir id girin");
                }
                DbActionResult office = _officeservices.DeleteOfficeById(id);
                ActionResult result = null;
                switch (office)
                {
                    case DbActionResult.OfficeNotFound:
                        result = BadRequest(new { isSucces = false, message = "ofis bulunmuyor." });
                        break;
                    case DbActionResult.UnknownError:
                        result = BadRequest(new { isSucces = false, message = "hata ile karşılaşıldı lütfen tekrar deneyin." });
                        break;
                    case DbActionResult.HaveDebitError:
                        result = BadRequest(new { isSucces = false, message = "bu ofisin içinde çalışanlar ve demirbaşlar bulunuyor lütfen önce onları çıkarın" });
                        break;
                    case DbActionResult.Successful:
                        result = Ok(new { isSucces = true, message = "silme işlemi başarılı" });
                        break;
                    default:
                        result = BadRequest(new { isSucces = false, message = "KRİTİK HATA" });
                        break;
                }
                return result;
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("DeleteOfficeById metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "id ye göre Ofis Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateOffice([FromBody] OfficeUpdateDto office, int? id, bool? durum)
        {
            try
            {
                if (office == null || id == null || durum == null)
                {
                    return BadRequest(new { isSucces = false, message = "lütfen boş alan bırakmayın" });
                }
                DbActionResult offices = _officeservices.UpdateDto(office, id, durum);
                ActionResult result = null;
                switch (offices)
                {
                    case DbActionResult.UnknownError:
                        result = BadRequest(new { isSucces = false, message = "bilinmeyen bir hata gerçekleşti" });
                        break;
                    case DbActionResult.HaveDebitError:
                        result = BadRequest(new { isSucces = false, message = "bu ofis içinde demirbaş veya çalışan var lütfen önce demirbaş ve çalışanları çıkarın" });
                        break;
                    case DbActionResult.Successful:
                        result = Ok(new { isSucces = true, message = "güncelleme başarılı" });
                        break;
                    default:
                        result = BadRequest(new { isSucces = false, message = "bir hata ile karşılaşıldı" });
                        break;
                }
                {
                    return result;
                }
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("UpdateOffice metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "id ye göre ofis sorgula"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<OfficeDto> GetOfficeById(int id)
        {
            try
            {
                OfficeDto office = _officeservices.GetOfficeById(id);
                if (office == null)
                {
                    return BadRequest(new { isSucces = false, message = "aranan kriterlere uygun ofis bulunamadı." });
                }
                else
                {
                    return Ok(new { isSucces = true, message = office });
                }
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("GetOfficeById metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "isme ve duruma göre office sorgulama"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public ActionResult GetOfficeByName([FromQuery] string name, bool? status)
        {
            try
            {
                OfficeDto[] office = _officeservices.GetOfficByName(name, status);
                if (office == null)
                {
                    return BadRequest(new { isSucces = false, message = "aranan kriterlere uygun sonuç bulunamadı" });
                }
                else
                {
                    return Ok(new { isSucces = true, message = office });
                }
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("GetOfficeByName metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Office> name)
        {
            try
            {
                OfficeDto Office = _officeservices.UpdatePatch(id, name);
                if (Office == null)
                {
                    return BadRequest(new { isSucces = false, message = "kayıt başarısız" });
                }
                else
                {
                    return Ok(new { isSucces = true, message = "güncelleme başarılı" });
                }
            }
            catch (System.Exception)
            {
                _logger.LogError(string.Format("UpdatePatch metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");

            }

        }
    }
}
