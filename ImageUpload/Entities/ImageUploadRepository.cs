using Commons.Entities;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Entities
{
    public class ImageUploadRepository : IImageUploadRepository
    {
        private readonly AppSettings _appSettings;
        private IDataProtector _protector;
        private readonly ILogger<ImageUploadRepository> _logger;

        public ImageUploadRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider,
            ILogger<ImageUploadRepository> logger)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector("treyryug");
            _logger = logger;
        }
        public async Task<ImageUploadResponse> UploadImage(ImageUploadRequest request)
        {
            bool isSaved = false;

            string imgExt = string.IsNullOrEmpty(request.ImageExtension) ? _appSettings.DefaultFileExtension : request.ImageExtension;

            string filePath = $@"{_appSettings.UploadPath}\{request.ImageType.ToLower()}\{request.customerNo}.{imgExt}";
            await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(request.ImageData));

            if (File.Exists(filePath))
            {
                try
                {
                    isSaved = await InsertImageDetails(new ImageDetails()
                    {
                        CUST_NO = request.customerNo,
                        IMAGE_NAME = $"{request.customerNo}.{imgExt})",
                        IMAGE_PATH = filePath,
                        IMAGE_TYPE = request.ImageType,
                        REQ_ID = request.requestId,
                        UPD_BY = request.userName,
                        UPD_DATE = DateTime.Now.ToString()
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"InsertImageDetails: {Environment.NewLine} {ex.ToString()}");
                    return new ImageUploadResponse() { status = "N", message = "Unable to save image Details." };
                }            
            }
            else
            {
                return new ImageUploadResponse() { status = "N", message = "Image Upload Failed." };
            }

            if (isSaved)
            {
                try
                {
                    isSaved = await UploadImageToCore(request.customerNo);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"UploadImageToCore: {Environment.NewLine} {ex.ToString()}");
                    return new ImageUploadResponse() { status = "N", message = "Unable to upload image to core." };
                }
                
            }

            return new ImageUploadResponse() { status = "Y", message = "Image Successfully Uploaded" };
        }

        public async Task<bool> InsertImageDetails(ImageDetails imgDet)
        {
            bool isSaved = false;

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));

            using (oralConnect)
            {
                string query = $@"INSERT INTO TEMP_SIG_IMAGE_UPLOAD(CUST_NO, IMAGE_NAME, IMAGE_PATH, IMAGE_TYPE, UPD_BY, REQ_ID, UPD_DATE) 
                                VALUES (:CUST_NO, :IMAGE_NAME, :IMAGE_PATH, :IMAGE_TYPE, :UPD_BY, :REQ_ID, :UPD_DATE)";

                int r = await oralConnect.ExecuteAsync(query, imgDet);

                isSaved = r > 0;
            }

            return isSaved;
        }

        public async Task<bool> UploadImageToCore(string customerNo)
        {
            bool isSaved = false;
            string storeProcedure = $"{_appSettings.FlexSchema}.{_appSettings.PaytSproc}";

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));

            var param = new DynamicParameters();
            param.Add("customeR_No", customerNo);

            using (oralConnect)
            {
                oralConnect.Open();
                int r = await oralConnect.ExecuteAsync(storeProcedure, param, commandType: CommandType.StoredProcedure);

                isSaved = r > 0;
            }

            return isSaved;
        }

        public string EncData(string value)
        {
            string output = string.Empty;
            try
            {
                output = _protector.Protect(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return output;
        }
    }
}
