using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public abstract class PortalMovingBase : MonoBehaviour, IMoving
    {
        #region IMoving implementation

        /// <summary>
        ///  If iMoving moves to the margin of screen he exits from another side of the screen(portal behaviour)
        /// </summary>
        public virtual void MoveTo(Vector3 targetPosition)
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
            
            transform.position = targetPosition;
        }

        #endregion
    }
}
