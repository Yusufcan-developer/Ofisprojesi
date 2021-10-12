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
           public IList<Zimmet> GetZimmets([FromQuery]int name2){

        
        

        var limit= _Zcontext.Zimmets.ToList();
        
        {
           return limit;

        }
       }
       [HttpPost]
        public void postzimmet( [FromQuery]Zimmet zimmet){

        Calisan c = new Calisan();
        Zimmet z = new Zimmet();
        var zimmetlenen =_Zcontext.Calisans.Where(p=>p.Calisanid==zimmet.ZimmetlenenKisi).FirstOrDefault();
        var zimmetdemirbass =_Zcontext.Demirbas.Where(p=>p.Demirbasid==zimmet.DemirbasZimmet).FirstOrDefault();
        z.ZimmetDurumu=true;
        z.ZimmetlenenTarih=DateTime.Today;
        _Zcontext.Zimmets.Add(zimmet);
        _Zcontext.SaveChanges();
}



[HttpDelete]
        public void Delete([FromQuery]int id)
        {
            var Deletedcalisan = _Zcontext.Zimmets.SingleOrDefault(p => p.Zimmetid == id);
            _Zcontext.Zimmets.Remove(Deletedcalisan);
            _Zcontext.SaveChanges();
            return;
        }

 [HttpPut]
        public void calisanPut(int id, [FromBody][FromQuery] Zimmet yenikayit)
        {
            var updateuser = _Zcontext.Zimmets.FirstOrDefault(p => p.Zimmetid== id);
            updateuser.ZimmetlenenKisi= yenikayit.ZimmetlenenKisi;
            updateuser.DemirbasZimmet=yenikayit.DemirbasZimmet;
            updateuser.ZimmetlenenTarih=yenikayit.ZimmetlenenTarih;
            updateuser.ZimmetDurumu=yenikayit.ZimmetDurumu;
            


            _Zcontext.SaveChanges();
            return;

        }
         [HttpGet("{zimmetidsorgu:int}")]
        public List<Zimmet> zimmetsorgusu(int zimmetidsorgu)
        {
         var zimmetidsorgus= _Zcontext.Zimmets.Where(p => p.Zimmetid == zimmetidsorgu).ToList();
            return zimmetidsorgus;



        }
    }
            
            
            
            


        }

     




    


