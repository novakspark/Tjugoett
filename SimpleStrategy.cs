namespace Tjugoett;

// Enkel strategi: köp så länge handen är under 17.
public class SimpleStrategy : IPlayerStrategy
{
    private const int StandValue = 17;

    public Move Decide(Hand hand, Card dealerUpCard)
    {
        return hand.Value < StandValue ? Move.Hit : Move.Stand;
    }
}