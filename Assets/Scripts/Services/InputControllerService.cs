using UnityEngine;
using UnityEngine.InputSystem;

namespace AsteroidsSurvival.Services
{
    public class InputControllerService : MonoBehaviour, IService
    {
        #region Fields
        [Header("Input Values")] 
        [HideInInspector] public Vector2 MoveValue;
        [HideInInspector] public bool ShootValue;
        [HideInInspector] public bool LaserValue;
        [HideInInspector] public bool StartValue;
        #endregion



        #region Methods

        public Vector2 GetMovement()
        {
            return MoveValue;
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnStart(InputValue value)
        {
            StartInput(value.isPressed);
        }
        
        public void OnLaser(InputValue value)
        {
            LaserInput(value.isPressed);
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            MoveValue = newMoveDirection;
        }

        public void ShootInput(bool newShootState)
        {
            ShootValue = newShootState;
        }
        
        public void LaserInput(bool newLaserState)
        {
            LaserValue = newLaserState;
        }

        public void StartInput(bool newStartState)
        {
            StartValue = newStartState;
        }

        #endregion
    }
}