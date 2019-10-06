using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Entities
{

    public enum ImageTypes
    {
        PHOTO = 1,
        SIGNATURE
    }
    public class ImageUploadRequest
    {
        public string customerNo { get; set; }
        public string customerType { get; set; }
        public string customerName { get; set; }
        public string sex { get; set; }
        public string ImageData { get; set; }
        public string ImageType { get; set; }
        public string ImageExtension { get; set; }
        public string userName { get; set; }
        public string requestId { get; set; }

    }
}
