using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FileUploadNet48Demo.Controllers
{
    [RoutePrefix("YudieflyAPI/FileData")]
    public class FileDataController : ApiController
    {
        private const int MaxFileSize = 50 * 1024 * 1024; // 最大文件大小（50MB） 
        private const string UploadFolder = "~/Uploads"; // 上传文件夹路径  
        private static readonly string[] AllowedFileTypes = { ".jpg", ".jpeg", ".png", ".gif", ".docx", ".bmp", ".xls",".xlxs", ".ppt", ".pptx", ".confg", ".ico", ".txt",".doc",".pdf" }; // 允许的文件类型  

        // POST api/fileupload  
        [Route("UpLoad")]
        public async Task<IHttpActionResult> Upload()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath(UploadFolder);
            Directory.CreateDirectory(root); // 确保目录存在  

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.Contents)
                {
                    if (!file.Headers.ContentType.MediaType.StartsWith("image/")&&!file.Headers.ContentType.MediaType.StartsWith("file/")&& !file.Headers.ContentType.MediaType.StartsWith("application/"))
                    {
                        return BadRequest("Unsupported media type.");
                    }
                    var fullName = file.Headers.ContentDisposition.FileName.Trim('"');
                    var filename = GetSafeFileName(fullName);
                    var fileExtension = Path.GetExtension(filename).ToLower();
                    if (!AllowedFileTypes.Contains(fileExtension))
                    {
                        return BadRequest("Unsupported file type.");
                    }

                    var fileBytes = await file.ReadAsByteArrayAsync();
                    if (fileBytes.Length > MaxFileSize)
                    {
                        return BadRequest("File size exceeds the maximum allowed limit.");
                    }

                    // 保存文件到服务器  
                    var filePath = Path.Combine(root, filename);
                    File.WriteAllBytes(filePath, fileBytes);
                }

                return Ok("Files uploaded successfully.");
            }
            catch (Exception ex)
            {
                // 处理异常，比如记录日志等  
                // ...  
                return InternalServerError(ex);
            }
        }

        private string GetSafeFileName(string fileName)
        {
            // 防止目录遍历攻击  
            return Path.GetFileName(fileName ?? "noname");
        }
    }
}
