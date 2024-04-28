using UnityEngine;

public interface IEnemy
{
    bool IsDestroyed { get; set; }
    
    bool IsDivided { get; set; }
    
    float Radius { get; }

    Transform Transform { get; }

    void UpdateEnemy();
}
