using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ofisprojesi
{ 
    [Route("api/demirbaslar")]
    [ApiController]
    public class demirbascontroller : ControllerBase
    {
        private OfisProjesiContext _demirbascontext;
        public demirbascontroller(OfisProjesiContext context)
        {
            _demirbascontext = context;
        }
        [HttpPost]
        public Demirba demirbasPost1([FromBody][FromQuery] Demirba yenikayit1)
        {
            _demirbascontext.Demirbas.Add(yenikayit1);
            _demirbascontext.SaveChanges();
            return yenikayit1;

        }

        [HttpDelete]
        public void demirbasDelete1([FromQuery]int id1)
        {
            var DeletedUser = _demirbascontext.Demirbas.SingleOrDefault(p => p.Demirbasid == id1);
            _demirbascontext.Demirbas.Remove(DeletedUser);
            _demirbascontext.SaveChanges();
        }
        [HttpPut]
        public void demirbasPut1(int id2, [FromBody][FromQuery] Demirba yenikayit2)
        {
            var updateuser = _demirbascontext.Demirbas.FirstOrDefault(p => p.Demirbasid== id2);
            updateuser.DemirbasAdi = yenikayit2.DemirbasAdi;
            


            _demirbascontext.SaveChanges();
            return;

        }
        [HttpGet("{userId2:int}")]
        public Demirba GetUserById1(int userId2)
        {
            return _demirbascontext.Demirbas.Where(p => p.Demirbasid == userId2).FirstOrDefault();
            



        }
        [HttpGet]
        public IList<Demirba> GetDemirbas([FromQuery]string name2,int pageindex2,int pagecount2){

            int page=1;
            int recordPage=10;
            

        var listin= (_demirbascontext.Demirbas.Where(p=>p.DemirbasAdi.Contains(name2)).Skip(recordPage* (page - 1)).Take(recordPage).ToList());
        

        var kosul= _demirbascontext.Demirbas.Skip(recordPage*(page - 1 )).Take(recordPage).ToList();
        
        if (name2 == null){
            return kosul;
        }
            return listin;
        }
    }
    }
