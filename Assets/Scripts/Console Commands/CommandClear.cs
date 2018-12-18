using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public class CommandClear : DeveloperConsole.ConsoleCommand
    {

        public override string Name { get; protected set; }

        public override string Command { get; protected set; }

        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandClear()
        {
            Name = "Clear";
            Command = "Clear";
            Description = "Clears all the text in the console window";
            Help = "Use this command to clear the console window text";
            AddCommandToConsole();
        }

        public override void RunCommand()
        {
            //Command logic 
            DeveloperConsole.Instance.consoleText.text = "";

            //CommandLog
            DeveloperConsole.AddCommandToCommandLog(Command);
        }

        public static CommandClear CreateCommand()
        {
            return new CommandClear();
        }

        
    }
}
