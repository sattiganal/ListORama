using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListORama.DataAccess;
using ListORama.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListORama.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDBContext dbContext;

        public UserController(ApplicationDBContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ViewResult> SignUp(
            String inputEmail,
            String inputPassword,
            String inputFirstName,
            String inputLastName,
            String inputAddress1,
            String inputAddress2,
            String inputCity,
            String inputState,
            String inputZip)
        {
            User userCreated = null;
            if (!String.IsNullOrWhiteSpace(inputEmail))
            {
                User newUser = new User();
                newUser.email = inputEmail;
                newUser.firstName = inputFirstName;
                newUser.lastName = inputLastName;
                newUser.password = inputPassword.GetHashCode();
                dbContext.users.Add(newUser);
                dbContext.SaveChanges();

                // READ operation
                userCreated = dbContext.users
                                        .Where(c => c.email == inputEmail)
                                        .First();
            }
            return View(userCreated);
        }
    }
}