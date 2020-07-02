using System;
using System.Collections.Generic;
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
}
