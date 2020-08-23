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

        public IActionResult AddList(MyList list)
        {
            String sessionList = HttpContext.Session.GetString("currentList");
            MyList currentList = null;
            if(!String.IsNullOrWhiteSpace(sessionList))
                currentList = JsonConvert.DeserializeObject<MyList>(sessionList);
            String formAction = list.userAction;

            if (null != list && !String.IsNullOrWhiteSpace(list.listName))
            {
                if ("ADD".Equals(formAction))
                {
                    String newItemAdded = list.newItem;
                    MyListItem listItem = new MyListItem();
                    listItem.itemName = newItemAdded;
                    if (null != currentList)
                    {
                        currentList.listName = list.listName;
                        list = currentList;
                    }
                    if (null == list.listItems)
                    {
                        list.listItems = new List<MyListItem>();
                    }
                    list.listItems.Add(listItem);
                }
                else if("REMOVE".Equals(formAction))
                {
                    String removeItemName = list.listItemToRemove;
                    if (null != currentList)
                    {
                        currentList.listName = list.listName;
                        list = currentList;
                    }
                    MyListItem itemToRemove = null;
                    foreach(MyListItem item in list.listItems)
                    {
                        if (item.itemName.Equals(removeItemName))
                        {
                            itemToRemove = item;
                            break;
                        }
                    }
                    list.listItems.Remove(itemToRemove);
                }
                HttpContext.Session.SetString("currentList", JsonConvert.SerializeObject(list));
            }
            return View(list);
        }

        public IActionResult SaveList(UserList list)
        {
            String sessionList = HttpContext.Session.GetString("currentList");
            MyList currentList = null;
            if (!String.IsNullOrWhiteSpace(sessionList))
                currentList = JsonConvert.DeserializeObject<MyList>(sessionList);
            User loggedInUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("loggedInUser"));
            User listUser = dbContext.users
                                    .Where(c => c.userID == loggedInUser.userID)
                                    .FirstOrDefault();

            UserList listToSave = new UserList();
            List<UserListItem> itemsToSave = new List<UserListItem>();

            listToSave.listName = currentList.listName;
            listToSave.listType = "SHOPPING";
            listToSave.listItems = itemsToSave;
            foreach(MyListItem listeItem in currentList.listItems)
            {
                UserListItem itemToSave = new UserListItem();
                itemToSave.itemName = listeItem.itemName;
                itemToSave.list = listToSave;
                itemsToSave.Add(itemToSave);
                dbContext.listItems.Add(itemToSave);
            }

            UserListUserMap listUserMapping = new UserListUserMap();
            listUserMapping.listId = listToSave;
            listUserMapping.userId = listUser;

            dbContext.listUserMap.Add(listUserMapping);
            dbContext.lists.Add(listToSave);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard","User");
        }


        //        public async Task<ViewResult> CreateList(ShoppingList shoppingList)
        [HttpPost]
        public IActionResult CreateList(ItemList ListObjs)
        {
            User loggedInUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("loggedInUser"));
            User u = dbContext.users
                                    .Where(c => c.userID == loggedInUser.userID)
                                    .FirstOrDefault();
            ShoppingList shoppingListCreated = null;
            ItemList items = ListObjs;
            string sListName = " ";
            int maxShoppingListId = 0;
            int i = 0;
            foreach (var item in ListObjs.ItemsList)
            {
                shoppingListCreated = item;
                i++;
                if (i == 1)
                    sListName = shoppingListCreated.listName;
                else
                    shoppingListCreated.listName = sListName;


                if (shoppingListCreated != null && !String.IsNullOrWhiteSpace(shoppingListCreated.listName))
                {
                    if (i == 1)
                    {
                        try
                        {
                          maxShoppingListId =  shoppingListCreated.listID = dbContext.shoppingList
                                               .Max(c => c.listID);
                        }

                        catch (Exception ex)
                        {
                            shoppingListCreated.listID = 0;
                        }
                    }

                                        
                    shoppingListCreated.listID = maxShoppingListId + 1;

                    shoppingListCreated.listStatus = "Open";

                    dbContext.shoppingList.Add(shoppingListCreated);
                    dbContext.SaveChanges();

                    // READ operation
                    shoppingListCreated = dbContext.shoppingList
                                            .Where(c => c.listName == shoppingListCreated.listName)
                                            .First();

                }
            }
        
            return View(items);
        }





    }
}
