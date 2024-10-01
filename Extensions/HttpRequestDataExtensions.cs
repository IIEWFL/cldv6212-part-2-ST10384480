using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace abcRetailFunctionApp.Extensions
{
    public static class HttpRequestDataExtensions
    {
        public static async Task<Dictionary<string, Stream>> ReadMultipartFormDataAsync(this HttpRequestData req)
        {
            var formData = new Dictionary<string, Stream>();

            if (!req.Headers.TryGetValues(HeaderNames.ContentType, out var contentTypeValues))
            {
                return formData;
            }

            // Use MediaTypeHeaderValue instead of ContentType
            var contentTypeHeader = contentTypeValues.FirstOrDefault();
            if (contentTypeHeader == null || !MediaTypeHeaderValue.TryParse(contentTypeHeader, out var mediaTypeHeader))
            {
                return formData;
            }

            if (!mediaTypeHeader.MediaType.Equals("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            {
                return formData;
            }

            var boundary = HeaderUtilities.RemoveQuotes(mediaTypeHeader.Boundary).Value;
            if (string.IsNullOrEmpty(boundary))
            {
                return formData;
            }

            var multipartReader = new MultipartReader(boundary, req.Body);
            var section = await multipartReader.ReadNextSectionAsync();

            while (section != null)
            {
                var contentDisposition = ContentDispositionHeaderValue.Parse(section.ContentDisposition);
                if (contentDisposition.IsFileDisposition())
                {
                    var fileName = HeaderUtilities.RemoveQuotes(contentDisposition.FileName).Value;
                    var stream = new MemoryStream();
                    await section.Body.CopyToAsync(stream);
                    stream.Position = 0;
                    formData.Add(fileName, stream);
                }
                section = await multipartReader.ReadNextSectionAsync();
            }

            return formData;
        }

        private static bool IsFileDisposition(this ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null &&
                   contentDisposition.DispositionType.Equals("form-data", StringComparison.OrdinalIgnoreCase) &&
                   (!string.IsNullOrEmpty(contentDisposition.FileName.Value) || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}