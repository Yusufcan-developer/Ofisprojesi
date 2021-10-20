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
        /// "Ofis Kayrdetme"
        /// </summary>
        /// <returns></returns>
        [Route("SaveOffice")]
        [HttpPost]//ofis yaratma
        public void SaveOffice([FromBody] Office office)
        {
            _context.Offices.Add(office);
            _context.SaveChanges();


        }
        /// <summary>
        /// "Ofis Silme"
        /// </summary>
        /// <returns></returns>
        [Route("DeleteOfficeById")]
        [HttpDelete]//ofis sil
        public void DeleteOfficeById([FromQuery] int id)
        {
            var Delete = _context.Offices.SingleOrDefault(p => p.Id == id);
            _context.Offices.Remove(Delete);
            _context.SaveChanges();
        }
        /// <summary>
        /// "Ofis Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("UpdateOffice")]
        [HttpPut]//ofis güncelle
        public void UpdateOffice([FromBody]Office office ,int id)
        {
            var Update = _context.Offices.FirstOrDefault(p => p.Id == id);
            Update.Name = office.Name;
            Update.Status =office.Status;



            _context.SaveChanges();
            return;

        }
        /// <summary>
        /// "Ofis id ye göre sorgula"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Id:int}")]//ofis id ye sorgulama
        public Office GetOfficeById(int Id)
        {
            return _context.Offices.Where(p => p.Id == Id).FirstOrDefault();




        }
        /// <summary>
        /// "Ofis Adına Göre Sorgula Yoksa Tümünü Getir"
        /// </summary>
        /// <returns></returns>
        [Route("GetOfisByName")]
        [HttpGet]//ofis adına göre sorgula yoksa tümünü getir
        public IList<Office> GetOfisByName([FromQuery] string name)
        {

            var OfficeNameSearch = (_context.Offices.Where(p => p.Name == name).ToList());


            var GetAllOffice = _context.Offices.ToList();

            if (name == null)
            {
                return GetAllOffice;
            }

            return OfficeNameSearch;
        }
    }
}
