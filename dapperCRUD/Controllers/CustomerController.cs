using Dapper;
using dapperCRUD.Models;
using dapperCRUD.Services.CustomerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.IO;

namespace dapperCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        public static IWebHostEnvironment _environment;

        public CustomerController(ICustomerRepository repository, IWebHostEnvironment environment)
        {
            _repository = repository; 
            _environment = environment;
        }
     
        [HttpGet("/GetAll")]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            
            IEnumerable<Customer> customers = await _repository.GetAllCustomers();
            return Ok(customers);
        }


        [HttpGet("/Get/{customerId}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid customerId)
        {
            
            var searchedCustomer = await _repository.GetCustomerById(customerId);

            if (searchedCustomer == null)
            {
                return NotFound();
            }

            return Ok(searchedCustomer);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<List<Customer>>> CreateCustomer([FromForm] Customer customer)
        {

            string savePath = _environment.WebRootPath + "\\Upload\\";
            try
            {
                if (customer.picture.Length > 0)
                //if (imageFile.Length > 0)
                {
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }

                    string filePath = Path.Combine(savePath, customer.picture.FileName);
                    //string filePath = Path.Combine(savePath, imageFile.FileName);
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        customer.picture.CopyTo(fileStream);
                        //imageFile.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    if (System.IO.File.Exists(filePath))
                    {
                        customer.PicturePath = Path.GetFullPath(filePath);
                        Console.WriteLine("File has been created successfully");

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            await _repository.CreateCustomer(customer);
            return Ok(await _repository.GetCustomerById(customer.Id));

        }

        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<List<Customer>>> UpdateCustomer(Customer customer)
        {
            var searchedCustomer = await _repository.GetCustomerById(customer.Id);

            if (searchedCustomer == null)
            {
                return NotFound();
            }
            await _repository.UpdateCustomer(customer);
            return Ok(await _repository.GetAllCustomers());
        }


        [HttpDelete("/Delete/{customerId}")]
        public async Task<ActionResult<List<Customer>>> DeleteCustomer(Guid customerId)
        {

            // Check if the customer exists
             var searchedCustomer = await _repository.GetCustomerById(customerId);

            if (searchedCustomer == null)
            {
                return NotFound();
            }
            await _repository.DeleteCustomer(customerId);

            return Ok(await _repository.GetAllCustomers());
        }

        [HttpGet("/GetIdName")]
        public async Task<ActionResult<List<string>>> GetIdNameCustomers()
        {

            IEnumerable<Customer> customers = await _repository.GetAllCustomers();
            List<string> names = new List<string>();
            foreach (Customer customer in customers)
            {
                names.Add(customer.Name);
            }
            return Ok(names);
        }

        [HttpGet("/GetIdName{customerId}")]
        public async Task<ActionResult<string>> GetNameCustomersId(Guid customerId)
        {

           Customer customer = await _repository.GetCustomerById(customerId);
           
            return Ok(customer.Name);
        }

        //[HttpPost("/Image")]
        //public async Task SetImage(IFormFile inputFile)
        //{

        //    string savePath = _environment.WebRootPath + "\\Upload\\";
        //    try
        //    {
        //        if (inputFile.Length > 0)
        //        {
        //            if (!Directory.Exists(savePath))
        //            {
        //                Directory.CreateDirectory(savePath);
        //            }
        //            using (FileStream fileStream = System.IO.File.Create(savePath + inputFile.FileName))
        //            {
        //                inputFile.CopyTo(fileStream);
        //                fileStream.Flush();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
