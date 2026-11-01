namespace Tjugoett;

public interface IPlayerStrategy
{
    Move Decide(Hand hand, Card dealerUpCard);
}