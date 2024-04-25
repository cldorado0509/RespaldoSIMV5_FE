using SIM.Models;
using System.Threading.Tasks;

namespace SIM.Services
{
    public interface IApiService
    {

        Task<Response> GetAsync<T>(string urlBase, string servicePrefix, string controller, string token);

        Task<Response> GetAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller, string token);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, FacebookProfile request);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);

        Task<Response> ModifyUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest, string token);

        Task<Response> GetUserByEMail(string urlBase, string servicePrefix, string controller, UserRequest userRequest, string token);

        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string token);

        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);

        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model);

        Task<ResponseMicroServicio> GetFilteredDataAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetFilteredDataAsync(string urlBase, string servicePrefix, string controller, string token);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlBase"></param>
        /// <param name="servicePrefix"></param>
        /// <param name="controller"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Response> GetMicroServicioListAsync<T>(string urlBase, string servicePrefix, string controller, string token);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlBase"></param>
        /// <param name="servicePrefix"></param>
        /// <param name="controller"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Response> GetMicroServicioAsync<T>(string urlBase, string servicePrefix, string controller, string token);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlBase"></param>
        /// <param name="servicePrefix"></param>
        /// <param name="controller"></param>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Response> PostMicroServicioAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlBase"></param>
        /// <param name="servicePrefix"></param>
        /// <param name="controller"></param>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Response> PutMicroServicioAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlBase"></param>
        /// <param name="servicePrefix"></param>
        /// <param name="controller"></param>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Response> DeleteMicroServicioAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);

    }
}
