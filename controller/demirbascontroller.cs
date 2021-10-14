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
        public void POST([FromQuery]string ad,int ofis,Boolean Durum)
        {
            Demirba d = new Demirba();
            d.Ad=ad;
            d.Durum=Durum;
            d.BulunduguOfis=ofis;
            _demirbascontext.Demirbas.Add(d);
            _demirbascontext.SaveChanges();
        }
        [HttpDelete]
        public void demirbasDelete([FromQuery]int id1)
        {
            var demirbassil = _demirbascontext.Demirbas.SingleOrDefault(p => p.Id == id1);
            _demirbascontext.Demirbas.Remove(demirbassil);
            _demirbascontext.SaveChanges();
        }
        [HttpPut]
        public void demirbasPut(int id2, [FromBody][FromQuery] Demirba güncelle)
        {
            var demirbasgüncelle = _demirbascontext.Demirbas.FirstOrDefault(p => p.Id== id2);
            demirbasgüncelle.Id = güncelle.Id;
            


            _demirbascontext.SaveChanges();
            return;

        }
        [HttpGet("{Id:int}")]
        public Demirba demirbasGetid(int Id)
        {
            return _demirbascontext.Demirbas.Where(p => p.Id == Id).FirstOrDefault();
            



        }
        [HttpGet]
        public IList<Demirba> demirbasGet([FromQuery]string name2){

        var demirbasadsorgula = _demirbascontext.Demirbas.Where(p=>p.Ad.Contains(name2)).ToList();
        

        var demirbaslarigetir= _demirbascontext.Demirbas.ToList();
        
        if (name2 == null){
            return demirbaslarigetir;
        }
            return demirbasadsorgula;
        }
    }
    }
