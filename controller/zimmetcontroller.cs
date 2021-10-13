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
           public IList<Zimmet> zimmetGet([FromQuery]int name2){

        
        

        var limit= _Zcontext.Zimmets.ToList();
        
        {
           return limit;

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
    }
            
            
            
            


        }

     




    


