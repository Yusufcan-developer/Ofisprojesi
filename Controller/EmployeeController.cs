using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Ofisprojesi;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ofisprojesi
{
    // [Authorize]
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private IEmployeeService _EmployeeService;

        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(OfisProjesiContext context, IMapper mapper, IEmployeeService EmployeeService, ILogger<EmployeeController> logger)
        {
            _context = context;
            _mapper = mapper;
            _EmployeeService = EmployeeService;
            _logger = logger;
        }
        ///<summary>
        /// Çalışan kaydetme
        /// </summary>
        /// <return></return>
        [Route("")]
        [HttpPost]
        public ActionResult SaveEmployee([FromBody] EmployeeUpdateDto employeeupdatedto)
        {   try
        {
             DbActionResult Employee = _EmployeeService.SaveEmployee(employeeupdatedto);
            ActionResult result=null;
            switch (Employee)
            {
                case DbActionResult.NameOrLastNameError:
                result=BadRequest(new{isSucces=false,message="isim ve soyisim boş bırakılamaz"});
                break;
                case DbActionResult.OfficeNotFound:
                result=BadRequest(new{isSucces=false,message="OFİS BULUNAMADI!!"});
                break;
                case DbActionResult.Successful:
                result=Ok(new{isSucces=true,message="kayıt başarılı"});
                break;
                default:
                result= BadRequest(new{isSucces=false,message="kayıt başarısız"});
                break;
            }
            return result;
        }
        catch (System.Exception)
        {
            
           _logger.LogError(string.Format("SaveEmployee metodu çalıştırılamadı"), null);
            return BadRequest("bir hata ile karşılaşıldı");
        }
            
        }
        ///<summary>
        /// id ye göre Çalışan Silme
        /// </summary>
        /// <return></return>
        [Route("id")]
        [HttpDelete]
        public ActionResult DeleteByIdEmployee([FromQuery] int? id)
        {
            try
            {
                  if (id==null)
            {
                
                return BadRequest( new{isSucces=false, message="id alanı boş"});
            }
            DbActionResult employee = _EmployeeService.DeleteEmployeById(id);
            ActionResult Result=null;
            switch (employee){
                case DbActionResult.UnknownError:
                Result=BadRequest(new{isSucces=false,message="kişi bulunamadı"});
                break;
                case DbActionResult.HaveDebitError:
                Result=BadRequest(new{isSucces=false,message="çalışan üzerinde zimmetler bulunuyor lütfen önce zimmetleri kaldırın"});
                break;
                case DbActionResult.Successful:
                Result=Ok(new{isSucces=true,message="başarı ile silindi"});
                break;
                default:
                Result=BadRequest(new{isSucces=false,message="silme sırasında hata"});
                break;
            }
            return Result;
            }
            catch (System.Exception)
            {
                _logger.LogError(string.Format("DeleteByIdEmployee metodu çalıştırılamadı"), null);
                return BadRequest("bir hata ile karşılaşıldı");
            }
           
        }
        ///<summary>
        /// id ye göre Çalışan Güncelleme
        /// </summary>
        /// <return></return>
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateEmployee([FromBody] EmployeeUpdateDto employeeupdatedto, int? id,bool? Status)
        {try
        {
             if (employeeupdatedto==null||id==null||Status==null)
            {
                return BadRequest("lütfen boş alanları doldurunuz");
            }
            DbActionResult Employee = _EmployeeService.UpdateEmployee(employeeupdatedto, id,Status);
            ActionResult result=null;
            switch(Employee){
                case DbActionResult.UnknownError:
                result=BadRequest(new{isSucces=false,message="bilinmeyen hata"});
                break;
                case DbActionResult.OfficeNotFound:
                result=BadRequest(new{isSucces=false,message="office bulunamadı"});
                break;
                case DbActionResult.HaveDebitError:
                result=BadRequest(new{isSucces=false,message="üzerinde zimmet olan kişiler işten çıkarılamaz"});
                break;
                case DbActionResult.Successful:
                result=Ok(new{isSucces=true,message="güncelleme başarılı"});
                break;
                default:
                result=BadRequest(new{isSucces=false,message="bilinmeyen hata"});
                break;
            }
            return result;
        }
        catch (System.Exception)
        {
            _logger.LogError(string.Format("UpdateEmployee metodu çalıştırılamadı"), null);
            return BadRequest(new {isSucces=false , Message= "bir hata ile karşılaşıldı"});
            
        }
            
        }
        ///<summary>
        /// Çalışanı id sine Göre Getir
        /// </summary>
        /// <return></return>
        [HttpGet("{id}", Name = "Get-Employee-By-Id")]
        public ActionResult GetEmployeeById(int? id)
        {
            try
            {
            EmployeeDto employee = _EmployeeService.GetEmployeeById(id);
            if (employee != null)
            {
                return Ok(new {isSucces=true ,message= employee});
            }
            else
            {
                return BadRequest(new {isSucces=false ,message="id bulunamadı"});
            }
            }
            catch (System.Exception)
            {
                _logger.LogError(string.Format("GetEmployeeById metodu çalıştırılamadı"), null);
                return BadRequest(new {isSucces=false ,message="bir hata ile karşılaşıldı"});
            }
            

            
        }
        ///<summary>
        /// isme ve duruma göre çalışan sorgulama
        /// </summary>
        /// <return></return>
        [Route("")]
        [HttpGet]
        public ActionResult<EmployeeDto> GetEmployeeByName([FromQuery] string name, bool? status)
        {
            try
            {
            EmployeeDto[] employee = _EmployeeService.GetEmployeeByName(name, status);
            if (employee == null)
            {
                return BadRequest(new{isSucces=false ,message="çalışan bulunamadı"});
            }
            else
            {
                return Ok(new{isSucces=true , message= employee});
            }
            }
            catch (System.Exception)
            {
                
                _logger.LogError(string.Format("GetEmployeeByName metodu çalıştırılamadı"), null);
                return BadRequest(new {isSucces=false ,message="bir hata ile karşılaşıldı"});
            }
            
        }
        /// <summary>
        /// "belirli alanları güncelle"
        /// </summary>
        /// <returns></returns>
        // [AllowAnonymous]
        [HttpPatch]
        public ActionResult<EmployeeUpdateDto> UpdatePatch(int id, [FromBody] JsonPatchDocument<Employee> name)
        {
            try
            {
            Employee employee = _EmployeeService.UpdatePatch(id, name);
            if (employee == null)
            {
                return BadRequest(new{isSucces=false,message="güncellemede hata var"});
            }
            else
            {
                return Ok(new{isSucces=true,message="başarılı güncelleme"});
            }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, string.Format("UpdatePatch metodu çalıştırılamadı"), null);
                return BadRequest(new {isSucces=false ,message="bir hata ile karşılaşıldı"});
            }
            
        }
    }
}









