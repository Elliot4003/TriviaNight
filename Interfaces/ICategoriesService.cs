using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface ICategoriesService
    {
        CategoriesList GetCategories();
        int GetCategoryCount();
        void SaveCategories(CategoriesList categories);
    }
}