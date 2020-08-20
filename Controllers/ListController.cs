using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListORama.DataAccess;
using ListORama.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace ListORama.Controllers
{
    public class ListController : Controller
    {

        private ApplicationDBContext dbContext;

        public ListController(ApplicationDBContext context)
        {
            dbContext = context;
        }


        //        public async Task<ViewResult> CreateList(ShoppingList shoppingList)
        public IActionResult CreateList(ShoppingList shoppingList)
        {
            ShoppingList shoppingListCreated = null;
//            [HttpPost]
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
                        return  View(shoppingListCreated);
//            return await Task.FromResult<ShoppingList>(shoppingListCreated);
        }





    }
}
