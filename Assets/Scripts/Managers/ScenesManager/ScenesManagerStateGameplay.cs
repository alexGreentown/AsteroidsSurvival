using System;
using AsteroidsSurvival.Gameplay;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.Managers.FSM;
using AsteroidsSurvival.ServiceLocator;
using AsteroidsSurvival.Services;
using AsteroidsSurvival.Utils;
using UnityEngine;

namespace AsteroidsSurvival.Managers.ScenesManager
{
    /// <summary>
    /// Gameplay State for ScenesManagerFSM
    /// </summary>
    [Serializable]
    public class ScenesManagerStateGameplay : IFSMState, ILogic
    {
        #region Fields
        private MainGameplayController _gameplayControllerInstance;
        
        private InputControllerService _input; 
        
        private PrefabsData _prefabsData;

        private bool _gameOver = false;
        #endregion

        
        
        #region Methods
        
        private void InitializeInputService()
        {
            InputControllerService inputService = MyServiceLocator.Get<InputControllerService>();
            _input = inputService;
        }
        
        private void InitializeGameField()
        {
            PrefabsDataService prefabsDataService = MyServiceLocator.Get<PrefabsDataService>();
            _prefabsData = prefabsDataService.PrefabsData;
            GameObject gameplayPrefab = _prefabsData.GameplayUIController;
            GameObject newObject = UnityEngine.Object.Instantiate(gameplayPrefab);
            _gameplayControllerInstance = newObject.GetComponent<MainGameplayController>();
            
            _gameplayControllerInstance.Initialize();
        }
        
        public void StartGameplay()
        {
            _gameplayControllerInstance.OnGameExit += EndGameplay;
            _gameplayControllerInstance.StartGameplay();
        }

        private void EndGameplay()
        {
            _gameplayControllerInstance.OnGameExit -= EndGameplay;

            _gameOver = true;
            InitializeInputService();
            
            string debugString = "EndGameplay()";
            debugString.Log();
        }
        #endregion


        #region IFSMState implementation

        public IFSM MyFSM { get; set; }
        
        public void EnterState()
        {
            _gameOver = false;
            
            InitializeGameField();
            
            StartGameplay();
        }

        public void UpdateState()
        {
            if (_gameOver)
            {
                // Start Main Menu on press Enter
                if (_input.StartValue)
                {
                    _input.StartValue = false;
                    ScenesManagerFSM scenesManagerFSM = MyFSM as ScenesManagerFSM;
                    var gameplayState = scenesManagerFSM.CreateState(ScenesStateType.MAIN_MENU);
                    scenesManagerFSM.ChangeState(gameplayState);
                }
            }
            else
            {
                _gameplayControllerInstance.UpdateGame();
            }
        }

        public void ExitState()
        {
            string debugString = "ExitState() ScenesManagerStateGameplay";
            debugString.Log();
            
            GameObject.Destroy(_gameplayControllerInstance.gameObject);
        }

        #endregion
    }
}