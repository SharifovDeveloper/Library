using Bogus;
using Exam06.Data;
using Exam06.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam06.Extensions
{
    public static class DatabaseSeeder
    {
        private static Faker _faker = new Faker();
        public static void SeedDatabase(this IServiceCollection _, IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<LibraryDbContext>>();
            using var context = new LibraryDbContext(options);
            CreateCategories(context);
            CreateAuthors(context);
            CreateBooks(context);
          

        }
        private static void CreateCategories(LibraryDbContext context)
        {
            if (context.Categories.Any()) return;

            List<string> categoryNames = new();
            List<Category> categories = new();

            for (int i = 0; i < 25; i++)
            {
                var categoryName = _faker.Commerce
                    .Categories(1)
                    .First();

                int attempts = 0;

                while (categoryNames.Contains(categoryName) && attempts < 100)
                {
                    categoryName = _faker.Commerce
                        .Categories(1)
                        .First();

                    attempts++;
                }

                categoryNames.Add(categoryName);
                categories.Add(new Category
                {
                    Name = categoryName,
                });
            }

            context.AddRange(categories);
            context.SaveChanges();
        }
        private static void CreateBooks(LibraryDbContext context)
        {
            if (context.Books.Any()) return;

            List<Book> books = new List<Book>();

            
            var categoryIds = context.Categories.Select(c => c.Id).ToList();

            for (int i = 0; i < 50; i++)
            {
                var book = new Book
                {
                    Title = _faker.Lorem.Word(),
                    Description = _faker.Lorem.Sentence(),
                    Price = _faker.Random.Decimal(10, 100),
                    CategoryId = _faker.Random.ListItem(categoryIds),
                    AuthorId = _faker.Random.Int(1, 10),
                };

                books.Add(book);
            }

            context.AddRange(books);
            context.SaveChanges();
        }

        private static void CreateAuthors(LibraryDbContext context)
        {
            if (context.Authors.Any()) return;

            List<Author> authors = new List<Author>();

            for (int i = 0; i < 10; i++)
            {
                var author = new Author
                {
                    FullName = _faker.Name.FullName(),
                    BirthDate = _faker.Date.Past().ToString("yyyy-MM-dd"),
                };

                authors.Add(author);
            }

            context.AddRange(authors);
            context.SaveChanges();
        }

     



    }
}
