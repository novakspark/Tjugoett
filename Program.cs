using Tjugoett;

var ui = new ConsoleUi();
var storage = new GameStorage(Path.Combine(AppContext.BaseDirectory, "players.json"));
var game = new Game(ui, storage);

try
{
    game.Run();
}
catch (EndOfStreamException)
{
    ui.ShowMessage("Inmatningen stängdes. Programmet avslutas.");
}