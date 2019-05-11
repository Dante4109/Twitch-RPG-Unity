using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [Header("UI Components")]
    public Canvas MainGuiCanvas;
    public ScrollRect scrollrectobject;
    public Button ConsoleButton;
    public Button ChatButton;
    public GameObject ConsoleWindow;
    public GameObject ChatWindow;
    private RectTransform[] RectTransforms = new RectTransform[2];

    public void Awake()
    {
        ConsoleWindow.SetActive(false);
        ChatWindow.SetActive(false);
        RectTransforms[0] = ConsoleWindow.GetComponent<RectTransform>();
        RectTransforms[1] = ChatWindow.GetComponent<RectTransform>();
    }

    


    public void Display_Console()
    {
        ChatWindow.gameObject.SetActive(false);
        ConsoleWindow.gameObject.SetActive(true);
        scrollrectobject.content = RectTransforms[0];
    }

    public void Display_Chat()
    {
        ConsoleWindow.gameObject.SetActive(false);
        ChatWindow.gameObject.SetActive(true);
        scrollrectobject.content = RectTransforms[1];
    }
}
