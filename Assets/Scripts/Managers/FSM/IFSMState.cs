namespace AsteroidsSurvival.Managers.FSM
{
    public interface IFSMState
    {
        IFSM MyFSM { get; set; }

        void EnterState();
        
        void UpdateState();
        
        void ExitState();
    }
}

