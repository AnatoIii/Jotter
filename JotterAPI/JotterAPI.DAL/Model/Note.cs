using System;
using System.Collections.Generic;
using System.Text;

namespace JotterAPI.DAL.Model
{
	public class Note
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string CategoryId { get; set; }

        public List<File> Files { get; set; }
        
        public Category Category { get; set; }
    }
}
