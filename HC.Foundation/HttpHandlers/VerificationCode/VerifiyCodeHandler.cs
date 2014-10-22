using System.Web;
using System.Web.SessionState;
using HC.Foundation.HttpHandlers.VerificationCode;

namespace HC.Foundation.HttpHandlers
{
    public class VerifiyCodeHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            new Validate().OutPutValidate();
        }

        public bool IsReusable { get; private set; }
    }
}