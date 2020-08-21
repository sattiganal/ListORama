using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListORama.Models
{
    [Table("ListGroups")]
    public class ListGroup
    {
        public int listGroupID { get; set; }
        public string listGroupName { get; set; }
        public int userID { get; set; }
    }
}
