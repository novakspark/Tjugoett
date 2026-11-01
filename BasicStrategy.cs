namespace Tjugoett;

// Grundstrategi för köpa/stanna (utan dubbla och dela par).
// Tabellerna slås upp med handens värde (rad) och givarens öppna kort (kolumn).
public class BasicStrategy : IPlayerStrategy
{
    // Kolumnerna är givarens öppna kort: 2, 3, 4, 5, 6, 7, 8, 9, 10, ess.
    // H = köp (Hit), S = stanna (Stand).
    private static readonly Dictionary<int, string> HardTable = new()
    {
        [12] = "HHSSSHHHHH",
        [13] = "SSSSSHHHHH",
        [14] = "SSSSSHHHHH",
        [15] = "SSSSSHHHHH",
        [16] = "SSSSSHHHHH"
    };

    private static readonly Dictionary<int, string> SoftTable = new()
    {
        [12] = "HHHHHHHHHH",
        [13] = "HHHHHHHHHH",
        [14] = "HHHHHHHHHH",
        [15] = "HHHHHHHHHH",
        [16] = "HHHHHHHHHH",
        [17] = "HHHHHHHHHH",
        [18] = "SSSSSSSHHH"
    };

    public Move Decide(Hand hand, Card dealerUpCard)
    {
        int value = hand.Value;
        int column = DealerColumn(dealerUpCard);

        if (hand.IsSoft)
        {
            return value >= 19 ? Move.Stand : Lookup(SoftTable, value, column);
        }

        if (value >= 17)
        {
            return Move.Stand;
        }
        if (value <= 11)
        {
            return Move.Hit;
        }
        return Lookup(HardTable, value, column);
    }

    private static Move Lookup(Dictionary<int, string> table, int value, int column)
    {
        return table[value][column] == 'S' ? Move.Stand : Move.Hit;
    }

    private static int DealerColumn(Card upCard) => upCard.Rank switch
    {
        Rank.Ace => 9,
        Rank.Jack or Rank.Queen or Rank.King => 8,
        _ => (int)upCard.Rank - 2
    };
}