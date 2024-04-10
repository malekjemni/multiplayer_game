using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ChatBehavior : NetworkBehaviour
{
    public static ChatBehavior instance;

    [HideInInspector] public TextMeshProUGUI chatText = null;
    [HideInInspector] public TMP_InputField inputField = null;
    [HideInInspector] public Button chatButton = null;

    private static event Action<string> OnMessage;


    public override void OnStartAuthority()
    {
        instance = this;
        OnMessage += HandleNewMessage;

    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!isLocalPlayer) { return; }

        OnMessage -= HandleNewMessage;
        instance = null; 
    }

    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }

   
    [Client]
    public void Send()
    {
         if (!isLocalPlayer) { return; }
        if (string.IsNullOrWhiteSpace(inputField.text)) { return; }
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        RpcHandleMessage($"{PlayerPrefs.GetString("Name")}[{connectionToClient.connectionId}]: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }


    [Client]
    public void ShowChatLog(string message)
    {
        if (!isLocalPlayer) { return; }
        CmdSendChatLog(message);
    }

    [Command]
    private void CmdSendChatLog(string message)
    {
        RpcHandleChatLog($"{PlayerPrefs.GetString("Name")}[{connectionToClient.connectionId}]: recieved {message}");
    }

    [ClientRpc]
    private void RpcHandleChatLog(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }


}
