using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListORama.DataAccess;
using ListORama.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;

namespace ListORama.Controllers
{
    public class ListController : Controller
    {

        private ApplicationDBContext dbContext;

        public ListController(ApplicationDBContext context)
        {
            dbContext = context;
        }


        public IActionResult CreateList ()
        {
            return View();
        }

        public IActionResult AddList(UserList list)
        {
            String sessionList = HttpContext.Session.GetString("currentList");
            UserList currentList = null;
            if(!String.IsNullOrWhiteSpace(sessionList))
                currentList = JsonConvert.DeserializeObject<UserList>(sessionList);

            if (null != list && !String.IsNullOrWhiteSpace(list.listName))
            {
                String newItemAdded = list.newItem;
                UserListItem listItem = new UserListItem();
                listItem.itemName = newItemAdded;
                if (null != currentList)
                {
                    currentList.listName = list.listName;
                    list = currentList;
                }
                if (null == list.listItems)
                {
                    list.listItems = new List<UserListItem>();
                }
                list.listItems.Add(listItem);
                HttpContext.Session.SetString("currentList", JsonConvert.SerializeObject(list));
            }
            return View(list);
        }


        //        public async Task<ViewResult> CreateList(ShoppingList shoppingList)
        [HttpPost]
        public IActionResult CreateList(ItemList ListObjs)
        {
            ShoppingList shoppingListCreated = null;
            

/*            

            if (shoppingList != null && !String.IsNullOrWhiteSpace(shoppingList.listName))
            {
                try
                {
                    shoppingList.listID = dbContext.shoppingList
                                       .Max(c => c.listID);
                }

                catch (Exception ex)
                {
                    shoppingList.listID = 0;
                }
                
               

                shoppingList.listID = shoppingList.listID + 1;

                shoppingList.listStatus = "Open";


                dbContext.shoppingList.Add(shoppingList);
                dbContext.SaveChanges();

                // READ operation
                shoppingListCreated = dbContext.shoppingList
                                        .Where(c => c.listName == shoppingList.listName)
                                        .First();
                                          
            }
*/
                        return  View(shoppingListCreated);
//            return await Task.FromResult<ShoppingList>(shoppingListCreated);
        }





    }
}
