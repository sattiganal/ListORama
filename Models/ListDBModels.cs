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
        [System.ComponentModel.DataAnnotations.Key][System.ComponentModel.DataAnnotations.Schema.Column(Order = 1)]
        public String listName { get; set; }
        public String listStatus { get; set; }

        
        //public List<String> listItems { get; set; }
        [System.ComponentModel.DataAnnotations.Key][System.ComponentModel.DataAnnotations.Schema.Column(Order = 2)]
        public String listItems { get; set; }
        //        public String listItem2 { get; set; }
        //        public String listItem3 { get; set; }
    }    

    public class ItemList
    {
        public List<ShoppingList> ItemsList { get; set; }
    }

    public class UserListUserMap
    {
        [Key]
        public int mapId { get; set; }
        public UserList listId { get; set; }
        public User userId { get; set; }  
    }

    public class UserList
    {
        [Key]
        public int listId { get; set; }
        public String listName { get; set; }
        public String listDescription { get; set; }
        public String listType { get; set; }
        public List<UserListItem> listItems { get; set; }
        public List<UserListUserMap> listUserMaps { get; set; }
    }

    public class UserListItem
    {
        [Key]
        public int listItemId { get; set; }
        public String itemName { get; set; }
        public UserList list { get; set; }
    }
}
