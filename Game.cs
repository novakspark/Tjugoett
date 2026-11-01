namespace Tjugoett;

public class Game
{
    private readonly ConsoleUi _ui;
    private readonly GameStorage _storage;
    private readonly Deck _deck = new();

    private readonly List<Player> _knownPlayers = new(); // alla som har spelat, sparas till fil
    private readonly List<Player> _table = new();        // de som sitter vid bordet just nu

    public Game(ConsoleUi ui, GameStorage storage)
    {
        _ui = ui;
        _storage = storage;
    }

    public void Run()
    {
        _ui.ShowMessage("Välkommen till Tjugoett!");
        LoadPlayers();

        bool running = true;
        while (running)
        {
            switch (_ui.AskMenuChoice(_table.Count))
            {
                case MenuChoice.AddPlayer:
                    AddPlayer();
                    break;
                case MenuChoice.PlayRound:
                    PlayRound();
                    break;
                case MenuChoice.ShowStatistics:
                    _ui.ShowStatistics(_knownPlayers);
                    break;
                case MenuChoice.Quit:
                    running = false;
                    break;
            }
        }

        SavePlayers();
        _ui.ShowMessage("Tack för att du spelade!");
    }

    private void AddPlayer()
    {
        if (_table.Count >= Round.MaxPlayers)
        {
            _ui.ShowMessage($"Bordet är fullt, högst {Round.MaxPlayers} spelare.");
            return;
        }

        string name = _ui.AskPlayerName(_table.Select(p => p.Name));

        // Finns namnet bland sparade spelare används deras resultat.
        Player? player = _knownPlayers.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        if (player == null)
        {
            player = new Player(name);
            _knownPlayers.Add(player);
        }
        else if (_table.Contains(player))
        {
            _ui.ShowMessage("Den spelaren sitter redan vid bordet.");
            return;
        }

        _table.Add(player);
        _ui.ShowMessage($"{player.Name} satte sig vid bordet.");
    }

    private void PlayRound()
    {
        if (_table.Count == 0)
        {
            _ui.ShowMessage("Lägg till minst en spelare först.");
            return;
        }

        var round = new Round(_table, _deck);
        round.Deal();

        foreach (Player player in _table)
        {
            PlayerTurn(round, player);
        }

        round.PlayDealer();
        IReadOnlyDictionary<Player, Outcome> outcomes = round.Settle();
        _ui.ShowRoundResult(round, outcomes);

        SavePlayers();
    }

    // Spelaren väljer upprepat mellan köpa och stanna. Vid 21 eller mer är turen slut.
    private void PlayerTurn(Round round, Player player)
    {
        while (player.Hand.Value < 21)
        {
            _ui.ShowTable(round, player);

            if (_ui.AskMove(player) == Move.Stand)
            {
                return;
            }

            round.Hit(player);
        }

        _ui.ShowTable(round, player);
    }

    private void LoadPlayers()
    {
        if (!_storage.TryLoad(out List<Player> players))
        {
            _ui.ShowMessage("Sparfilen gick inte att läsa. Du börjar med tom spelarlista.");
        }

        _knownPlayers.AddRange(players);
    }

    private void SavePlayers()
    {
        if (!_storage.TrySave(_knownPlayers))
        {
            _ui.ShowMessage("Varning: resultaten kunde inte sparas.");
        }
    }
}