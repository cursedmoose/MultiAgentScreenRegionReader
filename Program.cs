// See https://aka.ms/new-console-template for more information
using CursedMoose.MASR.Commands;
using CursedMoose.MASR.ElevenLabs;
using CursedMoose.MASR.Hotkeys;
using CursedMoose.MASR.Logging;
using MultiAgentScreenRegionReader.Commands;

Logger log = new Logger("Server");
HotKeyManager.Initialize();
ElevenLabs.Initialize();

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
           log.Error($"Command \"{next}\" not found.");
        }
        else
        {
            log.Error("Error processing command.");
        }
    }
    finally
    {
        next = null;
    }
}
