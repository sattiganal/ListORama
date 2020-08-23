using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListORama.Models
{
    
    public class Lists
    {
        public List[] list { get; set; }
    }

    public class List
    {
        public int listID { get; set; }
        public String listName { get; set; }
        public String listStatus { get; set; }
    }


    public class ShoppingList
    {
        public int listID { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        public String listName { get; set; }
        public String listStatus { get; set; }

        
        //public List<String> listItems { get; set; }

        public String listItems { get; set; }
//        public String listItem2 { get; set; }
//        public String listItem3 { get; set; }
    }    

    public class ItemList
    {
        public List<ShoppingList> ItemsList { get; set; }
    }

    public class UserList
    {
        [Key]
        public int listId { get; set; }
        public String listName { get; set; }
        public String listType { get; set; }
        public String newItem { get; set; }
        public List<UserListItem> listItems { get; set; }
    }

    public class UserListItem
    {
        [Key]
        public int listItemId { get; set; }
        public String itemName { get; set; }
    }
}
