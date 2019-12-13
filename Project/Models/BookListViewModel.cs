using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Models.Entities;

namespace Project.Models
{
    public class BookListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}