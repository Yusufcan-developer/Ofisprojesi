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
        public void POST([FromQuery]string ad,int ofis,Boolean Durum,String soyad)
        {
            Calisan c = new Calisan();
            c.Ad=ad;
            c.Soyad=soyad;
            c.Durum=Durum;
            c.BagliOlduguOfis=ofis;
            _context.Calisans.Add(c);
            _context.SaveChanges();
        }

        [HttpDelete]
        public void calisanDelete([FromQuery]int id)
        {
            var DeletedUser = _context.Calisans.SingleOrDefault(p => p.Id == id);
            _context.Calisans.Remove(DeletedUser);
            _context.SaveChanges();
        }
        [HttpPut]
        public void calisanPut([FromQuery] Calisan yenikayit, int id)
        {
            var updateuser = _context.Calisans.FirstOrDefault(p => p.Id==id);
            updateuser.Ad= yenikayit.Ad;
            updateuser.Soyad=yenikayit.Soyad;
            updateuser.Durum=yenikayit.Durum;
            updateuser.BagliOlduguOfis=yenikayit.BagliOlduguOfis;
            


            _context.SaveChanges();
            return;

        }
        [HttpGet("{userId:int}")]
        public Calisan GetUserById(int userId)
        {
            return _context.Calisans.Where(p => p.Id == userId).FirstOrDefault();
            


        
        }
        [HttpGet]
        public IList<Calisan> GetUserByName([FromQuery]string name){


        var listin= (_context.Calisans.Where(p=>p.Ad.Contains(name)).ToList());
        

        var kosul= _context.Calisans.ToList();
        
        if (name == null){
            return kosul;
        }
            return listin;
        }
    }
     
}



    


