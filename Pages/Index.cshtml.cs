﻿using Haitian_Market.Data;
using Haitian_Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Haitian_Market.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ApplicationDbContext context;
        public List<Product> Products { get; set; } = new List<Product>();




        //pagination
        public int pageIndex = 1;
        public int totalPage = 0;
        public readonly int pageSize = 5;

        //searche functionnality
        public string search = "";


        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public void OnGet(int? pageIndex, string? search)
        {
            IQueryable<Product> query = context.Product;

            //IQueryable<Product> query = context.Product;

            // search functionnality
            if (search != null)
            {
                this.search = search;
                query = query.Where(p => p.Name.Contains(search) || p.Brand.Contains(search) || p.Description.Contains(search) || p.Category.Contains(search));
            }
            query = query.OrderByDescending(p => p.Id);

            if (pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }
            this.pageIndex = (int)pageIndex;
            decimal count = query.Count();
            totalPage = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((this.pageIndex - 1) * pageSize).Take(pageSize);

            Products = query.ToList();

            // Products = context.Product.OrderByDescending(p => p.Id).ToList();

        }
    }
}