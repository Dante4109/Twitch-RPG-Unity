using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Console
{
    public class DeveloperConsole : MonoBehaviour
    {

        public abstract class ConsoleCommand
        {
            public abstract string Name { get; protected set; }

            public abstract string Command { get; protected set; }

            public abstract string Description { get; protected set; }

            public abstract string Help { get; protected set; }

            public void AddCommandToConsole()
            {

            }

            public abstract void RunCommand();
        }

        public static DeveloperConsole Instance { get; private set; }

        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }

        [Header("UI Components")]
        public Canvas ConsoleCanvas;
        public ScrollRect scrollRect;
        public Text consoleText;
        public Text inputText;
        public InputField consoleInput;

        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
        }

        private void Start()
        {
            ConsoleCanvas.gameObject.SetActive(false);
        }

        private void CreateCommands()
        {

        }

        public static void AddCommandsToConsole(string _name, ConsoleCommand _command)
        {
            if (!Commands.ContainsKey(_name))
            {
                Commands.Add(_name, _command);
            }

        }

        private void Update()
        {
             if(Input.GetKeyDown(KeyCode.BackQuote))
                
        }

    }

}


