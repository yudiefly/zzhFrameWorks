using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadForNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDataController : ControllerBase
    {
        private const string UploadFolder = "uploads"; // 上传文件夹名称  
        private const int MaxFileSizeMB = 50; // 最大文件大小（MB）  

        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif",".txt",".doc",".docx",".xls",".xlsx",".ppt",".pptx",".config",".log",".pdf" // 允许的文件扩展名  
        };

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || !files.Any())
            {
                return BadRequest("No files uploaded.");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), UploadFolder);
            Directory.CreateDirectory(uploadPath); // 确保上传文件夹存在  

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue; // 跳过空文件  
                }

                if (file.Length > MaxFileSizeMB * 1024 * 1024) // 检查文件大小  
                {
                    return BadRequest($"File size limit exceeded for {file.FileName}.");
                }

                var fileExtension = Path.GetExtension(file.FileName);
                if (!AllowedExtensions.Contains(fileExtension)) // 检查文件类型  
                {
                    return BadRequest($"File type {fileExtension} is not allowed for {file.FileName}.");
                }

                var filePath = Path.Combine(uploadPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok("Files uploaded successfully.");
        }

    }
}
