using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebDemo.Models
{
    public class UsersEntity : TableEntity
    {

        public UsersEntity(string firstName, string lastName)
        {
            this.RowKey = firstName;
            this.PartitionKey = lastName;
        }


        public UsersEntity()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string Email { get; set; }
        public string Phone { get; set; }
    }
}