using CursedMoose.MASR.Commands;
using CursedMoose.MASR.Hotkeys;

namespace MultiAgentScreenRegionReader.Commands
{
    internal class HotkeyCommands : ServerCommand
    {
        public HotkeyCommands() : base("hotkey")
        {

        }

        public override void Handle(string command)
        {
            var subCommand = StripCommandFromMessage(command);

            switch (subCommand)
            {
                case "select":
                    Log.Info("Capturing new hotkey for selecting screen region...");
                    CaptureNewHotkeyFor(HotkeyCommand.SelectScreenRegion);
                    break;
                case "capture":
                    Log.Info("Capturing new hotkey for capturing screen region...");
                    CaptureNewHotkeyFor(HotkeyCommand.CaptureScreenRegion);
                    break;
            }
        }

        private void CaptureNewHotkeyFor(HotkeyCommand hotkeyCommand)
        {
            var hotkey = HotKeyManager.CaptureNewHotKey();
            HotKeyManager.RegisterHotKey(hotkeyCommand, hotkey);
        }
    }
}
