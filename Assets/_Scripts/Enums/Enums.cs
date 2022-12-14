public enum GameState{
    Idle,
    GameLose,
    GameStart
}

public enum MovementState{
    freeze,
    unlimited,
    jumping,
    sliding,
    running,
}

public enum PlayerState{
    Grounded,
    OnLedge,
    Vault,
    Stepping,
    Climbing
}
[System.Serializable]
public enum PowerupType{
    Magnet,
    SpeedBoost, 
    None,
}