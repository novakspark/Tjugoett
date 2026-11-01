namespace Tjugoett;

public class ComputerPlayer : Player
{
    private readonly IPlayerStrategy _strategy;

    public ComputerPlayer(string name, IPlayerStrategy strategy) : base(name)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public Move ChooseMove(Card dealerUpCard)
    {
        return _strategy.Decide(Hand, dealerUpCard);
    }
}