using demo_api.Models;
using demo_api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace demo_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionsController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Inject both the database service and the web hosting environment
        public SubmissionsController(MongoDbService mongoDbService, IWebHostEnvironment webHostEnvironment)
        {
            _mongoDbService = mongoDbService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /api/Submissions
        [HttpGet]
        public async Task<List<FormSubmission>> Get()
        {
            return await _mongoDbService.GetAsync();
        }

        // POST: /api/Submissions
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FormSubmission submission, IFormFile? imageFile)
        {
            // Handle the image file upload
            if (imageFile != null && imageFile.Length > 0)
            {
                // 1. Create a unique file name to prevent conflicts
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                // 2. Define the path to save the image (wwwroot/uploads)
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 3. Save the image to the server's file system
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // 4. Build the public URL for the image and set it on the submission object
                var request = HttpContext.Request;
                submission.ImageUrl = $"{request.Scheme}://{request.Host}/uploads/{uniqueFileName}";
            }

            // Save the complete submission object (with location, image URL, etc.) to the database
            await _mongoDbService.CreateAsync(submission);

            return CreatedAtAction(nameof(Get), new { id = submission.Id }, submission);
        }
    }
}
