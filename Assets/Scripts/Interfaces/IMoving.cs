using AsteroidsSurvival.View;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public interface IMoving : IView
    {
        void MoveTo(Vector3 targetPosition);
    }
}
