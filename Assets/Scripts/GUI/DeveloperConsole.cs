using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
                string addMessage = " command has been added to the console.";

                DeveloperConsole.AddCommandsToConsole(Command, this);
                DeveloperConsole.AddStaticMessageToConsole(Name + addMessage);
            }

            public abstract void RunCommand();
        }

        public static DeveloperConsole Instance { get; private set; }

        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }

        public int TextLogPosition { get; private set; }

        public IList<string> CommandLog = new List<string>();

        public IList<string> ConsoleTextLog = new List<string>();

        public int GetTextLogPosition()
        {
            return TextLogPosition =  ConsoleTextLog.Count - 1;
        }

        


        [Header("UI Components")]
        public Canvas ConsoleCanvas;
        public ScrollRect scrollRect;
        public Text consoleText;
        public Text inputText;
        public InputField consoleInput;
        public Button enterBtn; 


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
            ConsoleCanvas.gameObject.SetActive(true);
            CreateCommands();
            GetTextLogPosition();
        }

        private void CreateCommands()
        {
            var filePath = "F:/Programs I made/Game Development Files/Projects/Unity/Twitch-RPG-Unity/Assets/scripts/Console Commands";
            DirectoryInfo dir = new DirectoryInfo(filePath);
            FileInfo[] info = dir.GetFiles("*.*");

            foreach(FileInfo f in info)
            {
                var fileName = Path.GetFileName(f.ToString());
                fileName = fileName.Remove(fileName.Length - 3);
                fileName = ("Console." + fileName);
                var createCommand = "CreateCommand";
                //var ClassName = "Console.CommandClear";
                Type type = Type.GetType(fileName);

                if (type != null)
                {
                    MethodInfo method = type.GetMethod(createCommand);

                    if (method != null)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
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
            

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ConsoleCanvas.gameObject.SetActive(!ConsoleCanvas.gameObject.activeInHierarchy);
            }

            if(ConsoleCanvas.gameObject.activeInHierarchy)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    ProcessOnSubmit(); 
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) && ConsoleTextLog.Count > 0)
                {
                    if (TextLogPosition > 0)
                    {
                        TextLogPosition = TextLogPosition - 1;
                        consoleInput.text = ConsoleTextLog[TextLogPosition];
                        Debug.Log("Position: " + TextLogPosition);
                        Debug.Log("Down Arrow Pressed");
                    }
                    else
                    {
                        consoleInput.text = "";
                        Debug.Log("Down Arrow Pressed");
                    }
                    
                    //cycle to previous string in ConsoleTextLog
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && ConsoleTextLog.Count > 0)
                {
                    if (TextLogPosition != ConsoleTextLog.Count - 1) { TextLogPosition += 1; }
                    consoleInput.text = ConsoleTextLog[TextLogPosition];
                    Debug.Log("Position: " + TextLogPosition);
                    Debug.Log("Up Arrow Pressed");
                    //cycle to next string in ConsoleTextLog
                }

                
            }
        }

        public void ProcessOnSubmit()
        {
            if (inputText.text != "")
            {
                AddMessageToConsole(inputText.text);
                AddTextToConsoleTextLog(inputText.text);
                ParseInput(inputText.text);
                consoleInput.text = "";
                consoleInput.ActivateInputField();
            }
        }

        //This is the command itself entered into the console. 
        private void AddMessageToConsole(string msg)
        {
            consoleText.text += msg + "\n";
            //scrollRect.verticalNormalizedPosition = 0f; 
        }

        public static void AddCommandToCommandLog(string msg)
        {
            DeveloperConsole.Instance.CommandLog.Add(msg);
        }

        private void AddTextToConsoleTextLog(string txt)
        {
            ConsoleTextLog.Add(txt);
        }

        //This is the output of a command 
        public static void AddStaticMessageToConsole(string msg)
        {
            DeveloperConsole.Instance.consoleText.text += msg + "\n";
            //DeveloperConsole.Instance.scrollRect.verticalNormalizedPosition = 0f;
        }

        private void ParseInput(string input)
        {
            string[] _input = input.Split(null);

            if (_input.Length == 0 || _input == null)
            {
                AddMessageToConsole("Command not recognized.");
                return;
            }

            if (!Commands.ContainsKey(_input[0]))
            {
                AddMessageToConsole("Command not recognized.");
            }

            else
            {
                Commands[_input[0]].RunCommand(); 
            }
        }

        public IList<string> GetCommandLog()
        {
            return CommandLog;
        }

        public IList<string> GetConsoleTextLog()
        {
            return ConsoleTextLog;
        }
    }

    

}


