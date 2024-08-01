using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.Models.ViewModels
{
    public class ArticleViewModel
    {
        public Article Article { get; set; }

        public IEnumerable<SelectListItem> ListCategory { get; set; }

    }
}
