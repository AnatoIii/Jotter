using System;
using System.Collections.Generic;

namespace JotterAPI.DAL.Model
{
	public class Note : Entity
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        public List<File> Files { get; set; }

        public Category Category { get; set; }
    }
}
