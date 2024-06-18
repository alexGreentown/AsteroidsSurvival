using System;
using AsteroidsSurvival.Gameplay.Player;
using AsteroidsSurvival.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AsteroidsSurvival.Gameplay.Enemy
{
    public class UFOController : MonoBehaviour, IMoving, IEnemy
    {
        #region Events
        public event Action<float, Vector2> OnMakeShot;
        #endregion
        
        
        
        #region Fields
        [SerializeField] private float _radius = 5f;
        [SerializeField] private Transform _content;

        private UFOLogic _UFOLogic = new();
        
        private PlayerController _playerController;

        public float Radius => _radius;
        public Transform Transform => transform;

        public PlayerController PlayerController
        {
            get => _playerController;
            set => _playerController = value;
        }

        public Transform Content
        {
            get => _content;
        }

        #endregion
        
        
        
        #region Methods
        
        public void Initialize(PlayerController player)
        {
            _playerController = player;
            
            _UFOLogic.Initialize(this);
        }

        public void MakeShot(float angleToPlayer, Vector2 shipNosePosition)
        {
            if (OnMakeShot == null)
            {
                throw new NotImplementedException("OnMakeShot event is missing");
            }
            OnMakeShot.Invoke(angleToPlayer, shipNosePosition);
        }

        #endregion



        #region IEnemy implementation
        public void UpdateEnemy()
        {
            _UFOLogic.MyUpdate();
        }
        #endregion
        
        
        
        #region IMoving implementation
        public void MoveTo(Vector3 targetPosition)
        {            
            transform.position = targetPosition;
        }
        #endregion

    }
}
