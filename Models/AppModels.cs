﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListORama.Models
{
    public class Groups
    {
        public UserGroup[] groups { get; set; }
    }

    public class Chart
    {
        public string chartTitle { get; set; }
        public string data { get; set; }
        public string chartType { get; set; }
        public string labels { get; set; }
        public string backgroundColors { get; set; }
    }

    public class Parks
    {
        public string total { get; set; }
        public Park[] data { get; set; }
        public string limit { get; set; }
        public string start { get; set; }
    }

    public class Park
    {
        public string states { get; set; }
        public string longitude { get; set; }
        public string directionsInfo { get; set; }
        public string directionsUrl { get; set; }
        public string url { get; set; }
        public string weatherInfo { get; set; }
        public string name { get; set; }
        public string latLong { get; set; }
        public string description { get; set; }
        public string designation { get; set; }
        public string parkCode { get; set; }
        public string id { get; set; }
        public string fullName { get; set; }
        public string latitude { get; set; }
    }

    public class MyList
    {
        public int listId { get; set; }
        public String listName { get; set; }
        public String description { get; set; }
        public String listType { get; set; }
        public String newItem { get; set; }
        public List<MyListItem> listItems { get; set; }
        public String userAction { get; set; }
        public String listItemToRemove { get; set; }
    }

    public class MyListItem
    {
        public String itemName { get; set; }
    }

    public class Dashboard
    {
        public List<MyList> listsOwned { get; set; }
    }

    public class Store
    {
        public String name { get; set; }
        public String location { get; set; }
        public String URL { get; set; }
        public StorePreview preview { get; set; }
    }
    public class Stores
    {
        public String result { get; set; }
        public Store[] stores { get; set; }
    }

    public class StorePreview
    {
        public String image { get; set; }
        public String title { get; set; }
        public String description { get; set; }
        public String url { get; set; }
    }
}
