using Demo4OAuth.AuthorizationServer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo4OAuth.AuthorizationServer.Service
{
    public interface IUserService
    {
        Task<List<User>> GetUsers(UserRequestModel model);
        Task<User> GetSingleUser(UserRequestModel model);
    }
}