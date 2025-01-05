using UnityEngine;

namespace Utils
{
    public class PositionIndicator : MonoBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private PlayerManager _playerManager;
     

        private void LateUpdate()
        {
            // Check if player manager or other player is null
            if (_playerManager == null)
            {
                Debug.Log("PlayerManager is not assigned to PositionIndicator");
                return;
            }

            PlayerManager otherPlayer = _playerManager.GetOtherPlayer();

            if (otherPlayer == null)
            {
                Debug.LogWarning("No other players found. Position indicator will not update");
                return;
            }

            // Calculate the position based on the other player's position
            Ray ray = new Ray(_playerManager.GetOtherPlayer().transform.position, transform.parent.position - _playerManager.GetOtherPlayer().transform.position);
            transform.position = ray.GetPoint(distance);
        }
    }
}