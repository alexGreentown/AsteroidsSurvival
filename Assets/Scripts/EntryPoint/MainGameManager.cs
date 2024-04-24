using UnityEngine;
using AsteroidsSurvival.Services;
using AsteroidsSurvival.Managers.ScenesManager;
using AsteroidsSurvival.ServiceLocator;
using AsteroidsSurvival.View;

namespace AsteroidsSurvival.EntryPoint
{
    /// <summary>
    /// The only purpose of MainGameManager is to initialize the game
    /// and to run Update() tick and propagate it to components which require Update() tick
    /// and initialize MyServiceLocator with Services which are attached to this gameObject
    /// </summary>
    public class MainGameManager : MonoBehaviour, IView
    {
        #region Fields and Properties
        private ScenesManagerFSM _scenesManagerFSM;
        
        [SerializeField] private PrefabsDataService _prefabsDataService;
        [SerializeField] private InputControllerService _inputControllerService;
        #endregion

        
        
        #region Unity LifeCycle

        private void Awake()
        {
            InitialiseServiceLocator();
        }

        private void Start()
        {
            InitialiseGame();
        }

        private void Update()
        {
            _scenesManagerFSM.UpdateState();
        }

        private void OnApplicationQuit()
        {
            _scenesManagerFSM.OnApplicationQuit();
        }

        #endregion


        
        #region Methods
        private void InitialiseServiceLocator()
        {
            MyServiceLocator.Register(_inputControllerService);
            MyServiceLocator.Register(_prefabsDataService);
        }
        
        private void InitialiseGame()
        {
            _scenesManagerFSM = new ScenesManagerFSM();
        }
        #endregion

    }
}