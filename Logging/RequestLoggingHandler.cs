using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace NeorisBackend.Logging
{
    /// <summary>
    /// Provee un manejador de mensajes HTTP que registra detalles de las solicitudes y respuestas HTTP.
    /// </summary>
    public class RequestLoggingHandler : DelegatingHandler
    {
        /// <summary>
        /// Envía una solicitud HTTP y registra detalles sobre la solicitud y la respuesta.
        /// </summary>
        /// <param name="request">Solicitud HTTP a enviar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Respuesta HTTP.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                stopwatch.Stop();

                Log.Information(
                    "HTTP {Method} {Url} responded {StatusCode} in {ElapsedMs} ms",
                    request.Method.Method,
                    request.RequestUri,
                    (int)response.StatusCode,
                    stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Log.Error(
                    ex,
                    "HTTP {Method} {Url} failed in {ElapsedMs} ms",
                    request.Method.Method,
                    request.RequestUri,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
