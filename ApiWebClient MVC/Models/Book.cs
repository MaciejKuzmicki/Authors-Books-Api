using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWebClient.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Book title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Author of the book is required")]
        public Author Author { get; set; }
        [Required(ErrorMessage ="Description is required")]
        public string Description { get; set; }

    }
}
