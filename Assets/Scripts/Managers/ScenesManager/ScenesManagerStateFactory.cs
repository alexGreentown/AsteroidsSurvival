using System.Collections.Generic;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.Managers.FSM;

namespace AsteroidsSurvival.Managers.ScenesManager
{
    class ScenesManagerStateFactory : ILogic
    {
        
        private Dictionary<ScenesStateType, IFSMState> _myStates = new();

        public IFSMState CreateState(ScenesStateType stateType, IFSM myFSM)
        {
            IFSMState newState;
            if (_myStates.TryGetValue(stateType, out newState))
            {
                return newState;
            }

            switch (stateType)
            {
                case ScenesStateType.MAIN_MENU:
                    newState = new ScenesManagerStateMainMenu();
                    break;
                case ScenesStateType.GAMEPLAY:
                    newState = new ScenesManagerStateGameplay();
                    break;
            }

            newState.MyFSM = myFSM;
            
            _myStates[stateType] = newState;

            return newState;
        }

    }
}