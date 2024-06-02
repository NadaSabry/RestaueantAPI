using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
//using RestaueantAPI.Models;
using RestaueantAPI.UATModels;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Text.Json.Nodes;
using ClosedXML.Excel;
using RestaueantAPI.ModelsPostGres;


namespace RestaueantAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RestaurantController : Controller
	{
		private readonly MomkenContext db;
		private readonly IWebHostEnvironment _environment;
        private readonly AdminToolContext _contextPG;

        // TmsContext dbb = new TmsContext();
        public RestaurantController(MomkenContext _db, IWebHostEnvironment webHostEnvironment, AdminToolContext contextPG)
		{
			db = _db ?? throw new ArgumentNullException();
            _environment = webHostEnvironment;
            _contextPG = contextPG;

        }
		
		[HttpGet]
		public IActionResult GetRestaurant()
		{
			var restaurant = db.Restaurants.Take(10).ToList();

			return Ok(restaurant);
		}
		
		[HttpGet("{Id}")]
		public IActionResult GetRestaurant(int Id)
		{
			var restaurant = db.Restaurants.Find(Id);
			if (restaurant == null) { 
				return NotFound();
			}
			return Ok(restaurant);
		}


		// to insure resturant exist in the database
		private bool RestaurantExist(int Id)
		{
			return db.Restaurants.Count(r => r.Id==Id) > 0;
		}

		[HttpDelete("{Id}")]
		public IActionResult Delete(int Id)
		{
			var res = db.Restaurants.Find(Id);
			if (res is null) {
				return NotFound();
			}
			db.Remove(res);
			db.SaveChanges();

			return Ok(res);
		}

		[HttpPut("{id}")]
		// علشان محدش يعدل في قيمه ال id 
		// to isure the id parameter not change
		public IActionResult PutRestaurant(int id, [FromBody]Restaurant res) 
		{ 
			if(!ModelState.IsValid){
				// to return the error
				return BadRequest(ModelState);
			}
			if(id != res.Id)
			{
				return BadRequest("Sorry you can't modified the primary_key !!!");
			}
			//db.Update(res);
			db.Update(res);
			try
			{
				db.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest($"Invalid object modification : error = {e.InnerException}");
			}
			return Ok(res);
		}



        //------------------------------------------------------------------------------------

		
        [HttpPost("BulkRestaurants")]
        public async Task<IActionResult> uploadExcelSheet(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string userName = Request.Form["UserName"];

            // you should accept only .xlsx

            try
            {
                var filePath = Path.Combine(_environment.ContentRootPath, "Temp", file.FileName);

                Console.WriteLine(" filePath = " + filePath + "\n");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Reading ExcelSheet 
                using var wbook = new XLWorkbook(filePath);
                var ws1 = wbook.Worksheet(1);
                int lastRow = ws1.LastRowUsed().RowNumber();

                int successTRXs = 0;

                for (var row = 2; row <= lastRow; row++)
                {

                    var RestaurantCodews = ws1.Cell(row, 1).Value.ToString();
                    string RestaurantNamews = ws1.Cell(row, 2).Value.ToString();
                    string RestaurantNameAr = ws1.Cell(row, 3).Value.ToString();
                    var res = new Restaurant
                    {
                        RestaurantCode = RestaurantCodews,
                        RestaurantName = RestaurantNamews,
                        RestaurantNameAr = RestaurantNameAr,
                    };

                    Console.WriteLine("---------------------------------------" + RestaurantNamews + "\n");
                    var existingRestaurant = await db.Restaurants.FirstOrDefaultAsync(res => res.RestaurantCode == RestaurantCodews);

                    if (existingRestaurant == null)
                    {
                        await db.Restaurants.AddAsync(res);
                        successTRXs++;
                        Console.WriteLine("xxxxxxxxxxxxxxx   -- tmam ");
                    }
                    else
                    {
                        Console.WriteLine("xxxxxxxxxxxxxxx error in restaurant name = ", RestaurantNamews);
                    }

                }
                Console.WriteLine("**************************************************");
                string status = "Failed";
                int success = successTRXs;
                int failed = (lastRow - 1) - successTRXs;
                if (success > 0)
                {
                    status = "Done";
                }
                var responseMessage = new UploadedFile
                {
                    EntryDate = DateTime.Now,
                    FileStatus = status,
                    Filename = file.FileName,
                    UserName = userName,
                    FileType = "Restaurant",
                    TotalCount = lastRow - 1,
                    FailedCount = failed,
                    SuccessCount = success,
                };

                await _contextPG.UploadedFiles.AddAsync(responseMessage);
                _contextPG.SaveChanges();
                db.SaveChanges();

                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return BadRequest(error: "An error occurred during the upload: " + ex.InnerException.Message);
            }
        }

        [Route("api/[controller]/serarch/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> SearchFiles(string fileName)
        {
            var ResturantFiles = await _contextPG.UploadedFiles.Where(
                file => file.FileType == "Restaurant" && file.Filename.ToLower().Contains(fileName.ToLower())).ToListAsync();
            return Ok(ResturantFiles);
        }

        [HttpGet("GetFfiles")]
        public IActionResult GetFiles()
        {
			var ResturantFiles = _contextPG.UploadedFiles.Where(
				file => file.FileType == "Restaurant").ToList();
            //var Resturants = await _context.Restaurants.ToListAsync();
            return Ok(ResturantFiles);
        }
		

    }
}
