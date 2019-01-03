using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchClient : MonoBehaviour {

    // the client object is defined within the TwitchLib Library if you were wondering what it was...
    public Client client;
    private readonly string channel_name = "gamerr3z";


	private void Start ()
    {
        //we want this script to always be running if our game application is running
        Application.runInBackground = true;

        //set up our bot and tell it which channel to join
        ConnectionCredentials credentials = new ConnectionCredentials("r3zgamebot", Secrets.bot_access_token);
        client = new Client();
        client.Initialize(credentials, channel_name);

        //here we will subscribe to any EVENTS we want our bot to listen for
        //FILL THIS IN LATER 
        client.OnMessageReceived += Client_OnMessageReceived;

        //connect our bout to the channel 
        client.Connect();
	}

    private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        Debug.Log("The bot just read a message in chat");
        Debug.Log(e.ChatMessage.Username + ": " + e.ChatMessage.Message);
    }




    // Update is called once per frame
    void Update () {
	if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            client.SendMessage(client.JoinedChannels[0], "This is a test message from the GamerR3Z twitch rpg bot");

        }
    }


}
