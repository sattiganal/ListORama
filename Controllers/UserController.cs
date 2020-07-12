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

        public IActionResult CreateUserGroup()
        {

            // CREATE Data
            User user1 = dbContext.users
                                    .Where(c => c.email == "jon.doe@something.com")
                                    .First();
            if (user1 == null)
            {
                user1 = new User();
                user1.email = "jon.doe@something.com";
                user1.firstName = "Jon";
                user1.lastName = "Doe";
            }

            User user2 = dbContext.users
                        .Where(c => c.email == "jak.smith@something.com")
                        .First();
            if (user2 == null)
            {
                user2 = new User();
                user2.email = "jak.smith@something.com";
                user2.firstName = "Jak";
                user2.lastName = "Smith";
            }

            User user3 = dbContext.users
                        .Where(c => c.email == "jon.smith@something.com")
                        .First();
            if (user3 == null)
            {
                user3 = new User();
                user3.email = "jon.smith@something.com";
                user3.firstName = "Jon";
                user3.lastName = "Smith";
            }

            User user4 = dbContext.users
                        .Where(c => c.email == "jon.smith@something.com")
                        .First();
            if (user4 == null)
            {
                user4 = new User();
                user4.email = "pat.sims@something.com";
                user4.firstName = "Pat";
                user4.lastName = "Sims";
            }


            UserGroup group1 = dbContext.groups
                        .Where(c => c.groupName == "Jon Doe's Group")
                        .First();
            if (group1 == null)
            {
                group1 =  new UserGroup();
                group1.groupName = "Jon Doe's Group";
                group1.groupOwner = user1;
                group1.groupStatus = "Active";
            }

            UserGroup group2 = dbContext.groups
                        .Where(c => c.groupName == "Pat's Group")
                        .First();
            if (group2 == null)
            {
                group2 = new UserGroup();
                group2.groupName = "Pat's Group";
                group2.groupOwner = user4;
                group2.groupStatus = "Inactive";
            }

            UserGroupMember groupMembership1 = new UserGroupMember();
            groupMembership1.user = user1;
            groupMembership1.group = group1;

            UserGroupMember groupMembership2 = new UserGroupMember();
            groupMembership2.user = user2;
            groupMembership2.group = group1;

            UserGroupMember groupMembership3 = new UserGroupMember();
            groupMembership3.user = user3;
            groupMembership3.group = group1;

            UserGroupMember groupMembership4 = new UserGroupMember();
            groupMembership4.user = user1;
            groupMembership4.group = group2;

            UserGroupMember groupMembership5 = new UserGroupMember();
            groupMembership5.user = user4;
            groupMembership5.group = group2;

            dbContext.groupMemberships.Add(groupMembership1);
            dbContext.groupMemberships.Add(groupMembership2);
            dbContext.groupMemberships.Add(groupMembership3);
            dbContext.groupMemberships.Add(groupMembership4);
            dbContext.groupMemberships.Add(groupMembership5);
            dbContext.SaveChanges();

            // READ Data
            UserGroup userGroupActive = dbContext.groups
                                    .Where(c => c.groupStatus == "Active")
                                    .First();

            return View(userGroupActive);
        }
    }
}