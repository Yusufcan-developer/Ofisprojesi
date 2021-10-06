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
    [Route("api/ofisler")]
    [ApiController]
    public class ofiscontroller : ControllerBase
    {
        private OfisProjesiContext _ofiscontext;
        public ofiscontroller(OfisProjesiContext context)
        {
            _ofiscontext = context;
        }
        [HttpPost]
        public ofi Post1([FromBody][FromQuery] ofi yenikayit1)
        {
            _ofiscontext.ofis.Add(yenikayit1);
            _ofiscontext.SaveChanges();
            return yenikayit1;

        }

        [HttpDelete]
        public void Delete1([FromQuery]int id1)
        {
            var DeletedUser = _ofiscontext.ofis.SingleOrDefault(p => p.Ofisid == id1);
            _ofiscontext.ofis.Remove(DeletedUser);
            _ofiscontext.SaveChanges();
        }
        [HttpPut]
        public void Put1(int id2, [FromBody][FromQuery] ofi yenikayit2)
        {
            var updateuser = _ofiscontext.ofis.FirstOrDefault(p => p.Ofisid== id2);
            updateuser.OfisIsim = yenikayit2.OfisIsim;
            


            _ofiscontext.SaveChanges();
            return;

        }
        [HttpGet("{userId2:int}")]
        public ofi GetUserById1(int userId2)
        {
            return _ofiscontext.ofis.Where(p => p.Ofisid == userId2).FirstOrDefault();
            



        }
        [HttpGet]
        public IList<ofi> GetOfis([FromQuery]string name2){

        var listin= (_ofiscontext.ofis.Where(p=>p.OfisIsim==name2).ToList());
        

        var kosul= _ofiscontext.ofis.ToList();
        
        if (name2 == null){
            return kosul;
        }

            return listin;
        }
    }
    }
