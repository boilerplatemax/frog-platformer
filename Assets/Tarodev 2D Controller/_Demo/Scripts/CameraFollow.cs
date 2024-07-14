using UnityEngine;

namespace TarodevController.Demo
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private Vector3 _offset = new Vector3(0, 1);
        [SerializeField] private float _lookAheadDistance = 2;
        [SerializeField] private float _lookAheadSpeed = 1;
        [SerializeField] private Vector2 _deadZone = new Vector2(1, 1); // Add dead zone dimensions

        private Vector3 _velOffset;
        private Vector3 _vel;
        private IPlayerController _playerController;
        private Vector3 _lookAheadVel;

        private void Awake() => _player.TryGetComponent(out _playerController);

        private void LateUpdate()
        {
            if (_playerController != null)
            {
                var velocity = _playerController.Velocity;
                Vector3 projectedPos = Vector3.zero;

                // Apply look ahead only if the velocity is outside the dead zone
                if (Mathf.Abs(velocity.x) > _deadZone.x)
                {
                    projectedPos.x = Mathf.Sign(velocity.x) * _lookAheadDistance;
                }
                if (Mathf.Abs(velocity.y) > _deadZone.y)
                {
                    // Show more above when rising and more below when falling
                    projectedPos.y = Mathf.Sign(velocity.y) * _lookAheadDistance;
                }

                _velOffset = Vector3.SmoothDamp(_velOffset, projectedPos, ref _lookAheadVel, _lookAheadSpeed);
            }

            Step(_smoothTime);
        }

        public void AssignNewPlayer(GameObject playerDummy)
        {
            _player = playerDummy.transform;
        }

        private void OnValidate() => Step(0);

        private void Step(float time)
        {
            var goal = _player.position + _offset + _velOffset;
            goal.z = -10;
            transform.position = Vector3.SmoothDamp(transform.position, goal, ref _vel, time);
        }
    }
}
