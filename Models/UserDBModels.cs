﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace ListORama.Models
{
    public class User
    {
        [Key]
        public int userID { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public byte[] password { get; set; }
        public String pswdString { get; set; }
        public byte[] salt { get; set; }
        public string fullName
        {
            get
            {
                return (lastName + ", " + firstName);
            }
        }
        public String address1 { get; set; }
        public String address2 { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zip { get; set; }
        public List<UserGroupMember> userGroupMembership { get; set; }
        public List<UserListUserMap> userListMaps { get; set; }
    }

    public class UserGroup
    {
        [Key]
        public int groupID { get; set; }
        public string groupName { get; set; }
        public User groupOwner { get; set; }
        public String groupStatus { get; set; }
        public List<UserGroupMember> groupMembers { get; set; }
    }

    public class UserGroupMember
    {
       [Key]
       public int userGroupMemberId { get; set; }
       public User user { get; set; }
       public UserGroup group { get; set; }
    }

    public class UserCred
    {
        [Key]
        public int userCredId { get; set; }
        public String userName { get; set; }
        public String password { get; set; }
    }
}
