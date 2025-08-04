using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Images
{
    public interface IImageService
    {
        public Task<(string fileId, string url)> UploadImageAsync(IFormFile file, string folder = "/Blog");
        public Task<bool> DeleteImageAsync(string fileId);
    }
}
