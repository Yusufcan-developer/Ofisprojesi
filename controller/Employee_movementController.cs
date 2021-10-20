using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ofisprojesi.controller

    {[Route("api/employee_movement")]
    [ApiController]
    public class Employee_MovementController : ControllerBase
    {
        private OfisProjesiContext _context;
        public Employee_MovementController(OfisProjesiContext context)
        {
            _context = context;
        }
     //   /// <summary>
    //  /// "calisanin hareketlerini hepsini getir"
    //    /// </summary>
     //   /// <returns></returns>
      //  [Route("GetEmployeeMovementById")]
       // [HttpGet]
     //   public List<Debit> GetEmployeeMovementById ()
      //  {

       //     var Querry = (from item in _context.Debits
         //   join item2 in _context.Employees
           // on item.EmployeeId equals item2.Id select new Debit){
  //              CalisanAd=item2.Ad,
  //              DemirbasAd=item.DemirbasAd,
    //            Tarih=item.Tarih,
      //          Durum=item.Durum
  //          }).ToList();
            // Querry.ToList();
    //         return Querry;
    }
}
   // }
   // }
 //   }