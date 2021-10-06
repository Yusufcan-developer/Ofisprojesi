using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ofisprojesi.Models;

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
        public Calisan Post([FromBody][FromQuery] Calisan yenikayit)
        {
            _context.Calisans.Add(yenikayit);
            _context.SaveChanges();
            return yenikayit;

        }

        [HttpDelete]
        public void Delete([FromQuery]int id)
        {
            var DeletedUser = _context.Calisans.SingleOrDefault(p => p.Calisanid == id);
            _context.Calisans.Remove(DeletedUser);
            _context.SaveChanges();
        }
        [HttpPut]
        public void Put(int id, [FromBody][FromQuery] Calisan yenikayit)
        {
            var updateuser = _context.Calisans.FirstOrDefault(p => p.Calisanid== id);
            updateuser.CalisanAdi = yenikayit.CalisanAdi;
            updateuser.CalisanAdi=yenikayit.CalisanAdi;
            


            _context.SaveChanges();
            return;

        }
        [HttpGet("{userId:int}")]
        public Calisan GetUserById(int userId)
        {
            return _context.Calisans.Where(p => p.Calisanid == userId).FirstOrDefault();
            



        }
        [HttpGet]
        public IList<Calisan> GetUserByName([FromQuery]string name,int pageindex,int pagecount){

            int page=1;
            int recordPage=10;
            

        var listin= (_context.Calisans.Where(p=>p.CalisanAdi.Contains(name)).Skip(recordPage* (page - 1)).Take(recordPage).ToList());
        

        var kosul= _context.Calisans.Skip(recordPage*(page - 1 )).Take(recordPage).ToList();
        
        if (name == null){
            return kosul;
        }
            return listin;
        }
    }
     
}



    


