using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ListORama.DataAccess;
using ListORama.Models;
using ListORama.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ListORama.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDBContext dbContext;
        private IMemoryCache _cache;

        [TempData]
        public int currentUserUserId { get; set; }
        public UserController(ApplicationDBContext context, IMemoryCache cache)
        {
            dbContext = context;
            _cache = cache;
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
                if (!userExists(user.email))
                {
                    byte[] salt = CreateSalt();
                    User newUser = new User();
                    newUser.email = user.email;
                    newUser.firstName = user.firstName;
                    newUser.lastName = user.lastName;
                    newUser.salt = salt;
                    newUser.password = GenerateSaltedHash(Encoding.UTF8.GetBytes(user.pswdString), salt);
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
                                            .FirstOrDefault();
                }
                else
                {
                    ViewBag.message = "User with email " + user.email + " aleady exists!!!";
                }
            }
            return View(userCreated);
        }

        private Boolean userExists(String userEmail)
        {
            User user = dbContext.users
                                        .Where(c => c.email == userEmail)
                                        .FirstOrDefault();
            return !(null == user);
        }

        public IActionResult Login(User user)
        {
            if (user != null && !String.IsNullOrWhiteSpace(user.email))
            {
                User u = dbContext.users
                                    .Where(c => c.email == user.email)
                                    .FirstOrDefault();
                if (null != u)
                {
                    String userPswd = user.pswdString;
                    byte[] suppliedPswd = GenerateSaltedHash(Encoding.UTF8.GetBytes(userPswd), u.salt);
                    if (CompareByteArrays(suppliedPswd, u.password))
                    {
                        HttpContext.Session.SetString("loggedInUser", JsonConvert.SerializeObject(u));
                        return RedirectToAction("Dashboard", "User");
                    }

                }

                ViewBag.message = "Invalid Username/Password.";

            }
            return View();
        }

        public IActionResult Dashboard()
        {
            User loggedInUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("loggedInUser"));
            if (null == loggedInUser)
            {
                return RedirectToAction("Login", "User");
            }

            List<UserListUserMap> userListMap = dbContext.listUserMap
                                                    .Include(p => p.listId)
                                                    .Include(p => p.listId.listItems)
                                                    .Include(p => p.userId)
                                                    .Where(c => c.userId.userID == loggedInUser.userID).ToList<UserListUserMap>();
            List<MyList> listsOwnded = new List<MyList>();
            foreach (UserListUserMap map in userListMap)
            {
                UserList list = map.listId;
                MyList dashBoardList = new MyList();
                listsOwnded.Add(dashBoardList);
                dashBoardList.listId = list.listId;
                dashBoardList.listName = list.listName;
                dashBoardList.description = list.listDescription;
                dashBoardList.listType = list.listType;
                foreach (UserListItem item in list.listItems)
                {
                    MyListItem listItem = new MyListItem();
                    listItem.itemName = item.itemName;
                }

            }

            Dashboard userDashBoard = new Dashboard();
            userDashBoard.listsOwned = listsOwnded;
            return View(userDashBoard);
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
                group1 = new UserGroup();
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

        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        private static byte[] CreateSalt()
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[8];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Encoding.UTF8.GetBytes(Convert.ToBase64String(buff));
        }

        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public IActionResult StoreOffers()
        {

            Stores stores = HomeController.SearchOffers();
            List<Store> usStoresWithOffers = new List<Store>();
            foreach (Store store in stores.stores)
            {
                String country = store.location;
                if (!String.IsNullOrWhiteSpace(country))
                {
                    String[] countries = country.Split(",");
                    foreach (String countryCode in countries)
                    {
                        if ("United States".Equals(countryCode) || "USA".Equals(countryCode))
                        {
                            usStoresWithOffers.Add(store);
                        }
                    }
                }
            }

            List<Store> sampledUSStores = null;
            if (!_cache.TryGetValue<List<Store>>("us.sample.stores", out sampledUSStores))
            {
                sampledUSStores = GetSampledUSStores(usStoresWithOffers);
                _cache.Set<List<Store>>("us.sample.stores", sampledUSStores);
            }
                
            Stores usStores = new Stores();
            usStores.result = "";
            usStores.stores = sampledUSStores.ToArray();
            return View(usStores);
        }

        public static List<Store>  GetSampledUSStores(List<Store> allUSStores)
        {
            List<Store> sampledUSStores = new List<Store>();
            int i = 0;
            foreach (Store element in allUSStores.OrderByDescending(key => key.name))
            {
                if (i == 20)
                {
                    break;
                }
                StorePreview preview = HomeController.GetPreview(element);
                element.preview = preview;
                sampledUSStores.Add(element);
                i++;

            }
            return sampledUSStores;
        }
    }
}