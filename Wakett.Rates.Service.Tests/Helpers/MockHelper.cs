using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Tests.Helpers
{
    public static class MockHelper
    {
        public static void ReturnsResponse(
            this Mock<HttpMessageHandler> mockHandler,
            HttpMethod method,
            string url,
            HttpStatusCode statusCode,
            string content)
        {
            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.Is<HttpRequestMessage>(req =>
                               req.Method == method && req.RequestUri.ToString() == url),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = statusCode,
                           Content = new StringContent(content),
                       });
        }
    }
}
