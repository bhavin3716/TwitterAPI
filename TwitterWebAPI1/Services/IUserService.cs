using TwitterWebAPI1.Dtos;
using TwitterWebAPI1.Model;

namespace TwitterWebAPI1.Services
{
    public interface IUserService
    {
        Task<GenericResponse<int>> Register(UserMaster user, string password);

        Task<bool> CheckUserExists(string userName);
        Task<bool> EmailExists(string email);

        Task<GenericResponse<string>> Login(string userName, string password);

        Task<GenericResponse<List<UserListDto>>> GetAllUsers();

        Task<GenericResponse<UserListDto>> GetByUserName(string userName);

        Task<GenericResponse<List<UserListDto>>> SearchByUserName(string userName);
    }
}
