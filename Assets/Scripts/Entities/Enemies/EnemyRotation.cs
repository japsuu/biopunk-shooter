using Entities.Player;
using UnityEngine;

namespace Entities.Enemies
{
    public class EnemyRotation : EntityRotation
    {
        private Enemy _enemy;
        
        protected override Vector2 RotateTowardsPosition => CalculateTargetPosition();
        
        
        public void Initialize(Enemy enemy)
        {
            _enemy = enemy;
        }


        /// <summary>
        /// Calculates the position we should face, for projectiles to hit when fired.
        /// </summary>
        /// <returns></returns>
        private Vector2 CalculateTargetPosition()
        {
            Vector2 playerPos = PlayerController.Instance.transform.position;
            Vector2 playerVelocity = PlayerController.Instance.PlayerMovement.Rb.velocity;
            float playerMovementSpeed = PlayerController.Instance.PlayerMovement.MovementSpeed;
            float projectileMovementSpeed = _enemy.Weapon.DynamicData.ProjectileSpeed;
            
            // Calculate the position we need to aim at, for the projectile to hit the player.
            Vector2 targetPosition = playerPos + playerVelocity * (Vector2.Distance(transform.position, playerPos) / projectileMovementSpeed);
            return targetPosition;
            
        }
    }
}