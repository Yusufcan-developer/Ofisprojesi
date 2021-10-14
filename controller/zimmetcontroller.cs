using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ofisprojesi
{ 


    [Route("api/zimmetler")]
    [ApiController]
    public class zimmetcontroller : ControllerBase
    {   
        private OfisProjesiContext _Zcontext;
        public zimmetcontroller(OfisProjesiContext context)
        {
            
            _Zcontext = context;
        }
        [HttpGet]
           public IList<Zimmet> Get([FromQuery]int name2){

        
        

        var alldata= _Zcontext.Zimmets.ToList();
        
        {
           return alldata;

        }
       }
     




[HttpDelete]
        public void zimmetDelete([FromQuery]int id)
        {
            var Deletedcalisan = _Zcontext.Zimmets.SingleOrDefault(p => p.Id == id);
            _Zcontext.Zimmets.Remove(Deletedcalisan);
            _Zcontext.SaveChanges();
            return;
        }

 [HttpPut]
        public void zimmetPut(int id, [FromBody][FromQuery] Zimmet güncelleme)
        {
            var updatezimmet = _Zcontext.Zimmets.FirstOrDefault(p => p.Id== id);
            updatezimmet.ZimmetlenenCalisanlar= güncelleme.ZimmetlenenCalisanlar;
            updatezimmet.ZimmetlenmisDemirbas=güncelleme.ZimmetlenmisDemirbas;
            updatezimmet.Tarih=güncelleme.Tarih;
            updatezimmet.Durum=güncelleme.Durum;
            


            _Zcontext.SaveChanges();
            return;

        }
         [HttpGet("{Id:int}")]
        public List<Zimmet> id(int Id)
        {
         var zimmetidsorgusu= _Zcontext.Zimmets.Where(p => p.Id == Id).ToList();
            return zimmetidsorgusu;
        }

        [HttpPost]
        public void zimmetPost(int calisan ,int demirbas,Boolean Durum)
        {   
            Zimmet zimmetlenme=new Zimmet ();
            zimmetlenme.ZimmetlenenCalisanlar=calisan;
            zimmetlenme.ZimmetlenmisDemirbas=demirbas;
            zimmetlenme.Durum=Durum;
            zimmetlenme.Tarih=DateTime.Now;
            
         _Zcontext.Zimmets.Add(zimmetlenme);
         _Zcontext.SaveChanges();
         
        }


    }
}

     




    


