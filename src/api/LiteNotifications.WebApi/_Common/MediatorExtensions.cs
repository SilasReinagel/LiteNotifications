using System.Threading.Tasks;
using Carvana;
using LiteMediator;
using Microsoft.AspNetCore.Mvc;

namespace LiteNotifications.WebApi
{
    public static class MediatorExtensions
    {
        public static async Task<IActionResult> Handle<TRequest>(this AsyncMediator requests, TRequest request) 
            => await requests.GetResponse<TRequest, Result>(request).AsResponse();

        public static async Task<IActionResult> AsResponse(this Task<Result> result) 
            => AsResponse(await result);
        
        public static IActionResult AsResponse(this Result result) 
            => new ObjectResult(result) { StatusCode = (int)result.Status };
    }
}
