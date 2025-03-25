public class PlayModeEntity : SourceEntity
{
    public static PlayModeEntity Instance { get; private set; }
    public Difficult Difficult;
    public string MapID;

    public MultiplayerMode MultiplayerMode;

    public override SourceEntity Init()
    {
        Instance = this;

        return this;
    }
}

public enum MultiplayerMode { inLobby, createLobby, findLobby, random }