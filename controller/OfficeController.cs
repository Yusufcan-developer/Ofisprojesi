using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ofisprojesi
{
    [Route("api/offices")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private OfisProjesiContext _context;
        public OfficeController(OfisProjesiContext context)
        {
            _context = context;
        }/// <summary>
         /// "Ofis Kaydetme"
         /// </summary>
         /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult SaveOffice([FromBody] Office offices)
        {
            if (offices.Name == null || !offices.Status.HasValue || offices.Id != null)
            {
                return BadRequest("hata");
            }
            else
            {
                _context.Offices.Add(offices);
                _context.SaveChanges();
                return Ok(offices);
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

            Office Delete = _context.Offices.SingleOrDefault(p => p.Id == id);
            Employee calisanSorgula = _context.Employees.FirstOrDefault(x => x.OfficeId == id);
            Fixture demirbasSorgula = _context.Fixtures.FirstOrDefault(p => p.OfficeId == id);

            if (Delete == null)
            {
                return BadRequest("silme işlemi başarısız kişi bulunmadı");
            }
            else if ((calisanSorgula != null) || (demirbasSorgula != null))
            {
                return BadRequest("Ofisin kullanıcı ve demirbaşta tanımlandığı için silemezsiniz");
            }
            else
            {
                _context.Offices.Remove(Delete);
                _context.SaveChanges();
                return BadRequest("silme başarılı");
            }

        }
        /// <summary>
        /// "Ofis Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]//ofis güncelle
        public ActionResult UpdateOffice([FromBody] Office office, int id)
        {
            Office Update = _context.Offices.FirstOrDefault(p => p.Id == id);
            if (office.Id != null)
            {
                return BadRequest("hata");
            }
            else
            {
                Update.Name = office.Name;
                Update.Status = office.Status;
                _context.SaveChanges();
                return Ok(Update);
            }

        }
        /// <summary>
        /// "Ofis id ye göre sorgula"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]//ofis id ye sorgulama
        public Office GetOfficeById(int id)
        {
            return _context.Offices.Where(p => p.Id == id).FirstOrDefault();
        }
        /// <summary>
        /// "Ofis Adına Göre Sorgula Yoksa Tümünü Getir"
        /// </summary>
        /// <returns></returns>
        [Route("name")]
        [HttpGet]//ofis adına göre sorgula yoksa tümünü getir
        public ActionResult GetOfisByName([FromQuery] string name, bool? status)
        {
            List<Office> OfficeNameSearch = _context.Offices.ToList();
            if (!string.IsNullOrWhiteSpace(name))
            {
                OfficeNameSearch = OfficeNameSearch.Where(p => p.Name.Contains(name)).ToList();
            }
            if (status.HasValue)
            {
                OfficeNameSearch = OfficeNameSearch.Where(p => p.Status == status).ToList();
            }
            if (OfficeNameSearch.ToList().Count > 0)
            {
                return Ok(OfficeNameSearch);
            }
            else
            {
                return BadRequest("kayıt bulunamadı");
            }
        }
    }
}
