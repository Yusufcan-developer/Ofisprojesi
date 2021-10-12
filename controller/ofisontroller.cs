using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

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
        public Ofi ofisPost([FromBody][FromQuery] Ofi yenikayit1)
        {
            _ofiscontext.Ofis.Add(yenikayit1);
            _ofiscontext.SaveChanges();
            return yenikayit1;

        }

        [HttpDelete]
        public void ofisDelete1([FromQuery]int id1)
        {
            var DeletedUser = _ofiscontext.Ofis.SingleOrDefault(p => p.Ofisid == id1);
            _ofiscontext.Ofis.Remove(DeletedUser);
            _ofiscontext.SaveChanges();
        }
        [HttpPut]
        public void ofisPut1(int id2, [FromBody][FromQuery] Ofi yenikayit2)
        {
            var updateuser = _ofiscontext.Ofis.FirstOrDefault(p => p.Ofisid== id2);
            updateuser.OfisIsim = yenikayit2.OfisIsim;
            


            _ofiscontext.SaveChanges();
            return;

        }
        [HttpGet("{userId2:int}")]
        public Ofi GetUserById1(int userId2)
        {
            return _ofiscontext.Ofis.Where(p => p.Ofisid == userId2).FirstOrDefault();
            



        }
        [HttpGet]
        public IList<Ofi> GetOfis([FromQuery]string name2){

        var listin= (_ofiscontext.Ofis.Where(p=>p.OfisIsim==name2).ToList());
        

        var kosul= _ofiscontext.Ofis.ToList();
        
        if (name2 == null){
            return kosul;
        }

            return listin;
        }
    }
    }
