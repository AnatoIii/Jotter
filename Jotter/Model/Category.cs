using System;
using System.Runtime.Serialization;

namespace Model
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public bool IsPrivate { get; set; }

        public Category()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
