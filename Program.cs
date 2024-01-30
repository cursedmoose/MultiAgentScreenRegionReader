// See https://aka.ms/new-console-template for more information
using CursedMoose.MASR.Hotkeys;

HotKeyManager.Initialize();
Console.WriteLine("Hotkey listener activated!");

while (true)
{
    var next = Console.ReadLine();
}
