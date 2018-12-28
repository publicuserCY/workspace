using Demo4OAuth.AuthorizationServer.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4OAuth.AuthorizationServer.Service
{
    public class UserService : IUserService
    {
        private AccountContext context;
        public UserService(AccountContext context)
        {
            this.context = context;
        }
        public Task<List<User>> GetUsers(UserRequestModel model)
        {
            var result = context.Users.ToList();
            return Task.FromResult(result);
        }

        public Task<User> GetSingleUser(UserRequestModel model)
        {
            var result = context.Users.SingleOrDefault(p => p.Id == model.Id && p.Password == model.Password);
            return Task.FromResult(result);
        }
    }
}