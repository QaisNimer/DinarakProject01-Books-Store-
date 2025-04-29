using DinarakProject01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinarakProject01.ViewModel
{
    public class ItemsViewModel
    {
        public List<DinarakClass> Items { get; set; }
        public List<BookClass> ItemsBook { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((double)TotalItems / PageSize); }
        }
    }
}