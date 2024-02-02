// See https://aka.ms/new-console-template for more information
using CursedMoose.MASR.Commands;
using CursedMoose.MASR.Hotkeys;
using MultiAgentScreenRegionReader.Commands;

HotKeyManager.Initialize();
Console.WriteLine("Hotkey listener activated!");

List<ServerCommand> handlers = new()
{
    new HotkeyCommands(),
    new ScraperCommands(),
    new TestCommand()
};

while (true)
{
    var next = Console.ReadLine();
    try
    {
        if (next != null)
        {
            var handler = handlers.First((command) => command.CanHandle(next));
            handler.Handle(next);
        }
    }
    catch (Exception ex)
    {
        if (ex is InvalidOperationException)
        {
           Console.WriteLine($"Command \"{next}\" not found.");
        }
        else
        {
            Console.WriteLine("Error processing command.");
        }
    }
    finally
    {
        next = null;
    }
}
