using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListORama.Models
{
    public class ListGroupUser
    {
        public int listGroupUserID { get; set; }
        public ListGroup listGroup { get; set; }

        public User user { get; set; }
       
        public int userID { get; set; }
        public int listGroupID { get; set; }

    }
}



