using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public class CommandPreviousCommands : DeveloperConsole.ConsoleCommand
    {
        private IList<string> commandLog;

        public override string Name { get; protected set; }

        public override string Command { get; protected set; }

        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandPreviousCommands()
        {
            Name = "PreviousCommands";
            Command = "PreviousCommands";
            Description = "Prints PreviousCommands in Console";
            Help = "Use this command to display a log of all previously entered commands"; 

            AddCommandToConsole();
        }

        public override void RunCommand()
        {
            //Command logic 
            DeveloperConsole.AddStaticMessageToConsole("Previously entered commands: ");
            commandLog = DeveloperConsole.Instance.GetCommandLog();
            foreach (var CommandName in commandLog)
            {
                DeveloperConsole.AddStaticMessageToConsole(CommandName);
            }

            //CommandLog
            DeveloperConsole.AddCommandToCommandLog(Command);
        }

        public static CommandPreviousCommands CreateCommand()
        {
            return new CommandPreviousCommands();
        }
    }
}