using Application.Common.Test;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class TestController : BaseController
    {
        [HttpPost("result")]
        public async Task<string> GetResult([FromBody]Test.Query request)
        {
            await Mediator.Send(request);
            return "Zich";
        }
    }
}
