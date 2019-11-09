using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Models.Data;

namespace WebStore.Models.ViewModels.Shop
{
    public class CategoryVM
    {
        public CategoryVM()
        {        }
        public CategoryVM(CategoryDTO model)
        {
            Id = model.Id;
            Name = model.Name;
            Slug = model.Slug;
            Sorting = model.Sorting;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}