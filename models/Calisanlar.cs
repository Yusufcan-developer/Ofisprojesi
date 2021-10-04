using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace ofisprojesi
{
   [Table("OfisProjesi")]
    public class Calisanlar
    {
        
        [Key]
        public int Calisanid { get;set;}
        public int CalisanAdi { get;set;}
        public int CalisanSoyadi { get;set;}
        
    }
}