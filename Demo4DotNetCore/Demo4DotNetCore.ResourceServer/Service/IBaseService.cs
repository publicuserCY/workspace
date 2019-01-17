using Demo4DotNetCore.ResourceServer.Model;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Service
{
    public interface IBaseService<M, T> where M : BaseRequestModel
    {
        Task<T> Single(M model);
        Task<T> Add(M model);
        Task<T> Modify(M model);
        Task<T> Delete(M model);
    }
}