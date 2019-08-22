using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CompressionFunction
{
    public static class Functions
    {
        [FunctionName(nameof(Compress))]
        public static async System.Threading.Tasks.Task<IActionResult> Compress(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Gzip the body in to a new memory stream. This is ok because bodies shouldn't exceed the 1.5GB memory limit of a Consumption Function
            using (var output = new MemoryStream())
            {
                using (var gs = new GZipStream(output, CompressionMode.Compress))
                {
                    await req.Body.CopyToAsync(gs);
                }

                // Convert the memory stream to a base64 string and return it to the caller
                return new OkObjectResult(Convert.ToBase64String(output.ToArray()));
            }
        }

        [FunctionName(nameof(Decompress))]
        public static async System.Threading.Tasks.Task<IActionResult> Decompress(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var outputString = new StringBuilder();
            // The input is expected to be gzipped, THEN base64 encoded

            using (var bodyReader = new StreamReader(req.Body))
            {
                // so base64 decode it first
                var bodyContent = bodyReader.ReadToEnd();
                var gzippedContent = Convert.FromBase64String(bodyContent);
                // then load that content into a gzipstream to be uncompressed
                using (var zippedContentReader = new MemoryStream(gzippedContent))
                using (var output = new MemoryStream())
                {
                    using (var gUnzipper = new GZipStream(zippedContentReader, CompressionMode.Decompress))
                    {
                        await gUnzipper.CopyToAsync(output);
                    }

                    return new OkObjectResult(Encoding.Default.GetString(output.ToArray()));
                }
            }
        }
    }
}
