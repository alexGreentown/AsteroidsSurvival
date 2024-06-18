using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    /// <summary>
    /// When a moving object gets to the margin of the field
    /// it gets to the opposite side of the field
    /// this is called portal moving
    /// </summary>
    public class PortalMovingLogic : ILogic
    {
        public virtual void CheckPortalMoving(Vector3 targetPosition)
        {
            if (targetPosition.x < 0f)
            {
                targetPosition.x = Screen.width;
            }
            else if (targetPosition.x > Screen.width)
            {
                targetPosition.x = 0f;
            }
            
            if (targetPosition.y < 0f)
            {
                targetPosition.y = Screen.height;
            }
            else if (targetPosition.y > Screen.height)
            {
                targetPosition.y = 0f;
            }
        }
    }
}
