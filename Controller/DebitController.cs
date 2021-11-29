using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ofisprojesi;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace ofisprojesi
{
    [Authorize]
    [Route("api/debits")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IDebitServices _debitservice;
        public DebitController(OfisProjesiContext context, IMapper mapper, IDebitServices debitservices)
        {
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
        public ActionResult GetAllDebit(bool? status)
        {
            List<DebitDto> debit = _debitservice.GetAllDebit(status);
            if (debit == null)
            {
                return BadRequest("sorgu başarısız");
            }
            else
            {
                return Ok(debit);
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
            Debit debit = _debitservice.DeleteDebitById(id);
            if (debit == null)
            {
                return BadRequest("silme başarısız");
            }
            else
            {
                return Ok("silme başarılı");
            }
        }
        /// <summary>
        /// "id'ye göre Zimmet Verisini Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateAllDebit(int id, [FromBody] DebitDto debit)
        {
            DebitDto debits = _debitservice.UpdateAllDebit(id, debit);
            if (debits == null)
            {
                return BadRequest("kayıt başarısız");
            }
            else
            {
                return Ok("kayıt başarılı");
            }
        }
        /// <summary>
        /// "id ye Göre Zimmet Verisi Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<DebitDto> GetDebitById(int id)
        {
            DebitDto debit = _debitservice.GetDebitById(id);
            if (debit == null)
            {
                return BadRequest("kayıt bulunamadı");
            }
            else
            {
                return Ok(debit);
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
            DebitDto debitt = _debitservice.SaveDebitById(debit);
            if (debitt == null)
            {
                return BadRequest("kayıt sırasında sorun oluştu");
            }
            else
            {
                return Ok("kayıt başarılı");
            }
        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Debit> name)
        {
            Debit debit = _debitservice.UpdatePatch(id, name);
            if (debit == null)
            {
                return BadRequest("güncelleme başarısız");
            }
            else
            {
                return Ok("güncelleme başarılı");
            }
        }
    }
}









