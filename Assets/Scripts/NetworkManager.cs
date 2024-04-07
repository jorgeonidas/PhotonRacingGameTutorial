using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [Header("Login UI")]
    public GameObject LoginUIPanel;
    public InputField PlayerNameInput;

    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("GameOptions  Panel")]
    public GameObject GameOptionsUIPanel;


    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public InputField RoomNameInputField;
    public string gameMode;
    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;


    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomUIPanel;


    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(LoginUIPanel.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion



    #region UI Callback Methods
    public void OnLoginButtonClicked()
    {

        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            ActivatePanel(ConnectingInfoUIPanel.name);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }

        }
        else
        {
            Debug.Log("PlayerName is invalid!");
        }


    }

    #endregion



    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} is connected to Photon");
        ActivatePanel(GameOptionsUIPanel.name);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " Is created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Joined Room " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            object gamemodeName;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gm", out gamemodeName))
            {
                Debug.Log($"{gamemodeName.ToString()}");
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }
    #endregion



    #region Public Methods
    public void ActivatePanel(string panelNameToBeActivated)
    {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelNameToBeActivated));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreatingRoomInfoUIPanel.SetActive(CreatingRoomInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreateRoomUIPanel.SetActive(CreateRoomUIPanel.name.Equals(panelNameToBeActivated));
        GameOptionsUIPanel.SetActive(GameOptionsUIPanel.name.Equals(panelNameToBeActivated));
        JoinRandomRoomUIPanel.SetActive(JoinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    }

    public void OnCreateRoomClicked()
    {
        
        if(string.IsNullOrEmpty(gameMode)) {
            Debug.LogError($"Invalid Gamemode");
            return; 
        }
        ActivatePanel(CreatingRoomInfoUIPanel.name);
        string roomName = RoomNameInputField.text;
        if (string.IsNullOrEmpty(roomName)) {
            roomName = $"Room {Random.Range(0, 1000)}";
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;

        string[] roomPropsInLobby = { "gm" };//gm = gamemode

        //two game modes
        //racing = rc
        //death race = dr

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", gameMode } };

        //naming custom room properties for lobby
        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;
        PhotonNetwork.CreateRoom(roomName, roomOptions);

    }

    public void SetGameMode(string _gameMode)
    {
        gameMode  = _gameMode;
    }
    #endregion

}