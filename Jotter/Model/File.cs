using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class File : Entity
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public File(string name, string path)
        {
            Id = Guid.NewGuid();
            Name = name;
            Path = path;
        }
    }
}
