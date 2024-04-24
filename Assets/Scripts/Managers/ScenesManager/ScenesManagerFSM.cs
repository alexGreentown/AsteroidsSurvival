using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.Managers.FSM;

namespace AsteroidsSurvival.Managers.ScenesManager
{
    public enum ScenesStateType
    {
        MAIN_MENU,
        GAMEPLAY
    }
    
    /// <summary>
    /// MainScenesManager is an FSM with game scenes(now I use GameObjects as scenes) as states
    /// </summary>
    public class ScenesManagerFSM : IFSM, ILogic
    {
        #region Fields and properties
        private IFSMState currentState;
        
        private readonly ScenesManagerStateFactory _statesFactory = new();
        #endregion



        #region Constructor
        
        public ScenesManagerFSM()
        {
            Initialize();
        }
        
        #endregion
       
        
        
        #region Initialization
        
        private void Initialize()
        {
            // setting currentState as instance of ScenesManagerStateMainMenu
            IFSMState newState = CreateState(ScenesStateType.MAIN_MENU);
            ChangeState(newState);
        }
        
        #endregion
        
        
        
        #region IFSM implementation
        
        public IFSMState CreateState(ScenesStateType stateType)
        {
            IFSMState newSTate = _statesFactory.CreateState(stateType, this);
            return newSTate;
        }
        
        public void ChangeState(IFSMState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        public void UpdateState()
        {
            currentState?.UpdateState();
        }
        
        #endregion



        #region Methods

        // this method is called from MainGameManager
        public void OnApplicationQuit()
        {
            currentState.ExitState();
        }
        
        #endregion
    }
}
