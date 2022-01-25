using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ofisprojesi;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ofisprojesi
{
    
    [Route("api/debits")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IDebitServices _debitservice;
        private readonly ILogger<DebitController> _logger;
        public DebitController(OfisProjesiContext context, IMapper mapper, IDebitServices debitservices, ILogger<DebitController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _debitservice = debitservices;
        }
        /// <summary>
        /// "duruma göre zimmet verisi getir"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public ActionResult GetAllDebit(debitservicesenum status, [FromQuery] DebitSearchDto datesearch)
        {

            try
            {
                DebitDto[] debit = _debitservice.GetAllDebit(status, datesearch);
                if (debit == null)
                {
                    return BadRequest(new { isSucces = false, message = "zimmet bilgisi bulunamadı" });
                }
                else
                {
                    return Ok(new { isSucces = true, message = debit });
                }
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("GetAllDebit metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "id ye Göre Zimmet Verisi sil"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteDebitById([FromQuery] int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new { isSucces = false, message = "id alanı boş bırakılamaz" });
                }
                DbActionResult debit = _debitservice.DeleteDebitById(id);
                ActionResult result = null;
                switch (debit)
                {
                    case DbActionResult.NotFound:
                        result = BadRequest(new { isSucces = true, message = "silinecek zimmet bilgisi bulunamadı" });
                        break;
                    case DbActionResult.Successful:
                        result = Ok(new { isSucces = true, message = "silme işlemi başarılı" });
                        break;
                    default:
                        result = BadRequest(new { isSucces = false, message = "bir hata oluştu" });
                        break;
                }
                return result;
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("DeleteDebitById metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "id'ye göre Zimmet Verisini Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateAllDebit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new { isSucces = false, message = "hata" });
                }

                DbActionResult debits = _debitservice.UpdateAllDebit(id);

                switch (debits)
                {
                    case DbActionResult.Successful:
                        return Ok(new { isSucces = true, message = "başarı ile güncellendi" });
                    case DbActionResult.UnknownError:
                        return BadRequest(new { isSucces = false, message = "bilinmeyen bir hata oluştu" });
                    case DbActionResult.NotHaveEmployee:
                        return BadRequest(new { isSucces = false, message = "çalışan bulunamadı" });
                    case DbActionResult.NotHaveFixture:
                        return BadRequest(new { isSucces = false, message = "demirbaş bulunamadı" });
                    default:
                        return BadRequest(new { isSucces = false, message = "bilinmeyen bir hata oluştu" });
                }
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("UpdateAllDebit metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "id ye Göre Zimmet Verisi Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<DebitDto> GetDebitById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new { isSucces = false, message = "id alanını boş bırakmayın" });
                }
                DebitDto debit = _debitservice.GetDebitById(id);
                if (debit == null)
                {
                    return BadRequest(new { isSucces = false, message = "aranan zimmet bilgisi bulunamadı" });
                }
                else
                {
                    return Ok(new { isSucces = true, message = debit });
                }
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("GetDebitById metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "Zimmet Verisi kaydet"
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult SaveDebitById([FromBody] DebitSaveDto debit)
        {
            try
            {
                if (debit == null)
                {
                    return BadRequest("zimmet bilgileri eksik veya yanlış");
                }
                DbActionResult debitt = _debitservice.SaveDebitById(debit);
                ActionResult result = null;
                switch (debitt)
                {
                    case DbActionResult.NotHaveFixture:
                        result = BadRequest(new { isSucces = false, message = "demirbaş bulunamadı" });
                        break;
                    case DbActionResult.NotHaveEmployee:
                        result = BadRequest(new { isSucces = false, message = "çalışan bulunamadı" });
                        break;
                    case DbActionResult.EmployeeFalse:
                        result = BadRequest(new { isSucces = false, message = "çalışan durumu zimmet için uygun değil" });
                        break;
                    case DbActionResult.FixtureFalse:
                        result = BadRequest(new { isSucces = false, message = "demirbaşın durumu zimmet için uygun değildir" });
                        break;
                    case DbActionResult.Successful:
                        result = Ok(new { isSucces = true, message = "zimmet kaydı başarılı" });
                        break;
                    default:
                        result = BadRequest(new { isSucces = false, message = "zimmetlenme sırasında hata" });
                        break;
                }
                return result;
            }
            catch (System.Exception)
            {

                _logger.LogError(string.Format("SaveDebitById metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }

        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Debit> name)
        {
            try
            {
                Debit debit = _debitservice.UpdatePatch(id, name);
                if (debit == null)
                {
                    return BadRequest(new { isSucces = false, message = "güncelleme başarısız" });
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









