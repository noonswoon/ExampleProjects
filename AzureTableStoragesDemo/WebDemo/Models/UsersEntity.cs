using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebDemo.Models
{
    [DataContract]
    public class UsersEntity : TableEntity
    {
        public UsersEntity(string firstname, string lastname)
        {
            this.PartitionKey = lastname;
            this.RowKey = firstname;
        }

        public UsersEntity()
        {
        }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}