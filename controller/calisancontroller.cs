using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ofisprojesi
{


    [Route("api/calisanlar")]
    [ApiController]
    public class calisankontrol : ControllerBase
    {
        private OfisProjesiContext _context;
        public calisankontrol(OfisProjesiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public void POST([FromQuery] string ad, int ofis, Boolean Durum, String soyad)
        {
            Calisan calisan = new Calisan();
            calisan.Ad = ad;
            calisan.Soyad = soyad;
            calisan.Durum = Durum;
            calisan.OfisId = ofis;
            _context.Calisans.Add(calisan);
            _context.SaveChanges();
        }

        [HttpDelete]
        public void Delete([FromQuery] int id)
        {
            var DeletedUser = _context.Calisans.SingleOrDefault(p => p.Id == id);
            _context.Calisans.Remove(DeletedUser);
            _context.SaveChanges();
        }
        [HttpPut]
        public void Put([FromQuery] string ad, int id , string soyad,Boolean Durum,int ofis)
        {
            var update = _context.Calisans.FirstOrDefault(p => p.Id == id);
            update.Ad = ad;
            update.Soyad = soyad;
            update.Durum = Durum;
            update.OfisId = ofis;



            _context.SaveChanges();
            return;

        }
        [HttpGet("{userId:int}")]
        public Calisan GetUserById(int userId)
        {
            return _context.Calisans.Where(p => p.Id == userId).FirstOrDefault();




        }
        [HttpGet]
        public IList<Calisan> GetUserByName([FromQuery] string name)
        {


            var SearchName = (_context.Calisans.Where(p => p.Ad.Contains(name)).ToList());


            var GetAllData = _context.Calisans.ToList();

            if (name == null)
            {
                return GetAllData;
            }
            return SearchName;
        }
    }

}






