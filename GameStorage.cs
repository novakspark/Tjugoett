using System.Text.Json;

namespace Tjugoett;

public record PlayerData(string Name, int Wins, int Losses, int Pushes);

public class GameStorage
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    private readonly string _filePath;

    public GameStorage(string filePath)
    {
        _filePath = filePath;
    }

    // Saknas filen är det inget fel (första starten). Är filen trasig returneras false.
    public bool TryLoad(out List<Player> players)
    {
        players = new List<Player>();

        if (!File.Exists(_filePath))
        {
            return true;
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            List<PlayerData>? data = JsonSerializer.Deserialize<List<PlayerData>>(json);

            if (data == null)
            {
                return true;
            }

            foreach (PlayerData item in data)
            {
                players.Add(new Player(item.Name, item.Wins, item.Losses, item.Pushes));
            }
            return true;
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException or ArgumentException)
        {
            players = new List<Player>();
            return false;
        }
    }

    public bool TrySave(IEnumerable<Player> players)
    {
        try
        {
            List<PlayerData> data = players
                .Select(p => new PlayerData(p.Name, p.Wins, p.Losses, p.Pushes))
                .ToList();

            File.WriteAllText(_filePath, JsonSerializer.Serialize(data, Options));
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}