using Entities.Player;
using UnityEngine;

namespace Entities.Enemies
{
    public class ChargeEnemyAim : EntityRotation
    {
        protected override Vector2 RotateTowardsPosition => GetTargetPosition();

        private bool _isChargingTowardsPlayer = false;
        private Vector2 _posBehindPlayer;
        private float _timeSpentInChargingState = 0;


        private Vector3 GetTargetPosition()
        {
            if (_isChargingTowardsPlayer)
                return _posBehindPlayer;
            
            return PlayerController.Instance.transform.position;
        }


        protected override void Update()
        {
            base.Update();
            
            if (_isChargingTowardsPlayer)
                _timeSpentInChargingState += Time.deltaTime;
            
            // If we are facing the player and not charging towards them, start charging. Select a position behind the player, and run towards it.
            bool isFacingPlayer = Vector2.Angle(transform.right, PlayerController.Instance.transform.position - transform.position) < 5;
            if (isFacingPlayer && !_isChargingTowardsPlayer)
            {
                _isChargingTowardsPlayer = true;
                _timeSpentInChargingState = 0;
                _posBehindPlayer = PlayerController.Instance.transform.position - (PlayerController.Instance.transform.position - transform.position).normalized * 15;
            }
            
            // If we are charging towards the player, and are close enough to the position behind the player, stop charging.
            bool closeToChargeTarget = Vector2.Distance(transform.position, _posBehindPlayer) < 1;
            if (_isChargingTowardsPlayer && (closeToChargeTarget || _timeSpentInChargingState > 5))
            {
                _isChargingTowardsPlayer = false;
                _timeSpentInChargingState = 0;
            }
        }
    }
}