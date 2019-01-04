using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Api.Models.Undocumented.Chatters;

public class TwitchApi : MonoBehaviour {
    public Api api;
    

    private void Start()
    {
        //Set to run in minimized mode
        Application.runInBackground = true;
        api = new Api();
        api.Settings.AccessToken = Secrets.bot_access_token;
        api.Settings.ClientId = Secrets.client_id;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //TO DO Replace gamerr3z with client channel reference 
            api.Invoke(
            api.Undocumented.GetChattersAsync("gamerr3z"), GetChatterListCallback
            );
        }
    }

    private void GetChatterListCallback(List<ChatterFormatted> listofchatters)
    {
        Debug.Log("List of " + listofchatters.Count + " Viewers: ");
        foreach (var chatterObject in listofchatters)
        {
            Debug.Log(chatterObject.Username);
        }
    }
}
