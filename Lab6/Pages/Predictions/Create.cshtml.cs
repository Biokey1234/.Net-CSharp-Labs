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
            if (!ModelState.IsValid || uploadedFile == null || uploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFile", "File is required.");
                return Page();  // Return form with validation error
            }
            string containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
            var containerClient = await GetOrCreateContainerClientAsync(containerName);

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string randomFileName = Path.GetRandomFileName();
                var blobClient = containerClient.GetBlobClient(randomFileName);

                try
                {
                    await UploadFileToBlobAsync(uploadedFile, blobClient);
                    Prediction.Url = blobClient.Uri.ToString();
                }
                catch (RequestFailedException)
                {
                    RedirectToPage("Error"); 
                }
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

