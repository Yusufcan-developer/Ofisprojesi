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
        public Ofi ofisPost([FromBody][FromQuery] Ofi ekle)
        {
            _ofiscontext.Ofis.Add(ekle);
            _ofiscontext.SaveChanges();
            return ekle;

        }

        [HttpDelete]
        public void ofisDelete([FromQuery]int id)
        {
            var ofissil = _ofiscontext.Ofis.SingleOrDefault(p => p.Id == id);
            _ofiscontext.Ofis.Remove(ofissil);
            _ofiscontext.SaveChanges();
        }
        [HttpPut]
        public void ofisPut(int id2, [FromBody][FromQuery] Ofi güncelleme)
        {
            var ofisgüncelleme = _ofiscontext.Ofis.FirstOrDefault(p => p.Id== id2);
            ofisgüncelleme.Ad = güncelleme.Ad;
            


            _ofiscontext.SaveChanges();
            return;

        }
        [HttpGet("{Id:int}")]
        public Ofi ofisidGet(int Id)
        {
            return _ofiscontext.Ofis.Where(p => p.Id == Id).FirstOrDefault();
            



        }
        [HttpGet]
        public IList<Ofi> ofisGet([FromQuery]string name2){

        var calisanisimara= (_ofiscontext.Ofis.Where(p=>p.Ad==name2).ToList());
        

        var tümcalisanlar= _ofiscontext.Ofis.ToList();
        
        if (name2 == null){
            return tümcalisanlar;
        }

            return calisanisimara;
        }
    }
    }
