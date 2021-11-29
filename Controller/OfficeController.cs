using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Ofisprojesi;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace ofisprojesi
{   [Authorize]
    [Route("api/offices")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IOfficeServices _officeservices;
        public OfficeController(OfisProjesiContext context, IMapper mapper, IOfficeServices officeServices)
        {
            _officeservices = officeServices;
            _mapper = mapper;
            _context = context;
        }/// <summary>
         /// "Ofis Kaydetme"
         /// </summary>
         /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult SaveOffice([FromBody] OfficeUpdateDto offices)
        {
            OfficeUpdateDto office = _officeservices.SaveOffice(offices);
            if (office == null)
            {
                return BadRequest("hatalı giriş kayıt başarısız");
            }
            else
            {
                return Ok("kayıt başarılı");
            }
        }
        /// <summary>
        /// "id ye göre Ofis Silme"
        /// </summary>
        /// <returns></returns>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteOfficeById([FromQuery] int id)
        {
            Office office = _officeservices.DeleteOfficeById(id);
            if (office == null)
            {
                return BadRequest("silme başarısız");
            }
            else
            {
                return Ok("silme başarılı");
            }
        }
        /// <summary>
        /// "id ye göre Ofis Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateOffice([FromBody] OfficeUpdateDto office, int id)
        {
            OfficeUpdateDto offices = _officeservices.UpdateDto(office, id);
            if (offices == null)
            {
                return BadRequest("hatalı güncelleme");
            }
            else
            {
                return Ok("güncelleme başarılı");
            }
        }
        /// <summary>
        /// "id ye göre ofis sorgula"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<OfficeDto> GetOfficeById(int id)
        {
            OfficeDto office = _officeservices.GetOfficeById(id);
            if (office == null)
            {
                return BadRequest("hata var");
            }
            else
            {
                return Ok(office);
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
            OfficeDto[] office = _officeservices.GetOfficByName(name, status);
            if (office == null)
            {
                return BadRequest("hata var");
            }
            else
            {
                return Ok(office);
            }
        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Office> name)
        {
            OfficeDto Office = _officeservices.UpdatePatch(id, name);
            if (Office == null)
            {
                return BadRequest("kayıt başarısız");
            }
            else
            {
                return Ok("başarılı");
            }
        }
    }
}
