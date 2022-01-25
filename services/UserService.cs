using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ofisprojesi;

namespace ofisprojesi{

       public interface IUserService
    {
        UserDto Authenticate(string username, string password);
        List<UserUpdateDto> GetUsers();
        UserDto GetUserById(int userId);
        IList<RoleDto> GetRoles();
        RoleDto GetRoleById(int roleId);
        RoleDto GetRoleByName(string roleName);
        bool SaveUser(UserUpdateDto userDto, int updateUserId, out int userId, out string message);
        bool DeleteUserById(int userId);
        bool ChangePassword(string username, string oldPassword, string newPassword);
        string EncodePassword(string password, string passwordKey);
        string GetPasswordKey();
        DbActionResult UpdateDto(UserUpdateDto userUpdateDto, int? id);
    }

    public class UserService : IUserService
    {
        private OfisProjesiContext _context;
        private IServiceProvider _provider;
        private readonly AppSettings _appSettings;
        private IMapper _mapper;

        public UserService(IServiceProvider provider,OfisProjesiContext context, IOptionsSnapshot<AppSettings> appSettings, IMapper mapper)
        {
            _context=context;
            _provider = provider;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public UserDto Authenticate(string username, string password)
        {
            User user = _context.Users.Include(it => it.Role).SingleOrDefault(item => item.Username == username && item.IsLocked == false);
            // return null if user not found
            if (user == null)
            {
                return null;
            }
            if (user != null)
            {
                //userId = user.Id;
                String pwd = user.Password;
                String passwordKey = user.PasswordKey;

                if (CheckPassword(password, pwd, passwordKey))
                {
                    // isValid = true;
                    // user.SonGirisTrh = DateTime.Now;
                    // user.SonPingTarihi = DateTime.Now;
                    // dbContext.SaveChanges();
                }
                else
                {
                    return null;
                }
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("uname", user.Username),
                    new Claim("urole", user.Role.Name ?? "N/A"),
                    new Claim("uid", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // user.Token = tokenHandler.WriteToken(token);
            
            UserDto userDto = _mapper.Map<UserDto>(user);
            userDto.Token = tokenHandler.WriteToken(token); 

            return userDto;
        }


        public List<UserUpdateDto> GetUsers()
        {
            List<User> userDto = _context.Users.ToList();
            List<UserUpdateDto> userUpdateDtos = _mapper.Map<List<UserUpdateDto>>(userDto);
            return userUpdateDtos;
        }

        public UserDto GetUserById(int userId)
        {
            UserDto u = _mapper.Map<User, UserDto>(_context.Users.Include(it => it.Role).SingleOrDefault(it => it.Id == userId));
            return u;
        }

        public IList<RoleDto> GetRoles()
        {
            return _mapper.Map<IList<Role>, IList<RoleDto>>(_context.Roles.OrderBy(it => it.Description).ToList());
        }

        public RoleDto GetRoleById(int roleId)
        {
            return _mapper.Map<Role, RoleDto>(_context.Roles.SingleOrDefault(it => it.Id == roleId));
        }

        public RoleDto GetRoleByName(string roleName)
        {
            return _mapper.Map<Role, RoleDto>(_context.Roles.SingleOrDefault(it => it.Name == roleName));
        }

        public bool SaveUser(UserUpdateDto userDto, int updateUserId, out int userId, out string message)
        {userId = -1;
            message = null;

            if (userDto == null)
            {
                message = "userDto null olamaz";
                return false;
            }
            if (string.IsNullOrWhiteSpace(userDto.Username))
            {
                message = "userDto.Username alanı boş ya da null olamaz";
                return false;
            }
            if (string.IsNullOrWhiteSpace(userDto.FirstName))
            {
                message = "userDto.FirstName alanı boş ya da null olamaz";
                return false;
            }
            if (string.IsNullOrWhiteSpace(userDto.LastName))
            {
                message = "userDto.LastName alanı boş ya da null olamaz";
                return false;
            }
            if (userDto.RoleId == null || userDto.RoleId <= 0)
            {
                message = "userDto.RoleId geçerli değil";
                return false;
            }
            Role role = _context.Roles.Find(userDto.RoleId);
            if (role == null)
            {
                message = string.Format("userDto.RoleId ({0}) geçersiz", userDto.RoleId);
                return false;
            }
            // int roleId = -1;
            // if (userDto.Role.Id > 0)
            // {
            //     Role role = _kryContext.Roles.Find(userDto.Role.Id);
            //     if (role == null)
            //     {
            //         message = string.Format("userDto.Role.Id ({0}) geçersiz", userDto.Role.Id);
            //         return false;
            //     }
            //     roleId = role.Id;
            // }
            // else if (string.IsNullOrWhiteSpace(userDto.Role.RoleName))
            // {
            //     Role role = _kryContext.Roles.SingleOrDefault(it => it.RoleName == userDto.Role.RoleName);
            //     if (role == null)
            //     {
            //         message = string.Format("userDto.Role.RoleName ({0}) geçersiz", userDto.Role.RoleName);
            //         return false;
            //     }
            //     roleId = role.Id;
            // }
            // else
            // {
            //     message = "userDto.Role geçerli Id ya da RoleName'e sahip olmalı";
            //     return false;
            // }

            User user;
            if (userDto.Id > 0)
            {
                user = _context.Users.Include(it => it.Role).SingleOrDefault(it => it.Id == userDto.Id);
                if (user == null)
                {
                    message = string.Format("userDto.Id ({0}) geçersiz", userDto.Id);
                    return false;
                }
                if (_context.Users.Any(it => it.Username == userDto.Username && it.Id != userDto.Id))
                {
                    message = string.Format("userDto.Username ({0}) başka bir kullanıcı tarafından kullanılıyor", userDto.Username);
                    return false;
                }
                user.Username = userDto.Username;
            }
            else
            {
                user = _context.Users.Include(it => it.Role).SingleOrDefault(it => it.Username == userDto.Username);
                if (user == null)
                {
                    user = new User();
                    user.Username = userDto.Username;
                    string passwordKey = GetPasswordKey();
                    string encodedPassword = EncodePassword("123456", passwordKey);
                    user.PasswordKey = passwordKey;
                    user.Password = encodedPassword;
                    _context.Users.Add(user);

                }
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.RoleId = (int)userDto.RoleId;
            user.IsLocked = userDto.IsLocked;
            if (user.Id > 0)
            {
                user.CreateDate = DateTime.Now;
                user.CreateUserId = updateUserId > 0 ? updateUserId : (int?)null;
            }
            else
            {
                user.LastUpdateDate = DateTime.Now;
                user.LastUpdateUserId = updateUserId > 0 ? updateUserId : (int?)null;
            }
            try
            {
                _context.SaveChanges();
                userId = user.Id;
                message = "Kayıt başarılı";
                return true;
            }
            catch (Exception)
            {
                message = "Kayıt sırasında bilinmeyen hata oluştu";
                return false;
            }
        }

        public bool DeleteUserById(int userId)
        {
            try
            {
                User user = _context.Users.Find(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return false;
            }

            try
            {
                User user = _context.Users.SingleOrDefault(it => it.Username == username);
                if (user == null) { return false; }

                if (!string.IsNullOrWhiteSpace(oldPassword))
                {
                    if (!CheckPassword(oldPassword, user.Password, user.PasswordKey))
                    {
                        return false;
                    }
                }

                string encodedPassword = EncodePassword(newPassword, user.PasswordKey);
                user.Password = encodedPassword;
                _context.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        #region Utility Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="dbPassword"></param>
        /// <param name="passwordKey"></param>
        /// <returns></returns>
        private bool CheckPassword(string password, string dbPassword, string passwordKey)
        {
            password = EncodePassword(password, passwordKey);
            return password == dbPassword;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordKey"></param>
        /// <returns></returns>
        public string EncodePassword(string password, string passwordKey)
        {
            if (password == null)
            {
                return null;
            }

            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] keyBytes = Convert.FromBase64String(passwordKey);
            byte[] keyedBytes = new byte[passwordBytes.Length + keyBytes.Length];
            Array.Copy(keyBytes, keyedBytes, keyBytes.Length);
            Array.Copy(passwordBytes, 0, keyedBytes, keyBytes.Length, passwordBytes.Length);

            HashAlgorithm hash = HashAlgorithm.Create("SHA1");
            return Convert.ToBase64String(hash.ComputeHash(keyedBytes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPasswordKey()
        {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            byte[] key = new byte[16];
            cryptoProvider.GetBytes(key);
            return Convert.ToBase64String(key);
        }
        #endregion
        public DbActionResult UpdateDto(UserUpdateDto userUpdateDto, int? id)
        {
            if (userUpdateDto == null || id == null)
            {
                return DbActionResult.UnknownError;
            }
            User user = _context.Users.FirstOrDefault(p => p.Id == id);
            if (user == null)
            {
                return DbActionResult.OfficeNotFound;
            }
            user.FirstName = userUpdateDto.FirstName;
            user.Username=userUpdateDto.Username;
            user.LastUpdateDate=DateTime.Now;
            user.Email = userUpdateDto.Email;
            user.LastName = userUpdateDto.LastName;
            _context.SaveChanges();
            return (DbActionResult.Successful);
        }
    }
}
    


