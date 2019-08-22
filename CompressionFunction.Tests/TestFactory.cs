using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO;

namespace CompressionFunction.Tests
{
    public class TestFactory
    {
        public static DefaultHttpRequest CreateHttpRequest(string content)
        {
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);
            sw.Write(content);
            sw.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = stream
            };

            return request;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}