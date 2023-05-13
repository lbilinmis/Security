using Microsoft.Extensions.Options;
using System.Net;

namespace Security.WhiteBlackList.WebUI.Middlewares
{
    public class IPSafeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPList _iplist;

        public IPSafeMiddleware(RequestDelegate next, IOptions<IPList> ipList)
        {
            this._next = next;
            this._iplist = ipList.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestIpAdress=httpContext.Connection.RemoteIpAddress;
            var whiteList=_iplist.WhiteList.Where(x=>IPAddress.Parse(x).Equals(requestIpAdress)).Any();

            if (!whiteList)
            {
                //kullanıcı o sayfaya erişemez
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden; 
                return;
            }

            await _next(httpContext);
        }
    }
}
