using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using Steamworks;
using TMPro;

public class ChatManager : MonoBehaviourPunCallbacks, IChatClientListener
{
    private const string ServerInviteMsg = "ServerMessage1001";
    private const string AcceptInviteMsg = "ServerMessage1003";
    private const string PartyDeletedMsg = "ServerMessage1010";
    private const string PartyMemberLeftMSG = "ServerMessage1015";
    private const string FindMatchMSG = "ServerMessage1200";
    private const string MapDataMsg = "MapTypeMsg:";
    private const string RoomIndexMsg = "RoomIndexMsg:";

    private HashSet<string> serverMessages = new HashSet<string>() { ServerInviteMsg, AcceptInviteMsg, PartyDeletedMsg, PartyMemberLeftMSG };

    private ChatClient chatClient;
    private string username, partyMap;
    private int roomIndex;

    public Transform parentPanel;
    public GameObject friendButtonPrefab;

    private string currentRecipient;
    public GameObject chatUI, inviteButton, leaveButton, inPartyButton, inPartyImage;
    public TMP_Text chatHistory, recipientText;
    public TMP_Text inputFieldText;

    public Transform parentInvitePanel;
    public GameObject invitePrefab;

    public Microtransactions paymentManager;

    public LobbyController findMatchSys;

    public GameObject findMatchButton, notPartyLeaderButton, deletePartyButton;

    void Start()
    {
        if (SteamManager.Initialized)
        {
            username = SteamFriends.GetPersonaName();
            //username = SteamUser.GetSteamID().ToString();

            paymentManager.SetSteamID(username);
            paymentManager.SetSteamUsername(SteamFriends.GetPersonaName());

            chatClient = new ChatClient(this);
            ConnectToPhotonChat();
            PopulateFriendPanel();
        }

        
    }

    private void TogglePartyUI()
    {
        TogglePartyImages();
        ToggleMatchButtons(PartySystem.IsPartyLeader());
        ToggleButtons();
    }

    public override void OnConnectedToMaster()
    {
        TogglePartyUI();
    }


    void Update()
    {
        if (SteamManager.Initialized)
        {
            chatClient.Service();
            ProcessMessage();
        }
    }

    public void PopulateFriendPanel()
    {
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

        for (int i = 0; i < friendCount; i++)
        {
            CSteamID m_Friend = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            string friendName = SteamFriends.GetFriendPersonaName(m_Friend);

            if (friendName.Length > 14)
            {
                friendButtonPrefab.GetComponentInChildren<TMP_Text>().text = friendName.Substring(0,14);
            }
            else
            {
                friendButtonPrefab.GetComponentInChildren<TMP_Text>().text = friendName;
            }

            //friendButtonPrefab.GetComponentInChildren<TMP_Text>().text = friendName;
            int imageID = SteamFriends.GetLargeFriendAvatar(m_Friend);

            friendButtonPrefab.GetComponentInChildren<RawImage>().texture = GetSteamImage(imageID);

            GameObject friendButton = Instantiate(friendButtonPrefab, parentPanel);
            friendButton.GetComponent<Button>().onClick.AddListener(delegate { SetCurrentRecipient(friendName); });
        }
    }

