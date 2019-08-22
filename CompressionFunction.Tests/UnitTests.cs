using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace CompressionFunction.Tests
{
    public class UnitTests
    {
        private const string UNCOMPRESSED_DATA = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Placerat in egestas erat imperdiet sed euismod. Neque convallis a cras semper auctor neque vitae tempus quam. Libero nunc consequat interdum varius sit amet mattis. Sed id semper risus in hendrerit gravida. Egestas sed sed risus pretium quam vulputate dignissim suspendisse. Bibendum neque egestas congue quisque egestas. Cursus metus aliquam eleifend mi in nulla posuere. Faucibus ornare suspendisse sed nisi lacus sed. Arcu bibendum at varius vel pharetra vel turpis. Tortor at auctor urna nunc id cursus metus aliquam eleifend. Aliquam id diam maecenas ultricies mi eget. Nunc aliquet bibendum enim facilisis gravida neque convallis a. Justo donec enim diam vulputate ut pharetra sit amet aliquam. Non pulvinar neque laoreet suspendisse interdum consectetur libero id. Morbi non arcu risus quis varius quam quisque id diam. Tincidunt id aliquet risus feugiat in ante.";

        // this is the compressed & base64-encoded version of 'UNCOMPRESSED_DATA'
        private const string COMPRESSED_DATA = @"H4sIAAAAAAAAC2NgGAWjITAaAgzDFgAAOpeCKOIDAAA=";

        [Theory]
        [InlineData(UNCOMPRESSED_DATA)]
        public async System.Threading.Tasks.Task TestCompressionAsync(string dataToCompress)
        {
            var result = await Functions.Compress(TestFactory.CreateHttpRequest(dataToCompress), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            var resultValue = (string)((OkObjectResult)result).Value;

            Assert.False(string.IsNullOrEmpty(resultValue));

            Console.WriteLine($@"Result: {resultValue}");
        }

        [Theory]
        [InlineData(COMPRESSED_DATA)]
        public async System.Threading.Tasks.Task TestDecompressionAsync(string dataToDecompress)
        {
            var result = await Functions.Decompress(TestFactory.CreateHttpRequest(dataToDecompress), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            var resultValue = (string)((OkObjectResult)result).Value;

            Assert.False(string.IsNullOrEmpty(resultValue));

            Console.WriteLine($@"Result: {resultValue}");
        }

        [Theory]
        [InlineData(UNCOMPRESSED_DATA)]
        [InlineData(@"едц")]    // to ensure that our encode/decode of compressed string works with "problematic" foreign characters
        public async System.Threading.Tasks.Task TestRoundTripAsync(string testString)
        {
            //compress a string via the Compress Function
            var result = await Functions.Compress(TestFactory.CreateHttpRequest(testString), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            var resultValue = (string)((OkObjectResult)result).Value;

            Assert.False(string.IsNullOrEmpty(resultValue));

            // pass the result into the Decompress function
            result = await Functions.Decompress(TestFactory.CreateHttpRequest(resultValue), TestFactory.CreateLogger(LoggerTypes.List)).ConfigureAwait(false);

            Assert.NotNull(result);

            Assert.IsType<OkObjectResult>(result);
            resultValue = (string)((OkObjectResult)result).Value;

            Assert.False(string.IsNullOrEmpty(resultValue));

            // Assert the result matches what we used in the beginning
            Assert.Equal(testString, resultValue);
        }
    }
}
