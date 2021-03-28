using System;
using System.Text;
using UnityEditor;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.Experimental.Networking.PlayerConnection;
using UnityEngine.Networking.PlayerConnection;

public class EditorConnectionExample : EditorWindow
{
    public static readonly Guid kMsgSendEditorToPlayer = new Guid("34d9b47f923142ff847c0d1f8b0554d9");
    public static readonly Guid kMsgSendPlayerToEditor = new Guid("12871ffeaf0c489189579946d8e0840f");
    private IConnectionState m_AttachToPlayerState;

    [MenuItem("Test/EditorConnectionExample")]
    static void Init()
    {
        EditorConnectionExample window = (EditorConnectionExample)EditorWindow.GetWindow(typeof(EditorConnectionExample));
        window.Show();
        window.titleContent = new GUIContent("EditorConnectionExample");
    }

    private void Awake()
    {
        EditorConnection.instance.Initialize();
        EditorConnection.instance.RegisterConnection(OnConnectionEvent);
        EditorConnection.instance.RegisterDisconnection(OnDisconnectionEvent);
    }

    void OnEnable()
    {
        if (m_AttachToPlayerState == null)
            m_AttachToPlayerState = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUIUtility.GetAttachToPlayerState(this);

        EditorConnection.instance.Register(kMsgSendPlayerToEditor, OnMessageEvent);
    }

    void OnDisable()
    {
        EditorConnection.instance.Unregister(kMsgSendPlayerToEditor, OnMessageEvent);
        //EditorConnection.instance.DisconnectAll();

        m_AttachToPlayerState?.Dispose();
        m_AttachToPlayerState = null;
    }

    private void OnConnectionEvent(int playerId)
    {
        Debug.Log("Connection " + playerId);
    }

    private void OnDisconnectionEvent(int playerId)
    {
        Debug.Log("Disconnection " + playerId);
    }

    private void OnMessageEvent(MessageEventArgs args)
    {
        var text = Encoding.ASCII.GetString(args.data);
        Debug.Log("Message from player: " + text);
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        UnityEditor.Experimental.Networking.PlayerConnection.EditorGUILayout.AttachToPlayerDropdown(m_AttachToPlayerState, EditorStyles.toolbarDropDown);
        GUILayout.EndHorizontal();

        var playerCount = EditorConnection.instance.ConnectedPlayers.Count;
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(string.Format("{0} players connected.", playerCount));
        int i = 0;
        foreach (var p in EditorConnection.instance.ConnectedPlayers)
        {
            builder.AppendLine(string.Format("[{0}] - {1} {2}", i++, p.name, p.playerId));
        }
        EditorGUILayout.HelpBox(builder.ToString(), MessageType.Info);

        if (GUILayout.Button("Send message to player"))
        {
            EditorConnection.instance.Send(kMsgSendEditorToPlayer, Encoding.ASCII.GetBytes("Hello from Editor"));
        }
        if (GUILayout.Button("DisconnectAll"))
        {
            EditorConnection.instance.DisconnectAll();
        }
    }
}