using System.Threading.Tasks;
using Carvana;
using LiteMediator;
using Microsoft.AspNetCore.Mvc;

namespace LiteNotifications.WebApi._Common
{
    public static class MediatorExtensions
    {
        public static async Task<IActionResult> Handle<TRequest>(this AsyncMediator requests, TRequest request)
        {
            return await requests.GetResponse<TRequest, Result>(request).AsResponse();
        }
        
        public static async Task<IActionResult> AsResponse(this Task<Result> result)
        {
            var completedResult = await result;
            return new ObjectResult(completedResult) { StatusCode = (int)completedResult.Status };
        }
    }
}
