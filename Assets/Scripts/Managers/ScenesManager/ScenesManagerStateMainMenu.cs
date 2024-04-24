using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.Services;
using AsteroidsSurvival.Managers.FSM;
using AsteroidsSurvival.ServiceLocator;
using AsteroidsSurvival.Utils;
using AsteroidsSurvival.View;

namespace AsteroidsSurvival.Managers.ScenesManager
{
    /// <summary>
    /// MainMenu State for ScenesManagerFSM
    /// </summary>
    [Serializable]
    public class ScenesManagerStateMainMenu : IFSMState, ILogic
    {
        #region Fields
        private bool isTextVisible = true;

        private MainMenuUIController _mainMenuInstance;
        private InputControllerService _input; 

        private float _blinkTime;

        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken _cancelToken;
        #endregion

        
        
        #region Methods

        public ScenesManagerStateMainMenu()
        {
            _blinkTime = .7f;
        }

        private void InitializeServices()
        {
            InputControllerService inputService = MyServiceLocator.Get<InputControllerService>();
            _input = inputService;
        }

        public void InitializeMainMenu()
        {
            PrefabsDataService prefabsDataService = MyServiceLocator.Get<PrefabsDataService>();
            GameObject prefabMainMenu = prefabsDataService.PrefabsData.MainMenuUIController;
            GameObject newObject = GameObject.Instantiate(prefabMainMenu);
            _mainMenuInstance = newObject.GetComponent<MainMenuUIController>();
            
            BlinkText();
        }
    
        private async void BlinkText()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(_blinkTime));
                
                if (_cancelToken.IsCancellationRequested)
                {
                    return;
                }
                
                isTextVisible = !isTextVisible;
                
                if (_mainMenuInstance == null)
                {
                    Debug.LogError("_mainMenuInstance not available");
                }
                else
                {
                    _mainMenuInstance.SetTextActive(isTextVisible);
                }
            }
        }
        
        #endregion
        
        
        
        #region IFSMState implementation

        public IFSM MyFSM { get; set; }

        public void EnterState()
        {
            _cancelTokenSource = new();
            _cancelToken = _cancelTokenSource.Token;
            
            InitializeServices();
            InitializeMainMenu();
        }

        public void UpdateState()
        {
            if (_input.StartValue)
            {
                // Start Gameplay on press Enter
                _input.StartValue = false;
                
                // start State Gameplay
                ScenesManagerFSM scenesManagerFSM = MyFSM as ScenesManagerFSM;
                var gameplayState = scenesManagerFSM.CreateState(ScenesStateType.GAMEPLAY);
                scenesManagerFSM.ChangeState(gameplayState);
            }
        }

        public void ExitState()
        {
            _cancelTokenSource.Cancel();

            GameObject.Destroy(_mainMenuInstance.gameObject);
            
            string debugString = "ExitState() ScenesManagerStateMainMenu";
            debugString.Log();
        }
        
        #endregion
    }
}
