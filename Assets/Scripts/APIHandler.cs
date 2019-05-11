using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIHandler : MonoBehaviour {

    private const string tokenname = "x-access-token";
    private const string tokenvalue = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwdWJsaWNfaWQiOiIxYzg5YTVjZC0xMjEwLTQwMTktOTA2MC1jMmFhM2ViNGEyMjciLCJleHAiOjE1NTcxMTM1Nzh9.l0vKLxGKTO-lgKh2_JL6S9TCBPa4OHv70Bue8fUZ1TQ";

    

    private const string APIURL = "http://10.0.0.227:5000/";
    
    public void GetToken()
    {
        StartCoroutine(GetRequest(APIURL + "user"));
    }

    

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        uwr.SetRequestHeader(tokenname, tokenvalue);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