    private Texture2D GetSteamImage(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];
            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));
            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        return texture;
    }

    public void InviteFriendToParty()
    {
        SendDirectMessage(currentRecipient, ServerInviteMsg);
    }

    public void LeaveParty()
    {
        if (PartySystem.GetPartyLeaderID() != username)
        {
            SendDirectMessage(PartySystem.GetPartyLeaderID(), PartyMemberLeftMSG);
        }

        PartySystem.SetInParty(false);
        PartySystem.SetIsPartyLeader(true);
        PartySystem.SetPartyLeaderID(username);

        //inPartyImage.SetActive(false);

        ToggleMatchButtons(true);
        TogglePartyImages();
    }

    public void DeleteParty()
    {
        for (int i = 1; i < PartySystem.GetExpectedUsers().Length; i++)
        {
            SendDirectMessage(PartySystem.GetExpectedUsers()[i], PartyDeletedMsg);
        }
        PartySystem.SetInParty(false);
        //inPartyImage.SetActive(false);
        PartySystem.ClearExpectedUsers();

        TogglePartyImages();
        ToggleMatchButtons(true);
        TogglePartyImages();

    }

    public Tuple<List<string>, List<object>> GetChatHistory(string clientTwo)
    {
        string privateChannelName = chatClient.GetPrivateChannelNameByUser(clientTwo);
        ChatChannel privateChannel;
        List<string> senders = new List<string>();
        List<object> messages = new List<object>();
        if (chatClient.TryGetChannel(privateChannelName, true, out privateChannel))
        {
            messages = privateChannel.Messages;
            senders = privateChannel.Senders;
        }
        return new Tuple<List<string>, List<object>>(senders, messages);
    }

    public void ProcessMessage()
    {
        if (chatUI.activeInHierarchy)
        {
            if (!string.IsNullOrEmpty(inputFieldText.text) && Input.GetKeyDown(KeyCode.Return))
            {
                SendDirectMessage(currentRecipient, inputFieldText.text);
                inputFieldText.text = "";
            }
        }
    }

    public void SendDirectMessage(string recipient, string message)
    {
        chatClient.SendPrivateMessage(recipient, message);
    }

    public void SetCurrentRecipient(string val)
    {
        chatHistory.text = "";
        chatUI.SetActive(true);

        currentRecipient = val;
        recipientText.text = "Recipient: " + currentRecipient;

        List<string> senders = GetChatHistory(currentRecipient).Item1;
        List<object> messages = GetChatHistory(currentRecipient).Item2;


        for (int i = 0; i < senders.Count; i++)
        {
            chatHistory.text += (senders[i] + ": " + messages[i].ToString() + "\n");
        }

        ToggleButtons();
    }

    private void ConnectToPhotonChat()
    {
        chatClient.AuthValues = new AuthenticationValues(username);
        ChatAppSettings chatSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        chatClient.ConnectUsingSettings(chatSettings);

        PartySystem.SetUserID(username);
        PartySystem.AddExpectedUsers(username);
    }

    private void HandleInviteMessage(string sender)
    {
        invitePrefab.GetComponentInChildren<TMP_Text>().text = sender;
        GameObject inviteMsg = Instantiate(invitePrefab, parentInvitePanel);
        Button[] options = inviteMsg.GetComponentsInChildren<Button>();
        options[0].onClick.AddListener(delegate { AcceptInvite(inviteMsg, sender); });
        options[1].onClick.AddListener(delegate { DeclineInvite(inviteMsg); });
    }

    private void AcceptInvite(GameObject obj, string sender)
    {
        if (!PartySystem.IsInParty())
        {
            PartySystem.SetIsPartyLeader(false);
            PartySystem.SetInParty(true);
            PartySystem.SetPartyLeaderID(sender);
            //inPartyImage.SetActive(true);

            SendDirectMessage(sender, AcceptInviteMsg);

            ToggleMatchButtons(false);
            ToggleButtons();
            TogglePartyImages();
        }

        Destroy(obj);
    }

    private void DeclineInvite(GameObject obj)
    {
        Destroy(obj);
    }

    public void ChangeMapData(string map)
    {
        partyMap = map;
    }

    public void ChangeRoomIndex(int index)
    {
        roomIndex = index;
    }

    private void ToggleButtons()
    {
        if (PartySystem.IsInParty())
        {
            inviteButton.SetActive(false);

            if (currentRecipient == PartySystem.GetPartyLeaderID())
            {
                leaveButton.SetActive(true);
                inPartyButton.SetActive(false);
            }
            else
            {
                inPartyButton.SetActive(true);
                leaveButton.SetActive(false);
            }
        }
        else
        {
            inviteButton.SetActive(true);
            leaveButton.SetActive(false);
            inPartyButton.SetActive(false);
        }
    }

    private void ToggleMatchButtons(bool isPartyLeader)
    {
        notPartyLeaderButton.SetActive(!isPartyLeader);
        findMatchButton.SetActive(isPartyLeader);
    }

    private void TogglePartyImages()
    {
        if (PartySystem.IsInParty())
        {
            inPartyImage.SetActive(true);
            deletePartyButton.SetActive(PartySystem.IsPartyLeader());
        }
        else
        {
            inPartyImage.SetActive(false);
            deletePartyButton.SetActive(false);
        }
    }

    private void FindMatch()
    {
        string style = "Duo";
        string map = partyMap;
        int roomInd = roomIndex;
        findMatchSys.FindMatchAsPartyMember(map, style, roomInd);
    }
    /// <summary>
    /// All debug output of the library will be reported through this method. Print it or put it in a
    /// buffer to use it on-screen.
    /// </summary>
    /// <param name="level">DebugLevel (severity) of the message.</param>
    /// <param name="message">Debug text. Print to System.Console or screen.</param>
    public void DebugReturn(DebugLevel level, string message)
    {}

    /// <summary>
    /// Disconnection happened.
    /// </summary>
    public void OnDisconnected()
    {}

    /// <summary>
    /// Client is connected now.
    /// </summary>
    /// <remarks>
    /// Clients have to be connected before they can send their state, subscribe to channels and send any messages.
    /// </remarks>
    public void OnConnected()
    {}

    /// <summary>The ChatClient's state changed. Usually, OnConnected and OnDisconnected are the callbacks to react to.</summary>
    /// <param name="state">The new state.</param>
    public void OnChatStateChange(ChatState state)
    {}

    /// <summary>
    /// Notifies app that client got new messages from server
    /// Number of senders is equal to number of messages in 'messages'. Sender with number '0' corresponds to message with
    /// number '0', sender with number '1' corresponds to message with number '1' and so on
    /// </summary>
    /// <param name="channelName">channel from where messages came</param>
    /// <param name="senders">list of users who sent messages</param>
    /// <param name="messages">list of messages it self</param>
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {}

    /// <summary>
    /// Notifies client about private message
    /// </summary>
    /// <param name="sender">user who sent this message</param>
    /// <param name="message">message it self</param>
    /// <param name="channelName">channelName for private messages (messages you sent yourself get added to a channel per target username)</param>
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string[] splitNames = channelName.Split(new char[] { ':' });
        string recipientName = splitNames[1];

        if (username != sender)
        {
            if (message.ToString() == ServerInviteMsg)
            {
                HandleInviteMessage(sender);
                chatHistory.text += (sender + " has sent an invite");
                return;
            }
            if (message.ToString() == AcceptInviteMsg)
            {
                PartySystem.AddExpectedUsers(sender);
                PartySystem.SetInParty(true);
                TogglePartyImages();
                return;
            }
            if (message.ToString() == PartyDeletedMsg)
            {
                LeaveParty();
                return;
            }
            if (message.ToString() == PartyMemberLeftMSG)
            {
                PartySystem.RemoveFromExpectedUsers(sender);
                return;
            }
            if (message.ToString() == FindMatchMSG)
            {
                FindMatch();
                //return;
            }
            if (message.ToString().Length > 13 && message.ToString().Substring(0,12) == RoomIndexMsg)
            {
                ChangeRoomIndex(Int32.Parse(message.ToString().Substring(13)));
                return;
            }
            if (message.ToString().Length > 11 && message.ToString().Substring(0, 10) == MapDataMsg)
            {
                ChangeMapData(message.ToString().Substring(11));
                return;
            }
        }
        if (recipientName == currentRecipient)
        {
            chatHistory.text += (sender + ": " + message.ToString() + "\n");
        }

    }

    /// <summary>
    /// Result of Subscribe operation. Returns subscription result for every requested channel name.
    /// </summary>
    /// <remarks>
    /// If multiple channels sent in Subscribe operation, OnSubscribed may be called several times, each call with part of sent array or with single channel in "channels" parameter.
    /// Calls order and order of channels in "channels" parameter may differ from order of channels in "channels" parameter of Subscribe operation.
    /// </remarks>
    /// <param name="channels">Array of channel names.</param>
    /// <param name="results">Per channel result if subscribed.</param>
    public void OnSubscribed(string[] channels, bool[] results)
    {}

    /// <summary>
    /// Result of Unsubscribe operation. Returns for channel name if the channel is now unsubscribed.
    /// </summary>
    /// If multiple channels sent in Unsubscribe operation, OnUnsubscribed may be called several times, each call with part of sent array or with single channel in "channels" parameter.
    /// Calls order and order of channels in "channels" parameter may differ from order of channels in "channels" parameter of Unsubscribe operation.
    /// <param name="channels">Array of channel names that are no longer subscribed.</param>
    public void OnUnsubscribed(string[] channels)
    {}

    /// <summary>
    /// New status of another user (you get updates for users set in your friends list).
    /// </summary>
    /// <param name="user">Name of the user.</param>
    /// <param name="status">New status of that user.</param>
    /// <param name="gotMessage">True if the status contains a message you should cache locally. False: This status update does not include a message (keep any you have).</param>
    /// <param name="message">Message that user set.</param>
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {}

    /// <summary>
    /// A user has subscribed to a public chat channel
    /// </summary>
    /// <param name="channel">Name of the chat channel</param>
    /// <param name="user">UserId of the user who subscribed</param>
    public void OnUserSubscribed(string channel, string user)
    {}

    /// <summary>
    /// A user has unsubscribed from a public chat channel
    /// </summary>
    /// <param name="channel">Name of the chat channel</param>
    /// <param name="user">UserId of the user who unsubscribed</param>
    public void OnUserUnsubscribed(string channel, string user)
    {}
}
