using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{

    public static WindowManager Instance { get; private set; }

    [Header("UI Components")]
    public Canvas MainGuiCanvas;
    public GameObject MainPanelViewPoint;
    public GameObject Console_Window;
    public GameObject Chat_Window;




    public void Display_Console()
    {
        Chat_Window.gameObject.SetActive(false);
        Console_Window.gameObject.SetActive(true);

    }

    public void Display_Chat()
    {
        Console_Window.gameObject.SetActive(false);
        Chat_Window.gameObject.SetActive(true);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}