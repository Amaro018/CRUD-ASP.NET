using Microsoft.AspNetCore.Mvc;
using Watch.Models;
using Watch.Services;

namespace Watch.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

		public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
			this.environment = environment;
		}
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }

		public IActionResult Create()
        { 
			return View();
		}

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "This Image File is Required");
            }

            if(!ModelState.IsValid)
            {
                return View(productDto);
            }


            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            Product product = new Product()
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
			};

            context.Products.Add(product);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Product created successfully!";


            return RedirectToAction("Index", "Products");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if(product==null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
            };

            ViewData["ProductId"] = product.Id;
			ViewData["ImageFileName"] = product.ImageFileName;
			ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
			return View(productDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
			var product = context.Products.Find(id);

            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if(!ModelState.IsValid)
            {
				ViewData["ProductId"] = product.Id;
				ViewData["ImageFileName"] = product.ImageFileName;
				ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(productDto);
			}

            string newFileName = product.ImageFileName;
            if(productDto.ImageFile != null)
            {
				newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

				string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
				using (var stream = System.IO.File.Create(imageFullPath))
				{
					productDto.ImageFile.CopyTo(stream);
				}

                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
			}

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageFileName = newFileName;

            context.SaveChanges();

            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction("Index", "Products");
		}


        //this is the DELETE FUNCTION
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);

            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
			System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction("Index", "Products");
		}

	}
}
