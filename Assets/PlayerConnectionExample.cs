using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.UI;

public class PlayerConnectionExample : MonoBehaviour
{
    public Button sendBtn;
    public Text logText;

    public static readonly Guid kMsgSendEditorToPlayer = new Guid("34d9b47f923142ff847c0d1f8b0554d9");
    public static readonly Guid kMsgSendPlayerToEditor = new Guid("12871ffeaf0c489189579946d8e0840f");

    private void OnEnable()
    {
        PlayerConnection.instance.RegisterConnection(OnConnectionEvent);
        PlayerConnection.instance.RegisterDisconnection(OnDisconnectionEvent);
        PlayerConnection.instance.Register(kMsgSendEditorToPlayer, OnMessageEvent);
        sendBtn.onClick.AddListener(OnSendToEditor);
        Application.logMessageReceived += ApplicationOnLogMessageReceived;
    }

    private void OnDisable()
    {
        PlayerConnection.instance.Unregister(kMsgSendEditorToPlayer, OnMessageEvent);
    }

    private void OnConnectionEvent(int playerId)
    {
        OnLog("Connection " + playerId);
    }

    private void OnDisconnectionEvent(int playerId)
    {
        OnLog("Disconnection " + playerId);
    }

    private void OnMessageEvent(MessageEventArgs args)
    {
        var text = Encoding.ASCII.GetString(args.data);
        OnLog("Message from editor: " + text);
    }

    private void OnSendToEditor()
    {
        PlayerConnection.instance.Send(kMsgSendPlayerToEditor, Encoding.ASCII.GetBytes("Hello from Player"));
    }

    private void OnLog(string log)
    {
        logText.text += DateTime.Now + "\t" + log + "\n";
    }

    private void ApplicationOnLogMessageReceived(string condition, string stacktrace, LogType type)
    {
        OnLog(condition);
        OnLog(stacktrace);
    }
}
