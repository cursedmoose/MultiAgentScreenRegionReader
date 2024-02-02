﻿namespace CursedMoose.MASR.Commands
{
    // Generally for testing new functionality
    internal class TestCommand : ServerCommand
    {
        public TestCommand() : base("test")
        {
        }

        public override void Handle(string command)
        {
            Log.Info("Using Test Command. Please consider creating a new command instead of relying on this one.");
        }
    }
}