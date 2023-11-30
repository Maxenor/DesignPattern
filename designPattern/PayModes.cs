namespace Bibliotheque;

public interface IPaymentStrategy
{
    bool ProcessPayment(double amount);
}

public class CreditCardPayment : IPaymentStrategy
{
    private string CardNumber;
    private string ExpiryDate;
    private string CVV;

    public CreditCardPayment(string cardNumber, string expiryDate, string cvv)
    {
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;
        CVV = cvv;
    }

    public bool ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing credit card payment of: {amount}€");
        return true; // Simulation
    }
}

public class PayPalPayment : IPaymentStrategy
{
    private string Email;
    private string Password;

    public PayPalPayment(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public bool ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing PayPal payment of: {amount}€");
        return true;
    }
}
