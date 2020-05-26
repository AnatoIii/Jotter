using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public Category(string name, string password = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Password = password;
        }

        public Category()
        {
            Id = Guid.NewGuid();
        }
    }
}
