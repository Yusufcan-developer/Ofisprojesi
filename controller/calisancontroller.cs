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
        public Calisan calisanPost([FromBody][FromQuery] Calisan yenikayit)
        {
            _context.Calisans.Add(yenikayit);
            _context.SaveChanges();
            return yenikayit;

        }

        [HttpDelete]
        public void calisanDelete([FromQuery]int id)
        {
            var DeletedUser = _context.Calisans.SingleOrDefault(p => p.Id == id);
            _context.Calisans.Remove(DeletedUser);
            _context.SaveChanges();
        }
        [HttpPut]
        public void calisanPut(int id, [FromBody][FromQuery] Calisan yenikayit)
        {
            var updateuser = _context.Calisans.FirstOrDefault(p => p.Id== id);
            updateuser.Ad= yenikayit.Ad;
            updateuser.Ad=yenikayit.Ad;
            


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



    


