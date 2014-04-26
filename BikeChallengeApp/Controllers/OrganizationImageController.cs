using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace BikeChallengeApp.Controllers
{
    public class OrganizationImageController : ApiController
    {
        [HttpPost] // api/OrganizationImage?OrgName=[orgname]
        public async Task<HttpResponseMessage> Upload(string OrgName)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = GetMultipartProviderOrg(OrgName);
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
            // so this is how you can get the original file name
            var originalFileName = GetDeserializedFileNameOrg(result.FileData.First());
           
            // uploadedFileInfo object will give you some additional stuff like file length,
            // creation time, directory name, a few filesystem methods etc..
            var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);
            var suffix = "\\ProfileImage" + DateTime.Now.ToString("dd_M_yyyy") + ".jpg";
            var new_file_name = uploadedFileInfo.Directory.ToString() + suffix;
            if(File.Exists(new_file_name))
            {
                System.IO.File.Move(new_file_name, uploadedFileInfo.Directory.ToString() + "\\OLD_ProfileImage" + DateTime.Now.ToString("dd_M_yyyy_HHmm") + ".jpg");
            }
            try
            {
                System.IO.File.Move(uploadedFileInfo.ToString(), new_file_name);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            // Remove this line as well as GetFormData method if you're not
            // sending any form data with your upload request
           // var fileUploadObj = GetFormData<UploadDataModel>(result);

            // Through the request response you can return an object to the Angular controller
            // You will be able to access this in the .success callback through its data attribute
            // If you want to send something to the .error callback, use the HttpStatusCode.BadRequest instead
            var returnData = "\\ProfileImages\\Organizations\\" + OrgName + suffix;
            return this.Request.CreateResponse(HttpStatusCode.OK, new { returnData });
        }

        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        private MultipartFormDataStreamProvider GetMultipartProviderOrg(string OrgName)
        {
            // IMPORTANT: replace "(tilde)" with the real tilde character
            // (our editor doesn't allow it, so I just wrote "(tilde)" instead)
            var uploadFolder = "~\\ProfileImages\\Organizations\\" + OrgName; // you could put this to web.config
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        // Extracts Request FormatData as a strongly typed model
        private object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData
                    .GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }

        private string GetDeserializedFileNameOrg(MultipartFileData fileData)
        {
            var fileName = GetFileNameOrg(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public string GetFileNameOrg(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
}