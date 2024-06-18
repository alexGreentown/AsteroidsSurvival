using UnityEngine;

namespace AsteroidsSurvival.Interfaces
{
    public interface IMoving : IView
    {
        void MoveTo(Vector3 targetPosition);
    }
}
