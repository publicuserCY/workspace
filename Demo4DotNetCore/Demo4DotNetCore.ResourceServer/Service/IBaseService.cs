using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.RequestModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Service
{
    public interface IBaseService<M, T> where M : BaseRequestModel
    {
        Task<T> Single(M model);
        Task<T> Add(M model);
        Task<T> Modify(M model);
        Task<T> Delete(M model);
    }
}