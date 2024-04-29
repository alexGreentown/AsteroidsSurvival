using AsteroidsSurvival.Services;
using System;
using AsteroidsSurvival.Gameplay.Shot;
using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay.Player
{
    public class PlayerController : PortalMovingBase, IVisited
    {
        #region Events
        public event Action<float, Vector2> OnMakeShot;
        public event Action OnPlayerKilled;
        #endregion
        
        
        
        #region Fields and Properties
        private InputControllerService _inputController;

        private float _rotation = 0f;

        [SerializeField] private Transform _contentTransform;
        [SerializeField] private GameObject _flameObject;
        [SerializeField] private GameObject _laserLines;
        [SerializeField] private LaserController _laserController;

        private float _laserFill = 1f;
        private int _bulletsCount = 10;

        public bool IsLaserActive { get; set; }
        public int EnemiesKilled { get; set; }
        
        public float Radius => 25f;

        public LaserController LaserController => _laserController;
        public GameObject FlameObject => _flameObject;

        private PlayerLogic _playerLogic = new();
        
        public float LaserFill
        {
            get => _laserFill;
            set => _laserFill = value;
        }

        public GameObject LaserLines => _laserLines;

        public Transform ContentTransform => _contentTransform;

        public int BulletsCount
        {
            get => _bulletsCount;
            set => _bulletsCount = value;
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }
        #endregion



        #region Unity Lifecycle
        private void Awake()
        {
            _playerLogic.Initialize(this);
        }

        #endregion
        
        
        
        #region Methods

        public void Initialize()
        {
            EnemiesKilled = 0;
        }
        
        public void UpdatePlayer()
        {
            _playerLogic.MyUpdate();
        }
      
        public void IncreaseBullets()
        {
            _bulletsCount = _bulletsCount + 1;
        }

        public void GetPlayerData(PlayerDataSet playerDataSet)
        {
            playerDataSet.Angle = Rotation % 360f;
            playerDataSet.Coordinates.x = transform.position.x;
            playerDataSet.Coordinates.y = transform.position.y;
            playerDataSet.Speed = _playerLogic.MovementVector.magnitude;
            playerDataSet.BulletsCount = BulletsCount;
            playerDataSet.LaserFill = _laserFill;
            playerDataSet.Score = EnemiesKilled;
        }

        public void PlayerKilled()
        {
            if (OnPlayerKilled == null)
            {
                throw new NotImplementedException("OnPlayerKilled event is missing");
            }
            OnPlayerKilled.Invoke();
        }

        public void MakeShot(float rotation, Vector3 shipNosePosition)
        {
            if (OnMakeShot == null)
            {
                throw new NotImplementedException("OnMakeShot event is missing");
            }
            OnMakeShot(rotation, shipNosePosition);
        }
        #endregion

       

        #region IVisited implementation
        public void AcceptVisit()
        {
            IncreaseBullets();
        }
        #endregion
    }
}
