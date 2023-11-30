namespace Bibliotheque;

public interface ILoanState
{
    void Handle(Loan loan);
}

public class AvailableState : ILoanState
{
    public void Handle(Loan loan)
    {
        Console.WriteLine("Le livre est disponible pour le prêt.");
    }
}

public class BorrowedState : ILoanState
{
    public void Handle(Loan loan)
    {
        Console.WriteLine("Le livre est actuellement emprunté.");
    }
}

public class OverdueState : ILoanState
{
    public void Handle(Loan loan)
    {
        Console.WriteLine("Le prêt est en retard.");
    }
}

public class ReturnedState : ILoanState
{
    public void Handle(Loan loan)
    {
        Console.WriteLine("Le livre a été retourné.");
    }
}

public class Loan
{
    public ILoanState State { get; set; }
    public Book Book { get; set; }
    public Customer Customer { get; set; }

    public Loan(Book book, Customer customer)
    {
        Book = book;
        Customer = customer;
        State = new AvailableState(); // État initial
    }

    public void NextState(ILoanState newState)
    {
        State = newState;
        State.Handle(this);
    }
}
