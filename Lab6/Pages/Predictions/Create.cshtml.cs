using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab6.Data;
using Lab6.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        private readonly Lab6.Data.PredictionDataContext _context;

        public CreateModel(Lab6.Data.PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedFile)
        {
            string containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
            var containerClient = await GetOrCreateContainerClientAsync(containerName);

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string fileExtension = Path.GetExtension(uploadedFile.FileName);
                string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                var blobClient = containerClient.GetBlobClient(uniqueFileName);

                try
                {
                    await UploadFileToBlobAsync(uploadedFile, blobClient);
                    Prediction.FileName = uniqueFileName;
                    Prediction.Url = blobClient.Uri.ToString(); // ✅ Ensure URL is always set
                }
                catch (RequestFailedException ex)
                {
                    Console.WriteLine($"Blob Upload Failed: {ex.Message}");
                    return RedirectToPage("/Error");  // ✅ Redirect to error page
                }
            }
            else
            {
                ModelState.AddModelError("UploadedFile", "File is required.");
                return Page();  // ✅ Stay on the page if file is missing
            }

            _context.Predictions.Add(Prediction);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task<BlobContainerClient> GetOrCreateContainerClientAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!await containerClient.ExistsAsync())
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            return containerClient;
        }

        private async Task UploadFileToBlobAsync(IFormFile file, BlobClient blobClient)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream);
            }
        }
    }
}

