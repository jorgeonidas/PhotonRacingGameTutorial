using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("LoginUI")]
    public InputField PlayerNameInput;

    #region Unity
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
    #region UI_Methods
    public void OnLoginButtonClicked()
    {
        string playername = PlayerNameInput.text;
        if (!string.IsNullOrEmpty(playername))
        {
            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playername;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.LogError("Player Name is Invalid");
        }
    }
    #endregion

    #region PhotonCallbacks
   //Conexion a internet
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }
    //Conexion establecida con servidores de photon;
    public override void OnConnectedToMaster()
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} Connected to Photon");
    }
    #endregion
}
