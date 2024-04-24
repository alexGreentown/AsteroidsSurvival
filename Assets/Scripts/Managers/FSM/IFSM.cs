
namespace AsteroidsSurvival.Managers.FSM
{
    public interface IFSM
    {
        void ChangeState(IFSMState newState);

        void UpdateState();
    }
}
