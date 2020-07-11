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

        public async Task<ViewResult> SignUp(User user)
        {
            User userCreated = null;
            if (user != null && !String.IsNullOrWhiteSpace(user.email))
            {
                User newUser = new User();
                newUser.email = user.email;
                newUser.firstName = user.firstName;
                newUser.lastName = user.lastName;
                newUser.password = user.password.GetHashCode();
                newUser.address1 = user.address1;
                newUser.address2 = user.address2;
                newUser.city = user.city;
                newUser.state = user.state;
                newUser.zip = user.zip;
                dbContext.users.Add(newUser);
                dbContext.SaveChanges();

                // READ operation
                userCreated = dbContext.users
                                        .Where(c => c.email == user.email)
                                        .First();
            }
            return View(userCreated);
        }
    }
}