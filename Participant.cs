namespace Tjugoett;

public abstract class Participant
{
    public string Name { get; }
    public Hand Hand { get; } = new();

    protected Participant(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Namnet får inte vara tomt.", nameof(name));
        }

        Name = name.Trim();
    }

    public void Receive(Card card)
    {
        Hand.Add(card);
    }
}