using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Entities
{
    public interface IImageUploadRepository
    {
        Task<ImageUploadResponse> UploadImage(ImageUploadRequest request);
        string EncData(string value);
    }
}
