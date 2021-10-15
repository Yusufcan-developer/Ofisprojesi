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
        public void POST([FromQuery] string ad, int ofis, Boolean Durum)
        {
            Demirba d = new Demirba();
            d.Ad = ad;
            d.Durum = Durum;
            d.OfisId = ofis;
            _demirbascontext.Demirbas.Add(d);
            _demirbascontext.SaveChanges();
        }
        [HttpDelete]
        public void Delete([FromQuery] int id)
        {
            var delete = _demirbascontext.Demirbas.SingleOrDefault(p => p.Id == id);
            _demirbascontext.Demirbas.Remove(delete);
            _demirbascontext.SaveChanges();
        }
        [HttpPut]
        public void Put(int id, [FromQuery] string ad , Boolean Durum ,int ofis)
        {
            var update = _demirbascontext.Demirbas.FirstOrDefault(p => p.Id == id);
            update.Ad =ad;
            update.Durum=Durum;
            update.OfisId=ofis;



            _demirbascontext.SaveChanges();
            return;

        }
        [HttpGet("{Id:int}")]
        public Demirba GetDemirbasById(int Id)
        {
            return _demirbascontext.Demirbas.Where(p => p.Id == Id).FirstOrDefault();




        }
        [HttpGet]
        public IList<Demirba> Get([FromQuery] string name)
        {

            var sorgula = _demirbascontext.Demirbas.Where(p => p.Ad.Contains(name)).ToList();


            var AllDataGet = _demirbascontext.Demirbas.ToList();

            if (name == null)
            {
                return AllDataGet;
            }
            return sorgula;
        }
    }
}
