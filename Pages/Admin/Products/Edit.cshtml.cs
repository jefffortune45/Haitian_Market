using Haitian_Market.Data;
using Haitian_Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Haitian_Market.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;
        [BindProperty]
        public ProducDto ProducDto { get; set; }= new ProducDto();
        public Product Product { get; set; } = new Product();
        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("Index");
                return;
            }

            var product = context.Product.Find(id);
            if(product == null)
            {
                Response.Redirect("Index");
                return;
            }
            ProducDto.Name = product.Name;
            ProducDto.Description = product.Description;
            ProducDto.Price = product.Price;
            ProducDto.Brand = product.Brand;
            ProducDto.Category = product.Category;

            Product = product;

        }
        public void OnPost(int? id)
        {

            if (id == null)
            {
                Response.Redirect("Index");
                return;
            }
            if (!ModelState.IsValid)
            {
                errorMessage = "please provide all the required fields";
                return;
            }
            var product = context.Product.Find(id);
            if( product == null)
            {
                Response.Redirect("Index");
                return;
            }

            // update image
            string newFileName = product.ImageFileName;
            if(ProducDto.ImageFile!= null)
            {
                 newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ProducDto.ImageFile!.FileName);
                string imageFullPath = environment.WebRootPath + "/Products/" + newFileName;

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    ProducDto.ImageFile.CopyTo(stream);
                }
                //delete olimage
                string olImageFullPath = environment.WebRootPath + "/Products/" + product.ImageFileName;
                System.IO.File.Delete(olImageFullPath);
            }

            // update product
            product.Name = ProducDto.Name;
            product.Brand = ProducDto.Brand;
            product.Category = ProducDto.Category;
            product.Price = ProducDto.Price;
            product.Description = ProducDto.Description ?? "";
            product.ImageFileName = newFileName;

            context.SaveChanges();

            Product = product;
            successMessage = "update is successfull";
            Response.Redirect("Index");



        }
    }
}
