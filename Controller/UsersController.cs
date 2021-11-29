using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofisprojesi;
using Swashbuckle.AspNetCore.Annotations;

namespace ofisprojesi{
    
    [Authorize]
    [ApiController]
    [Route("api/security")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Kullanıcı doğrulama
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult<UserDto> Authenticate([FromBody] LoginDto login)
        {
            var user = _userService.Authenticate(login.username, login.Password);

            if (user == null)
                return BadRequest(new { IsSuccess = false, Message = "Kullanıcı adı ya da parola hatalı" });

            return Ok(user);
        }
        /// <summary>
        /// Kullanıcı arama
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpPost("users/search")]
        public ActionResult<UserDto> GetUsers([FromBody] UserSearchCriteria criteria)
        {
            Claim userRoleClaim = User.Claims.SingleOrDefault(it => it.Type == "urole");
            if (userRoleClaim == null)
            {
                return BadRequest(new { IsSuccess = false, Message = "Erişim yetkiniz yok!" });
            }

            UserDto list = _userService.GetUsers(criteria);

            return Ok(list);
        }
        /// <summary>
        /// Kullanıcıyı Id ile getirme
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpGet("users/{userId:int}", Name = "GetUserById")]
        public ActionResult<UserDto> GetUserById(int userId)
        {
            Claim userRoleClaim = User.Claims.SingleOrDefault(it => it.Type == "urole");
            if (userRoleClaim == null)
            {
                return BadRequest(new { IsSuccess = false, Message = "Erişim yetkiniz yok!" });
            }

            UserDto user = _userService.GetUserById(userId);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound(new { IsSuccess = false, Message = "Kullanıcı bulunamadı!" });
            }
        }
        /// <summary>
        /// Geçerli Kullanıcıyı getirme
        /// </summary>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpGet("users/default-user", Name = "GetDefaultUser")]
        public ActionResult<UserDto> GetDefaultUser()
        {
            Claim userIdClaim = User.Claims.SingleOrDefault(it => it.Type == "uid");
            if (userIdClaim == null)
            {
                return BadRequest(new { IsSuccess = false, Message = "Erişim yetkiniz yok!" });
            }

            UserDto user = _userService.GetUserById(int.Parse(userIdClaim.Value));

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound(new { IsSuccess = false, Message = "Kullanıcı bulunamadı!" });
            }
        }
        /// <summary>
        /// Tüm roller
        /// </summary>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpGet("roles")]
        public ActionResult<IList<RoleDto>> GetRoles()
        {
            var l = _userService.GetRoles();

            return Ok(l);
        }
        /// <summary>
        /// Rolü Id ile getirme
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpGet("roles/id/{roleId:int}")]
        public ActionResult<RoleDto> GetRoleById(int roleId)
        {
            var role = _userService.GetRoleById(roleId);
            if (role != null)
            {
                return Ok(role);
            }
            else
            {
                return NotFound(new { IsSuccess = false, Message = "Rol bulunamadı!" });
            }
        }
        /// <summary>
        /// Rolü adıyla getirme
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpGet("roles/name/{roleName}")]
        public ActionResult<RoleDto> GetRoleByName(string roleName)
        {
            var role = _userService.GetRoleByName(roleName);
            if (role != null)
            {
                return Ok(role);
            }
            else
            {
                return NotFound(new { IsSuccess = false, Message = "Rol bulunamadı!" });
            }
        }
        /// <summary>
        /// Kullanıcı kaydetme
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpPost("users")]
        public ActionResult<UserDto> SaveUser([FromBody] UserUpdateDto userDto)
        {
            Claim userRoleClaim = User.Claims.SingleOrDefault(it => it.Type == "urole");
           

            int updateUserId = -1;
            Claim userIdClaim = User.Claims.SingleOrDefault(it => it.Type == "uid");
            if (userIdClaim != null)
            {
                updateUserId = int.Parse(userIdClaim.Value);
            }
            int userId;
            string message;
            bool success = _userService.SaveUser(userDto, updateUserId, out userId, out message);

            if (success)
            {
                UserDto u = _userService.GetUserById(userId);
                return CreatedAtRoute("GetUserById", new { userId = u.Id }, u);
            }
            else
            {
                return BadRequest(new { IsSuccess = false, Message = message });
            }
        }
        /// <summary>
        /// Kullanıcı silme
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpDelete("users/{userId:int}")]
        public ActionResult<bool> DeleteUserById(int userId)
        {
            Claim userRoleClaim = User.Claims.SingleOrDefault(it => it.Type == "urole");
            if (userRoleClaim == null)
            {
                return BadRequest(new { IsSuccess = false, Message = "Erişim yetkiniz yok!" });
            }

            if (_userService.GetUserById(userId) == null)
            {
                return NotFound(new { isSuccess = false, message = "userId ile ilişkili kullanıcı bulumadı" });
            }

            bool result = _userService.DeleteUserById(userId);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { isSuccess = false, message = "Kullanıcı silinirken hata oluştu" });
            }
        }
        /// <summary>
        /// Parola güncelleme
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        [SwaggerOperationAttribute(Tags = new[] { "Kullanıcı İşlemleri" })]
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] changePasswordDto changePasswordDto)
        {
            Claim usernameClaim = User.Claims.SingleOrDefault(it => it.Type == "uname");
            Claim userRoleClaim = User.Claims.SingleOrDefault(it => it.Type == "urole");

            string username = !string.IsNullOrWhiteSpace(changePasswordDto.username) ? changePasswordDto.username : usernameClaim.Value;

            if (userRoleClaim.Value != null)
            {
                if (username != usernameClaim.Value)
                {
                    return BadRequest(new { IsSuccess = false, Message = "Erişim yetkiniz yok! Sadece Admin başka kullanıcının parolasını değiştirebilir." });
                }
                if (string.IsNullOrWhiteSpace(changePasswordDto.oldPassword))
                {
                    return BadRequest(new { IsSuccess = false, Message = "Eski parola boş bırakılamaz!" });
                }
            }

            bool success = _userService.ChangePassword(username, changePasswordDto.oldPassword, changePasswordDto.newPassword);
            if (success)
            {
                return Ok(new { IsSuccess = true, Message = "Parola başarıyla güncellendi" });
            }
            else
            {
                return BadRequest(new { IsSuccess = false, Message = "Parola değiştirme işlemi başarısız!" });
            }
        }
    }
}
