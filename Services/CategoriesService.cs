using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly TriviaNightDbContext _context;

        public CategoriesService(TriviaNightDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Enregistre les catégories dans le contexte au premier appel de l'API OTDB
        /// </summary>
        /// <param name="categories"></param>
        public void SaveCategories(CategoriesList categories)
        {
            if (categories.Categories != null)
            {
                // Insertion des questions dans la mémoire
                foreach (var category in categories.Categories)
                {
                    _context.Categories.Add(category);
                }

                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retourne le nombre de catégories dans le contexte
        /// </summary>
        /// <returns>Le nombre de catégories</returns>
        public int GetCategoryCount()
        {
            return _context.Categories.Count();
        }

        /// <summary>
        /// Retourne les catégories présentes dans le contexte
        /// </summary>
        /// <returns>Les catégories</returns>
        public CategoriesList GetCategories()
        {
            var categories = new CategoriesList();
            categories.Categories = []; // Initialisation
            foreach (var category in _context.Categories)
            {
                categories.Categories.Add(category);
            }
            return categories;
        }
    }
}
