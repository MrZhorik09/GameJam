using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Jump
}
public class PlayerStateManager : StateManager<PlayerState>
{
    void Awake()
    {
        // Initialisation
        States.Add(PlayerState.Idle, new IdleState(PlayerState.Idle));
        States.Add(PlayerState.Run, new RunState(PlayerState.Run));


        // Initial state
        CurrentState = States[PlayerState.Idle];
    }
}
public class IdleState : BaseState<PlayerState>
{
    public IdleState(PlayerState key) : base(key) { }
    public override void EnterState() { Debug.Log("Idle: Enter"); }
    public override void ExitState() { Debug.Log("Idle: Exit"); }
    public override void UpdateState() { }
    public override PlayerState GetNextState() { return PlayerState.Idle; }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}

public class RunState : BaseState<PlayerState>
{
    public RunState(PlayerState key) : base(key) { }
    public override void EnterState() { Debug.Log("Run: Enter"); }
    public override void ExitState() { Debug.Log("Run: Exit"); }
    public override void UpdateState() { }
    public override PlayerState GetNextState() { return PlayerState.Run; }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}

