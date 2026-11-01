namespace Tjugoett;

public class Player : Participant
{
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Pushes { get; private set; }

    public Player(string name) : this(name, 0, 0, 0)
    {
    }

    // Används när en spelare läses tillbaka från fil.
    public Player(string name, int wins, int losses, int pushes) : base(name)
    {
        if (wins < 0 || losses < 0 || pushes < 0)
        {
            throw new ArgumentException("Resultat kan inte vara negativa.");
        }

        Wins = wins;
        Losses = losses;
        Pushes = pushes;
    }

    public void Record(Outcome outcome)
    {
        switch (outcome)
        {
            case Outcome.Win:
            case Outcome.Blackjack:
                Wins++;
                break;
            case Outcome.Lose:
                Losses++;
                break;
            case Outcome.Push:
                Pushes++;
                break;
        }
    }
}