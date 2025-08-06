using Bogus;
using FakeBookGenerator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeBookGenerator.Server.Services;

public class BookGeneratorService
{
    public IEnumerable<Book> GenerateBooks(BookQueryParameters query)
    {
        // The effective seed combines the user's seed and the page number
        // to ensure each page is unique but reproducible.
        int effectiveSeed = query.Seed + query.Page;

        // Set the locale for the faker. Defaults to "en" if the provided locale is invalid.
        var bookFaker = new Faker<Book>(query.Locale)
            .UseSeed(effectiveSeed)
            .RuleFor(b => b.Id, f => f.IndexGlobal)
            .RuleFor(b => b.Isbn, f => f.Commerce.Ean13())
            .RuleFor(b => b.Title, f => f.Commerce.ProductName()) // Or use custom title logic
            .RuleFor(b => b.Publisher, f => f.Company.CompanyName());

        // This is the CRITICAL part for reproducible but independent data.
        // We seed a separate Randomizer for book-specific details.
        bookFaker.FinishWith((f, book) =>
        {
            // Get a random seed for this specific book's details
            var detailSeed = f.Random.Int(); 
            
            var authorFaker = new Faker<Author>(query.Locale)
                .UseSeed(detailSeed)
                .RuleFor(a => a.FirstName, f_auth => f_auth.Name.FirstName())
                .RuleFor(a => a.LastName, f_auth => f_auth.Name.LastName());

            var reviewFaker = new Faker<Review>(query.Locale)
                .UseSeed(detailSeed) // Use the same seed for reviews for consistency
                .RuleFor(r => r.ReviewerName, f_rev => f_rev.Name.FullName())
                .RuleFor(r => r.Text, f_rev => f_rev.Rant.Review());

            var bookRandomizer = new Randomizer(detailSeed);

            book.Authors = authorFaker.Generate(bookRandomizer.Number(1, 3));

            // Handle fractional reviews (e.g., an average of 2.5 reviews per book)
            if (bookRandomizer.Double() < (query.Reviews - Math.Floor(query.Reviews)))
                book.Reviews = reviewFaker.Generate((int)Math.Ceiling(query.Reviews));
            else
                book.Reviews = reviewFaker.Generate((int)Math.Floor(query.Reviews));

            // Handle fractional likes (e.g., an average of 4.2 likes per book)
            if (bookRandomizer.Double() < (query.Likes - Math.Floor(query.Likes)))
                book.Likes = (int)Math.Ceiling(query.Likes);
            else
                book.Likes = (int)Math.Floor(query.Likes);
        });

        // For infinite scroll, generate 20 books for the first page and 10 for subsequent pages.
        int batchSize = query.Page == 1 ? 20 : 10;
        return bookFaker.Generate(batchSize);
    }
}
