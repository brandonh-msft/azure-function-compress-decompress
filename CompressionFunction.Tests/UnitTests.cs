using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace CompressionFunction.Tests
{
    public class UnitTests
    {
        private const string UNCOMPRESSED_DATA = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Placerat in egestas erat imperdiet sed euismod. Neque convallis a cras semper auctor neque vitae tempus quam. Libero nunc consequat interdum varius sit amet mattis. Sed id semper risus in hendrerit gravida. Egestas sed sed risus pretium quam vulputate dignissim suspendisse. Bibendum neque egestas congue quisque egestas. Cursus metus aliquam eleifend mi in nulla posuere. Faucibus ornare suspendisse sed nisi lacus sed. Arcu bibendum at varius vel pharetra vel turpis. Tortor at auctor urna nunc id cursus metus aliquam eleifend. Aliquam id diam maecenas ultricies mi eget. Nunc aliquet bibendum enim facilisis gravida neque convallis a. Justo donec enim diam vulputate ut pharetra sit amet aliquam. Non pulvinar neque laoreet suspendisse interdum consectetur libero id. Morbi non arcu risus quis varius quam quisque id diam. Tincidunt id aliquet risus feugiat in ante.";

        private const string COMPRESSED_DATA = @"H4sIAAAAAAAAC2NgGAWjITAaAgzDFgAAOpeCKOIDAAA=";

        [Fact]
        public async System.Threading.Tasks.Task TestCompressionAsync()
        {
            var result = await Functions.Compress(TestFactory.CreateHttpRequest(UNCOMPRESSED_DATA), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            var resultValue = (string)((OkObjectResult)result).Value;

            Assert.True(resultValue.Length > 0);

            Console.WriteLine($@"Result: {resultValue}");
        }

        [Fact]
        public async System.Threading.Tasks.Task TestDecompressionAsync()
        {
            var result = await Functions.Decompress(TestFactory.CreateHttpRequest(COMPRESSED_DATA), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            var resultValue = (string)((OkObjectResult)result).Value;

            Assert.True(resultValue.Length > 0);

            Console.WriteLine($@"Result: {resultValue}");
        }

        [Fact]
        public async System.Threading.Tasks.Task TestRoundTripAsync()
        {
            //compress a string via the Compress Function
            var result = await Functions.Compress(TestFactory.CreateHttpRequest(UNCOMPRESSED_DATA), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            var resultValue = (string)((OkObjectResult)result).Value;

            // pass the result into the Decompress function
            result = await Functions.Decompress(TestFactory.CreateHttpRequest(resultValue), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            resultValue = (string)((OkObjectResult)result).Value;

            Assert.True(resultValue.Length > 0);

            // Assert the result matches what we used in the beginning
            Assert.Equal(UNCOMPRESSED_DATA, resultValue);
        }

        const string FOREIGN_CHAR_PAYLOAD = @"едц";
        [Fact]
        public async System.Threading.Tasks.Task TestForeignCharRoundTripAsync()
        {
            //compress a string via the Compress Function
            var result = await Functions.Compress(TestFactory.CreateHttpRequest(FOREIGN_CHAR_PAYLOAD), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            var resultValue = (string)((OkObjectResult)result).Value;

            // pass the result into the Decompress function
            result = await Functions.Decompress(TestFactory.CreateHttpRequest(resultValue), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            resultValue = (string)((OkObjectResult)result).Value;

            Assert.True(resultValue.Length > 0);

            // Assert the result matches what we used in the beginning
            Assert.Equal(FOREIGN_CHAR_PAYLOAD, resultValue);
        }
    }
}
