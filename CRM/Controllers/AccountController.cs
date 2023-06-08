using CRM.Models;
using CRM.Models.Data;
using CRM.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Controllers
{   
   
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public AccountController(CDbContext context, IWebHostEnvironment environment)
        {
            Context = context;
            this.environment = environment;
        }

        public CDbContext Context { get; }

        public IActionResult Index()
        {
            List<Studentmodel>
                ind = Context.Studentmodels.ToList();
            return View(ind);
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync(string search)
        {
            ViewData["GetStudentDetail"] = search;
            var studquery = from x in Context.Studentmodels select x;
            if (!string.IsNullOrEmpty(search))
            {

                studquery = studquery.Where(x => x.Mobile.Contains(search) || x.Email.Contains(search));
            }

            return View(await studquery.AsNoTracking().ToListAsync());
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Login(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = Context.Users.Where(e => e.Username == model.Username).SingleOrDefault();
                if (data != null)
                {
                    bool isvalid = (data.Username == model.Username && data.Password == model.Password);
                    if (isvalid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Username) },
                         CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("Index", "home");

                    }
                    else
                    {
                        TempData["errorPassword"] = "invalid password!";
                        return View(model);
                    }
                }

                else
                {
                    TempData["erroruserName"] = "UserName not found!";
                    return View(model);
                }

            }
            else
            {
                return View(model);
            }

           
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var StoredCookies = Request.Cookies.Keys;
            foreach(var Cookies in StoredCookies)
            {
                Response.Cookies.Delete(Cookies);
            }
            return RedirectToAction("Login", "Account");
        }
    

        [AcceptVerbs("Post","Get")]
        public IActionResult UserNameIsExite(string userNmae)
        {
            var data = Context.Users.Where(e => e.Username == userNmae).SingleOrDefault();
            if(data!= null)
            {
                return Json($"Username{userNmae} already in use!");
            }
            else
            {
                return Json(true);
            }
        }

        public IActionResult SignUp()
        {
            return View();
        } 
        [HttpPost] 

        public IActionResult SignUp(SignUpUserViewModel model)
        { 
            if(ModelState.IsValid)
            {
                var data = new User()
                {
                   Username= model.Username,
                   Email=model.Email,
                   Password=model.Password,
                   Mobile=model.Mobile,
                   isActive=model.isActive
                };

                Context.Users.Add(data);
                Context.SaveChanges();
                TempData["SuccessMessage"] = "you are eligibal to login,please fill own cirdential's them login!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["errorMessage"] = "Empty From con't be submitted";
                return View(model);
            }
           
        }



        public IActionResult Create()
        {
            ViewBag.Studentmodel = new SelectList(GetDropmodels(), "Id", "Subject");
            return View();
        }
        [HttpPost]


        public IActionResult Create(Studentmodel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = UploadImage(model);
                    var data = new Studentmodel()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        Gender = model.Gender,
                        StudentFee = model.StudentFee,
                        Discription = model.Discription,
                        Subject = model.Subject,
                        Dob = model.Dob,
                        Path = uniqueFileName
                    };
                    Context.Studentmodels.Add(data);
                    Context.SaveChanges();
                    TempData["Success"] = "Record SuccessFully saved!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Model property is not valid,please check");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

        private string UploadImage(Studentmodel model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "Content/Laptop/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


       
        public IActionResult Edit(int id)
        {
            ViewBag.Studentmodel = new SelectList(GetDropmodels(), "Id", "Subject");

            var data = Context.Studentmodels.Where(e => e.Id == id).SingleOrDefault();
            if (data != null)
            {
                return View(data);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
       
        public IActionResult Edit(Studentmodel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = Context.Studentmodels.Where(e => e.Id == model.Id).SingleOrDefault();
                    string uniqueFileName = string.Empty;
                    if (model.ImagePath != null)
                    {
                        string filepath = Path.Combine(environment.WebRootPath, "Content/Laptop/", data.Path);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                        uniqueFileName = UploadImage(model);
                    }
                    data.Name = model.Name;
                    data.Email = model.Email;
                    data.Mobile = model.Mobile;
                    data.Gender = model.Gender;
                    data.StudentFee = model.StudentFee;
                    data.Discription = model.Discription;
                    data.Dob = model.Dob;
                    data.Subject = model.Subject;
                    if (model.ImagePath != null)
                    {
                        data.Path = uniqueFileName;
                    }
                    Context.Studentmodels.Update(data);
                    Context.SaveChanges();
                    ViewBag.Studentmodel = new SelectList(GetDropmodels(), "Id", "Subject");

                    TempData["Success"] = "Record Update Successfully!";
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Index");
        }



        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = Context.Studentmodels.Where(e => e.Id == id).SingleOrDefault();
                if (data != null)
                {
                    string deleteFormFolder = Path.Combine(environment.WebRootPath, "Content/Laptop/");
                    string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFormFolder, data.Path);
                    if (currentImage != null)
                    {
                        if (System.IO.File.Exists(currentImage))
                        {
                            System.IO.File.Delete(currentImage);
                        }
                    }
                    Context.Studentmodels.Remove(data);
                    Context.SaveChanges();
                    TempData["Success"] = "Record Deleted!";
                }
            }
            return RedirectToAction("Index");
        }





        private List<Studentmodel> GetDropmodels()
        {
            return new List<Studentmodel>()
            {
                new Studentmodel(){ Id=1,Subject="Hindi"},
                 new Studentmodel(){ Id=2,Subject="English"},
                  new Studentmodel(){ Id=3,Subject="Maths"},
                   new Studentmodel(){ Id=4,Subject="Computer"},
                    new Studentmodel(){ Id=5,Subject="Physics"},
                     new Studentmodel(){ Id=6,Subject="SST"},
                      new Studentmodel(){ Id=7,Subject="Chemistry"},
                       new Studentmodel(){ Id=8,Subject="Biology"},
                        new Studentmodel(){ Id=9,Subject="GK"}


            };

        }



















    }
}
