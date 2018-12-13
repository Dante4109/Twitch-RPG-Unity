using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public class CommandHelloWorld : DeveloperConsole.ConsoleCommand
    {

        public override string Name { get; protected set; }

        public override string Command { get; protected set; }

        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandHelloWorld()
        {
            Name = "HelloWorld";
            Command = "HelloWorld";
            Description = "Prints HelloWorld in Console";
            Help = "Use this command to test the console is working!";

            AddCommandToConsole();
        }

        public override void RunCommand()
        {
            //Command logic 
            DeveloperConsole.AddStaticMessageToConsole("Hello World Test");

        }

        public static CommandHelloWorld CreateCommand()
        {
            return new CommandHelloWorld();
        }
    }
}