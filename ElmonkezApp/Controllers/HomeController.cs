using ElmonkezApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using ElmonkezApp.Data;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using ElmonkezApp.Utility;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElmonkezApp.Controllers
{
    public class HomeController : Controller
    {

        //private ApplicationDbContext _db;

        //public HomeController(ApplicationDbContext db)
        //{
        //    _db = db;
        //}
        ElmonkezApp.Data.ApplicationDbContext db = new Data.ApplicationDbContext();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public IActionResult Index()
        {

            dynamic dy = new ExpandoObject();
            dy.CategoryList = GetCategory();
            
            return View(dy);

        }

        public IActionResult About()
        {
            dynamic dy = new ExpandoObject();
            dy.EmployeeList = GetEmployees();
            return View(dy);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Cart()
        {
            List<Service> services = HttpContext.Session.Get<List<Service>>("services");
            if (services == null)
            {
                services = new List<Service>();
            }
            return View(services);
        }
        public IActionResult Service(int? id)
        {
            ElmonkezApp.Data.ApplicationDbContext db = new Data.ApplicationDbContext();

            dynamic dy = new ExpandoObject();

            var service = db.Services.Include(c => c.Category).FirstOrDefault(c => c.CategoryId == id);

            if (id == null)
            {
                dy.Servicelist = GetServices();
            }
            else
            {

                dy.Servicelist = GetServices().Where(ser => ser.CategoryId == id);
            }


            return View(dy);
        }
        //Get Service Details Action Methed:
        public IActionResult ServiceDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = db.Services.Include(c => c.Category).FirstOrDefault(c => c.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);

        }

        [HttpPost]
        [ActionName("ServiceDetails")]
        public IActionResult Details(int? id)
        {

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };



            List<Service> services = new List<Service>();

            if (id == null)
            {
                return NotFound();
            }
            var service = db.Services.Include(c => c.Category).FirstOrDefault(c => c.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }


            services = HttpContext.Session.Get<List<Service>>("services");
            if (services == null)
            {
                services = new List<Service>();
            }
            services.Add(service);
            HttpContext.Session.Set("services", services);
            return RedirectToAction(nameof(Service));

        }


        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Service> services = HttpContext.Session.Get<List<Service>>("services");
            if (services != null)
            {
                var service = services.FirstOrDefault(s => s.ServiceId == id);
                if (service != null)
                {
                    services.Remove(service);
                    HttpContext.Session.Set("services", services);
                }
            }
            return RedirectToAction(nameof(Service));
        }

        //Get RemoveToCart Action Method:
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Service> services = HttpContext.Session.Get<List<Service>>("services");
            if (services != null)
            {
                var service = services.FirstOrDefault(s => s.ServiceId == id);
                if (service != null)
                {
                    services.Remove(service);
                    HttpContext.Session.Set("services", services);
                }
            }
            return RedirectToAction(nameof(Service));
        }

        //Get Checkout Action Method:
        public IActionResult Checkout(int? id)
        {

            //dynamic dy = new ExpandoObject();
            //dy.ServiceList = GetServices();
            //dy.InvoiceList = GetInvoices();
            return View();
        }


        // Post Checkout Action Method:

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Invoice anInvoice)
        {
            List<Service> services = HttpContext.Session.Get<List<Service>>("services");
            if (services != null)
            {
                foreach (var item in services)
                {
                    InvoiceDetail invoiceDetail = new InvoiceDetail();
                    invoiceDetail.ServiceId = item.ServiceId;
                    anInvoice.InvoiceDetails.Add(invoiceDetail);
                }
            }
            anInvoice.OrderNumber = GetInvoiceNum();
            db.Invoices.Add(anInvoice);
            await db.SaveChangesAsync();
            HttpContext.Session.Set("services", new List<Service>());
            return View();
        }

        public string GetInvoiceNum()
        {
            int rowCount = db.Invoices.ToList().Count();
            return rowCount.ToString("000");
        }



        //Model Dynamic Database:
        public List<Category> GetCategory()
        {
            List<Category> categories = db.Categories.ToList();
            return categories;
        }



        public List<Service> GetServices()
        {
            List<Service> services = db.Services.ToList();
            return services;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = db.Employees.ToList();
            return employees;
        }

        public List<Invoice> GetInvoices()
        {
            List<Invoice> invoices = db.Invoices.ToList();
            return invoices;
        }

        public List<InvoiceDetail> GetInvoiceDetails()
        {
            List<InvoiceDetail> invoiceDetails = db.InvoiceDetails.ToList();
            return invoiceDetails;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
