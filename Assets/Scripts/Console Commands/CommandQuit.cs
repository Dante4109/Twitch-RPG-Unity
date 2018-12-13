using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public class CommandQuit : DeveloperConsole.ConsoleCommand
    {
        
        public override string Name { get; protected set; }

        public override string Command { get; protected set; }

        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandQuit()
        {
            Name = "Quit";
            Command = "Quit";
            Description = "Quits the application";
            Help = "Use this command with no arguments to force unity to quit!";

            AddCommandToConsole();
        }

        public override void RunCommand()
        {
            if (Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }

            else
            {
                Application.Quit();
            }
        }

        public static CommandQuit CreateCommand()
        {
            return new CommandQuit();
        }
    }
}
