using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RPCControl : NetworkBehaviour {

    public GameObject HeroDumb;
    
    // Use this for initialization
    void Start ()
    {
        if (!isServer)
            CmdSendName("Test");
        
        
    }
        
	

// Update is called once per frame
    private void Update ()
    {
        if (!isServer)
        {
            HeroDumb = GameObject.Find("BaseHeroDumb");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdDestroyHero(HeroDumb);
                Debug.Log("Space was pressed");
            }
        }
    }

    [Command]
    void CmdSendName(string name)
    {
        transform.name = name;
    }

    [ClientRpc]
    void RpcUpdateName(string name)
    {
        transform.name = name;
    }

    [Command]
    void CmdDestroyHero(GameObject hero)
    {
        Destroy(hero);
    }

    [ClientRpc]
    void RpcDestroyHero(GameObject hero)
    {
        Destroy(hero);
    }

}
