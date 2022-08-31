using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TwitterWebAPI1.Data;
using TwitterWebAPI1.Dtos;
using TwitterWebAPI1.Model;

namespace TwitterWebAPI1.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly TwitterAppDataContext _dataContext;

        public IConfiguration _configuration { get; }

        public UserService(TwitterAppDataContext dataContext, IConfiguration configuration, IMapper mapper)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<bool> CheckUserExists(string userName)
        {
            if (await _dataContext.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower()))
                return true;
            return false;
        }

        public async Task<bool> EmailExists(string email)
        {
            if (await _dataContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
                return true;
            return false;
        }

        public async Task<GenericResponse<List<UserListDto>>> GetAllUsers()
        {
            GenericResponse<List<UserListDto>> serviceResponse = new GenericResponse<List<UserListDto>>();
            serviceResponse.Data = await _dataContext.Users.Select(x => _mapper.Map<UserListDto>(x)).ToListAsync();
            return serviceResponse;
        }

        public async Task<GenericResponse<UserListDto>> GetByUserName(string userName)
        {
            GenericResponse<UserListDto> response = new GenericResponse<UserListDto>();
            var users = await _dataContext.Users.Where(u => u.UserName.Contains(userName)).ToListAsync();
            if (users.Any())
            {
                response.Success = true;
                response.Data = _mapper.Map<UserListDto>(users);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
        }

        public async Task<GenericResponse<string>> Login(string userName, string password)
        {
            var response = new GenericResponse<string>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Success = true;
                response.Message = "Login successful";
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<GenericResponse<int>> Register(UserMaster user, string password)
        {
            GenericResponse<int> response = new GenericResponse<int>();

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            if (await CheckUserExists(user.UserName))
            {
                response.Success = false;
                response.Message = "User already exists!";
                return response;
            }

            if (await EmailExists(user.Email))
            {
                response.Success = false;
                response.Message = "Email already exists!";
                return response;
            }
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            response.Data = user.Id;
            response.Success = true;
            response.Message = "User registerd successfully.";
            return response;
        }

        public async Task<GenericResponse<List<UserListDto>>> SearchByUserName(string userName)
        {
            GenericResponse<List<UserListDto>> serviceResponse = new GenericResponse<List<UserListDto>>();
            serviceResponse.Data = await _dataContext.Users.Where(x => x.UserName.Contains(userName)).Select(x => _mapper.Map<UserListDto>(x)).ToListAsync();
            return serviceResponse;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA384())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA384(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(UserMaster user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretToken").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
