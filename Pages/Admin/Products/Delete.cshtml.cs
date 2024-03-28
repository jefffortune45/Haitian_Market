using Haitian_Market.Data;
using Haitian_Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Haitian_Market.Pages.Admin.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;
        [BindProperty]
        public Product Product { get; set; } = new Product();

        public DeleteModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if(id == null)
            {
                Response.Redirect("Index");
                return;
            }
            var product = context.Product.Find(id);
            if (product == null)
            {
                Response.Redirect("Index");
                return;
            }
            string ImageFullPath = environment.WebRootPath + "/Products/" + product.ImageFileName;
            System.IO.File.Delete(ImageFullPath);

            context.Product.Remove(product);
            context.SaveChanges();

           // Response.Redirect("Index");
        }
    }
}
