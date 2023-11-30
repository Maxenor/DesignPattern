using System;
namespace Bibliotheque;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Author Author { get; set; }
    public Category Category { get; set; }
    public string Language { get; set; }
    public DateTime PublishDate { get; set; }
    private static int nextBookId = 1;

    public Book(int id, string title, Author author, Category category, string language, DateTime publishDate)
    {
        Id = id;
        Title = title;
        Author = author;
        Category = category;
        Language = language;
        PublishDate = publishDate;
    }

    public static int GetNextBookId()
    {
        return nextBookId++; 
    }

    public static void AddBook()
    {
        Console.WriteLine("Adding a new book.");

        int bookId = GetNextBookId(); 
        int authorId = Author.GetNextAuthorId(); 
        int categoryId = Category.GetNextCategoryId(); 

        // Détails du livre
        Console.Write("Book title : ");
        string title = Console.ReadLine();

        Console.Write("Author's name : ");
        string authorName = Console.ReadLine();
        string authorBiography = "Biographie par défaut";

        Console.Write("Book's category: ");
        string categoryName = Console.ReadLine();
        string categoryDescription = "Description par défaut";

        Console.Write("Book's language : ");
        string language = Console.ReadLine();

        Console.Write("Published date (format YYYY-MM-DD) : ");
        string publishDateString = Console.ReadLine();
        DateTime publishDate;
        while (!DateTime.TryParse(publishDateString, out publishDate))
        {
            Console.Write("Invalid format (format YYYY-MM-DD) : ");
            publishDateString = Console.ReadLine();
        }

        Author author = new Author(authorId, authorName, authorBiography);
        Category category = new Category(categoryId, categoryName, categoryDescription);

        Book newBook = new Book(bookId, title, author, category, language, publishDate);

        SaveBook(newBook);

        Console.WriteLine("Book added succesfully");
    }

    // save to json
    static void SaveBook(Book book)
    {
        string bookJson = System.Text.Json.JsonSerializer.Serialize(book);
        File.AppendAllText("books.json", bookJson + Environment.NewLine);

        Console.WriteLine("Book saved in 'books.json'.");
    }

}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
    public List<Book> BooksWritten { get; set; }
    private static int nextAuthorId = 1;
    public Author(int id, string name, string biography)
    {
        Id = id;
        Name = name;
        Biography = biography;
        BooksWritten = new List<Book>();
    }

    public static int GetNextAuthorId()
    {
        return nextAuthorId++; 
    }
    public void AddBook(Book book)
    {
        BooksWritten.Add(book);
    }
}
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Book> Books { get; set; }
    
    private static int nextCategoryId = 1;

    public Category(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        Books = new List<Book>();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public static int GetNextCategoryId()
    {
        return nextCategoryId++;
    }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<Loan> Loans { get; set; }
    public List<Author> FavoriteAuthors { get; set; }

    public Customer(int id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
        Loans = new List<Loan>();
        FavoriteAuthors = new List<Author>();
    }

    public void AddLoan(Loan loan)
    {
        Loans.Add(loan);
    }

    public void FollowAuthor(Author author)
    {
        FavoriteAuthors.Add(author);
    }
}

public class Order
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public Dictionary<int, int> BooksOrdered { get; set; }
    public string DeliveryAddress { get; set; }
    public IPaymentStrategy PaymentStrategy { get; set; }
    public double AmountDue { get; set; }

    public Order()
    {
        BooksOrdered = new Dictionary<int, int>();
    }

    public Order(int id, Customer customer, string deliveryAddress) : this()
    {
        Id = id;
        Customer = customer;
        DeliveryAddress = deliveryAddress;
    }

    public void AddBook(Book book, int quantity)
    {
        int bookId = book.Id;
        if (BooksOrdered.ContainsKey(bookId))
        {
            BooksOrdered[bookId] += quantity;
        }
        else
        {
            BooksOrdered.Add(bookId, quantity);
        }
    }
    public bool ProcessOrderPayment()
    {
        if (PaymentStrategy == null)
        {
            throw new InvalidOperationException("Payment strategy not set.");
        }
        return PaymentStrategy.ProcessPayment(AmountDue);
    }

    public static void OrderABook()
    {
        Console.WriteLine("Passer une commande");

        Console.Write("Entrez l'ID du client : ");
        int customerId = int.Parse(Console.ReadLine());
        Console.Write("Entrez le nom du client : ");
        string customerName = Console.ReadLine();

        Customer customer = new Customer(customerId, customerName, "Adresse du client");

     
        Console.Write("Entrez l'ID du livre commandé : ");
        int bookId = int.Parse(Console.ReadLine());

        Book book = new Book(bookId, "Titre du Livre", new Author(1, "Auteur", "Biographie"), new Category(1, "Catégorie", "Description"), "FR", DateTime.Now);

        var orderBuilder = new OrderBuilder();
        var order = orderBuilder
            .SetCustomer(customer)
            .SetDeliveryAddress("Adresse de livraison")
            .AddBook(book, 1) 
            .Build();

        SaveOrder(order);

        Console.WriteLine("Commande passée et enregistrée.");
    }

    static void SaveOrder(Order order)
    {
        string orderJson = System.Text.Json.JsonSerializer.Serialize(order);
        File.AppendAllText("orders.json", orderJson + Environment.NewLine);
        Console.WriteLine("Commande sauvegardée dans 'orders.json'.");
    }

}

class Program
{
    static void Main(string[] args)
    {
        // Customer customer = new Customer(1,"Maxime","130 rue d'Isitech");
        // Author maxime = new Author(1,"Maxime","Un super auteur");
        // Category aventure = new Category(1,"Aventure","Genre aventure");
        // Console.WriteLine(customer.Address);
        // Book book1 = new Book(1,"Les aventures de Maxime",maxime,aventure,"FR", new DateTime(20231129));
        // Book book2 = new Book(2,"Les aventures de Ilhan",maxime,aventure,"FR", new DateTime(20231129));
        // Console.WriteLine(book1.Title);

        // Order order = new OrderBuilder()
        //     .SetCustomer(customer)
        //     .SetDeliveryAddress("123, Rue Exemple")
        //     .AddBook(book1, 2)
        //     .AddBook(book2, 1)
        //     .Build();
    
        // Console.WriteLine(order.BooksOrdered);
        // order.AmountDue = 150.0;
        // order.PaymentStrategy = new PayPalPayment("test@test.fr", "test");
        // Console.WriteLine(order.PaymentStrategy);
        // bool isPaymentSuccessful = order.ProcessOrderPayment();
        // if (isPaymentSuccessful)
        // {
        //     Console.WriteLine("Payment succeed.");
        // }
        // else
        // {
        //     Console.WriteLine("Payment failed.");
        // }

        //  // Création d'un prêt
        // Loan loan = new Loan(book1, customer);

        // loan.NextState(new AvailableState());
        // // Passer le prêt à l'état "emprunté"
        // loan.NextState(new BorrowedState());

        // // Supposons que le livre est retourné
        // loan.NextState(new ReturnedState());

        // // Si le prêt est en retard
        // loan.NextState(new OverdueState());

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Choose an option :");
            Console.WriteLine("1: Add a book");
            Console.WriteLine("2: List all books");
            Console.WriteLine("3: Book Loan");
            Console.WriteLine("4: Order a book");
            Console.WriteLine("5: Quit");
            Console.Write("Enter your choice : ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Book.AddBook();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    Order.OrderABook();
                    break;
                case "5":
                    running = false;
                    Console.WriteLine("Exiting.");
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
