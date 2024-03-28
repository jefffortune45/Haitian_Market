using Haitian_Market.Data;
using Haitian_Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Haitian_Market.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ProducDto productDto { get; set; } = new ProducDto();

        public CreateModel( IWebHostEnvironment environment,ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet()
        {
        }
        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost()
        {
            if(productDto.ImageFile== null)
            {
                ModelState.AddModelError("productDto.ImageFile","image file is required");
            }
            if (!ModelState.IsValid)
            {
                errorMessage = "please provide all required file";
            }
            //save image
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/Products/" + newFileName;
            using(var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            // save product
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description ?? "",
                ImageFileName = newFileName,
                CreatedAt= DateTime.Now,
            };
            context.Add(product);
            context.SaveChanges();


            //clear the form
            productDto.Name = "";
            productDto.Brand = "";
            productDto.Category = "";
            productDto.Price = 0;
            productDto.Description = "";
            productDto.ImageFile = null;

            ModelState.Clear();

            successMessage = "product create successfully";
            Response.Redirect("Index");
        }
    }
}
