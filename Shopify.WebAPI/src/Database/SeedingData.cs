using Shopify.Core.src.Entity;
using Shopify.Service.src.Shared;

namespace Shopify.WebAPI.src.Database;

public class SeedingData
{
  private static readonly Category gameCategory = new()
  {
    Id = Guid.NewGuid(),
    Image = $"https://picsum.photos/seed/{new Random().Next()}/1024",
    Name = "Game",
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow
  };
  private static readonly Category movieCategory = new()
  {
    Id = Guid.NewGuid(),
    Image = $"https://picsum.photos/seed/{new Random().Next()}/1024",
    Name = "Movie",
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow
  };

  private static readonly Product product1 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Doom",
    Description = "A classic first-person shooter released in 1993 known for its intense gameplay and iconic monsters.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product2 = new()
  {
    Id = Guid.NewGuid(),
    Title = "The Elder Scrolls: Arena",
    Description = "An epic role-playing game released in 1994, the first installment in The Elder Scrolls series.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product3 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Command & Conquer",
    Description = "A groundbreaking real-time strategy game released in 1995, featuring intense battles and strategic warfare.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product4 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Warcraft II: Tides of Darkness",
    Description = "A classic real-time strategy game released in 1995, known for its engaging campaigns and multiplayer battles.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product5 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Quake",
    Description = "A revolutionary first-person shooter released in 1996, featuring advanced 3D graphics and fast-paced action.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product6 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Diablo",
    Description = "A dark and atmospheric action role-playing game released in 1996, known for its addictive gameplay and dungeon crawling.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product7 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Age of Empires",
    Description = "A historical real-time strategy game released in 1997, allowing players to build and lead civilizations through the ages.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product8 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Final Fantasy VII",
    Description = "An iconic role-playing game released in 1997, featuring a rich narrative and memorable characters in a fantasy world.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product9 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Half Life",
    Description = "A groundbreaking first-person shooter released in 1998, known for its immersive storytelling and innovative gameplay.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product10 = new()
  {
    Id = Guid.NewGuid(),
    Title = "StarCraft",
    Description = "A highly acclaimed real-time strategy game released in 1998, set in a futuristic science fiction universe.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product11 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Baldur's Gate",
    Description = "A classic role-playing game released in 1998, based on the Dungeons & Dragons universe, featuring epic adventures.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product12 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Counter-Strike",
    Description = "A popular multiplayer first-person shooter released in 1999, known for its intense team-based gameplay.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product13 = new()
  {
    Id = Guid.NewGuid(),
    Title = "System Shock 2",
    Description = "A sci-fi horror first-person shooter released in 1999, combining elements of role-playing and survival horror.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product14 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Heroes of Might and Magic III",
    Description = "A fantasy turn-based strategy game released in 1999, known for its deep gameplay and strategic battles.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product15 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Planescape: Torment",
    Description = "A critically acclaimed role-playing game released in 1999, celebrated for its deep narrative and unique setting.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product16 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Unreal Tournament",
    Description = "A fast-paced multiplayer first-person shooter released in 1999, featuring a variety of game modes and maps.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product17 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Age of Empires II: The Age of Kings",
    Description = "The highly praised sequel to Age of Empires, released in 1999, offering enhanced graphics and new civilizations.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product18 = new()
  {
    Id = Guid.NewGuid(),
    Title = "The Sims",
    Description = "A popular life simulation game released in 2000, allowing players to create and control virtual people.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product19 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Deus Ex",
    Description = "A cyberpunk-themed first-person shooter released in 2000, known for its immersive story and player choices.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product20 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Diablo",
    Description = "An atmospheric action RPG released in 1996 where players descend into the depths of hell, battling demonic forces and uncovering dark secrets",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product21 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Diablo II",
    Description = "The highly acclaimed action role-playing game released in 2000, featuring a vast world and addictive gameplay.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product22 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Theme Hospital",
    Description = "A quirky simulation game released in 1997 that challenges players to design, manage, and cure comical ailments in their very own hospital.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product23 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Populous: The Beginning",
    Description = "A strategic god-game released in 1998, inviting players to command powerful shamanic abilities and lead their tribe to dominance in a dynamic, evolving world.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product24 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Dungeon Keeper",
    Description = "A dark and humorous strategy game released in 1997, where players take on the role of an evil overlord, building and defending a dungeon while unleashing wicked forces upon meddling heroes.",
    Price = 19.9m,
    CategoryId = gameCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  private static readonly Product product25 = new()
  {
    Id = Guid.NewGuid(),
    Title = "Mortal Kombat (1995)",
    Description = "a martial arts fantasy film based on the video game series of the same name. The movie follows a group of Earth's greatest warriors as they enter a tournament to save the world from the evil sorcerer Shang Tsung.",
    Price = 19.9m,
    CategoryId = movieCategory.Id,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
  };

  public static List<Category> GetCategories()
  {
    return new List<Category> { gameCategory, movieCategory };
  }

  public static List<Product> GetProducts()
  {
    return new List<Product>
    {
      product1, product2, product3, product4, product5, product6, product7,product8,product9,product10,product11,product12,
      product13, product14, product15, product16,product17,product18,product19,product20,product21,product22,product23,product24,
      product25
    };
  }

  public static List<User> GetUsers()
  {
    PasswordService.HashPassword("admin", out string salt, out string hashedPassword);
    return new List<User>
    {
      new()
        {
          Id = Guid.NewGuid(),
          Role = Role.Admin,
          Email = "admin@mail.com",
          FirstName ="Admin",
          LastName="User",
          Avatar = $"https://picsum.photos/seed/{new Random().Next()}/1024",
          Salt = salt,
          Password = hashedPassword,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow,
        }
    };
  }

  public static List<Image> GetImages()
  {
    var images = new List<Image>();

    foreach (var product in GetProducts())
    {
      var random = new Random();
      for (int i = 0; i < 3; i++)
      {
        images.Add(new Image
        {
          Id = Guid.NewGuid(),
          ProductId = product.Id,
          URL = $"https://picsum.photos/seed/{random.Next()}/1024",
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow,
        });
      }
    }

    return images;
  }
}