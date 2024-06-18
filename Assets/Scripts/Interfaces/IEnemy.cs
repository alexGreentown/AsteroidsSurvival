using UnityEngine;

namespace AsteroidsSurvival.Interfaces
{
    public interface IEnemy
    {
        float Radius { get; }

        Transform Transform { get; }

        void UpdateEnemy();
    }
}