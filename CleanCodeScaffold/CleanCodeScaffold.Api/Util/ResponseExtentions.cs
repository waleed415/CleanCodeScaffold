using CleanCodeScaffold.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CleanCodeScaffold.Api.Util
{
    public static class ResponseExtentions
    {
        public static IActionResult ToResponse<T>(this Response<T> response)
        {
            if (response.Status.Equals("Error"))
                return new BadRequestObjectResult(response);
            else
                return new OkObjectResult(response);
        }
    }
}
