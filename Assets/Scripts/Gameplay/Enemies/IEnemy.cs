using UnityEngine;

public interface IEnemy
{
    float Radius { get; }

    Transform Transform { get; }

    void UpdateEnemy();
}
