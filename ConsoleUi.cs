using System.Text;

namespace Tjugoett;

public class ConsoleUi
{
    private const int MaxNameLength = 20;

    public ConsoleUi()
    {
        // Behövs för att färgsymbolerna ♥ ♦ ♣ ♠ och å, ä, ö ska visas rätt.
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public MenuChoice AskMenuChoice(int playersAtTable)
    {
        Console.WriteLine();
        Console.WriteLine($"=== Tjugoett === (spelare vid bordet: {playersAtTable})");
        Console.WriteLine("1. Lägg till spelare");
        Console.WriteLine("2. Spela en runda");
        Console.WriteLine("3. Visa resultat");
        Console.WriteLine("4. Avsluta");

        return (MenuChoice)AskNumber(1, 4);
    }

    public string AskPlayerName(IEnumerable<string> takenNames)
    {
        List<string> taken = takenNames.ToList();

        while (true)
        {
            Console.Write("Spelarens namn: ");
            string name = ReadLine().Trim();

            if (name.Length == 0)
            {
                Console.WriteLine("Namnet får inte vara tomt.");
            }
            else if (name.Length > MaxNameLength)
            {
                Console.WriteLine($"Namnet får vara högst {MaxNameLength} tecken.");
            }
            else if (taken.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Det namnet finns redan vid bordet.");
            }
            else
            {
                return name;
            }
        }
    }

    public Move AskMove(Player player)
    {
        Console.WriteLine($"{player.Name}, din tur:");
        Console.WriteLine("1. Köpa ett kort");
        Console.WriteLine("2. Stanna");

        return AskNumber(1, 2) == 1 ? Move.Hit : Move.Stand;
    }

    public void ShowTable(Round round, Player activePlayer)
    {
        Console.WriteLine();
        Console.WriteLine($"Givaren: {FormatDealer(round.Dealer)}");

        foreach (Player player in round.Players)
        {
            string marker = player == activePlayer ? ">" : " ";
            Console.WriteLine($"{marker} {player.Name}: {FormatHand(player.Hand)}");
        }
    }

    public void ShowRoundResult(Round round, IReadOnlyDictionary<Player, Outcome> outcomes)
    {
        Console.WriteLine();
        Console.WriteLine("--- Resultat ---");
        Console.WriteLine($"Givaren: {FormatHand(round.Dealer.Hand)}");

        foreach (Player player in round.Players)
        {
            Console.WriteLine($"{player.Name}: {FormatHand(player.Hand)} -> {OutcomeText(outcomes[player])}");
        }
    }

    public void ShowStatistics(IReadOnlyList<Player> players)
    {
        Console.WriteLine();
        if (players.Count == 0)
        {
            Console.WriteLine("Inga spelare har spelat än.");
            return;
        }

        Console.WriteLine("--- Resultat genom tiderna ---");
        foreach (Player player in players)
        {
            Console.WriteLine($"{player.Name}: {player.Wins} vinster, {player.Losses} förluster, {player.Pushes} lika");
        }
    }

    private int AskNumber(int min, int max)
    {
        while (true)
        {
            Console.Write("Välj: ");
            string input = ReadLine();

            if (int.TryParse(input, out int number) && number >= min && number <= max)
            {
                return number;
            }

            Console.WriteLine($"Ogiltigt val. Skriv en siffra mellan {min} och {max}.");
        }
    }

    // Stängs inmatningen (t.ex. Ctrl + Z) får Program.cs avsluta snyggt.
    private static string ReadLine()
    {
        return Console.ReadLine() ?? throw new EndOfStreamException("Inmatningen stängdes.");
    }

    private static string FormatCard(Card card)
    {
        return $"{SuitSymbol(card.Suit)}{RankLabel(card.Rank)}";
    }

    private static char SuitSymbol(Suit suit) => suit switch
    {
        Suit.Hearts => '♥',
        Suit.Diamonds => '♦',
        Suit.Clubs => '♣',
        Suit.Spades => '♠',
        _ => '?'
    };

    private static string RankLabel(Rank rank) => rank switch
    {
        Rank.Jack => "Kn",
        Rank.Queen => "D",
        Rank.King => "K",
        Rank.Ace => "A",
        _ => ((int)rank).ToString()
    };

    private static string FormatHand(Hand hand)
    {
        string cards = string.Join(" ", hand.Cards.Select(FormatCard));
        string text = $"{cards} = {hand.Value}";

        if (hand.IsBust)
        {
            return text + " (SPRÄCKT)";
        }
        if (hand.IsBlackjack)
        {
            return text + " (naturlig tjugoett)";
        }
        if (hand.IsSoft)
        {
            return text + " (mjuk)";
        }
        return text;
    }

    // Hålkortet visas inte förrän givaren vänder det.
    private static string FormatDealer(Dealer dealer)
    {
        if (dealer.IsHoleCardRevealed)
        {
            return FormatHand(dealer.Hand);
        }

        Card? upCard = dealer.UpCard;
        return upCard == null ? "(inga kort)" : $"{FormatCard(upCard)} [dolt kort]";
    }

    private static string OutcomeText(Outcome outcome) => outcome switch
    {
        Outcome.Win => "vinst",
        Outcome.Lose => "förlust",
        Outcome.Push => "lika",
        Outcome.Blackjack => "naturlig tjugoett, vinst",
        _ => throw new ArgumentOutOfRangeException(nameof(outcome))
    };
}