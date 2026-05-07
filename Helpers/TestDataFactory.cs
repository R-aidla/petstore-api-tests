using PetstoreApiTests.Models;

namespace PetstoreApiTests.Helpers;

public static class TestDataFactory
{
    public static readonly long TestID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// Create a pet using this function. If each param is not overwritten,
    /// it will create a Fantasy Fire Dragon test pet by default,
    /// </summary>
    /// <returns>Creates and returns a <see cref="Pet"/> object</returns>
    public static Pet CreatePet(long id, string name = "Dragon", Category? category = null, List<string>? photoUrls = null, List<Tag>? tags = null)
    {
        return new Pet
        {
            Id = id,
            Name = name,
            Status = "available",

            Category = category ?? new Category
            {
                Id = 1,
                Name = "Fantasy"
            },

            PhotoUrls = photoUrls ?? new List<string>
            {
                "https://example.com/dragon.png"
            },

            Tags = tags ?? new List<Tag>
            {
                new Tag
                {
                    Id = 1,
                    Name = "Fire"
                }
            }
        };
    }
}