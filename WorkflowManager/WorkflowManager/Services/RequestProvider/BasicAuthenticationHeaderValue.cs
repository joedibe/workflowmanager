using System.Net.Http.Headers;

namespace WorkflowManager.Services.RequestProvider
{
    internal class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        public BasicAuthenticationHeaderValue(string scheme, string parameter) : base(scheme, parameter) {}
    }
}