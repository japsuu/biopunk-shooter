using Camera;
using UnityEngine;

namespace Entities.Player
{
    /// <summary>
    /// Rotates the assigned transform to face the mouse position.
    /// </summary>
    public class PlayerRotation : EntityRotation
    {
        protected override Vector2 RotateTowardsPosition => CameraController.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
    }
}