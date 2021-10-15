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
        public void Post([FromQuery] String ad, Boolean Durum)
        {
            Ofi O = new Ofi();
            O.Ad = ad;
            O.Durum = Durum;
            _ofiscontext.Ofis.Add(O);
            _ofiscontext.SaveChanges();


        }

        [HttpDelete]
        public void Delete([FromQuery] int id)
        {
            var ofissil = _ofiscontext.Ofis.SingleOrDefault(p => p.Id == id);
            _ofiscontext.Ofis.Remove(ofissil);
            _ofiscontext.SaveChanges();
        }
        [HttpPut]
        public void Put(int id,[FromQuery]string ad , Boolean Durum)
        {
            var ofisgüncelleme = _ofiscontext.Ofis.FirstOrDefault(p => p.Id == id);
           ofisgüncelleme.Ad=ad;
           ofisgüncelleme.Durum=Durum;



            _ofiscontext.SaveChanges();
            return;

        }
        [HttpGet("{Id:int}")]
        public Ofi idGet(int Id)
        {
            return _ofiscontext.Ofis.Where(p => p.Id == Id).FirstOrDefault();




        }
        [HttpGet]
        public IList<Ofi> Get([FromQuery] string name)
        {

            var calisanisimara = (_ofiscontext.Ofis.Where(p => p.Ad == name).ToList());


            var tümcalisanlar = _ofiscontext.Ofis.ToList();

            if (name == null)
            {
                return tümcalisanlar;
            }

            return calisanisimara;
        }
    }
}
