using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Note : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }

        public List<File> Files { get; set; }
        public Category Category { get; set; }

        public Note(string name, string description, Category category)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Category = category;
            Files = new List<File>();
        }

        public Note()
        {
            Id = Guid.NewGuid();
            Files = new List<File>();
        }

        public void AddFile(File file)
        {
            Files.Add(file);
        }

        public void RemoveFile(File file)
        {
            Files.Remove(file);
        }
    }
}
