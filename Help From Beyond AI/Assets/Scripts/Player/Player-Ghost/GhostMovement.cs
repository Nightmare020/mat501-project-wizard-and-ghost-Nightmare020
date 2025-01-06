using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private GhostValues _ghostValues;
    private float _gamePadAddedSpeed;
    private MyInputManager _inputs;

    private Vector2 _screenBounds;
    private BoxCollider2D _ghostCollider;
    private LayerMask _wallCollisionLayer;

    //private Vector2 _upperBound;
    //private Vector2 _lowerBound;
    //private float _objectWidth;
    //private float _objectHeight;


    private float speed = 0;

    private void Start()
    {
        _ghostValues = GetComponent<GhostValues>();
        _inputs = GetComponentInParent<MyInputManager>();
        //_screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height) * 0.5f);

        // get ghost collider and assign the wall collision layer
        _ghostCollider = GetComponent<BoxCollider2D>();
        _wallCollisionLayer = LayerMask.GetMask("Wall");

        //_screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //_objectWidth = _ghostValues.spriteRenderer.bounds.extents.x; //extents = size of width / 2
        //_objectHeight = _ghostValues.spriteRenderer.bounds.extents.y; //extents = size of height / 2
    }

    private void FixedUpdate()
    {
        // Fly
        if (MoveDirectionVectorNormalized() != Vector2.zero)
        {
            Vector2 moveDirection = MoveDirectionVectorNormalized();
            speed = _ghostValues.moveSpeed;
            moveDirection *= _gamePadAddedSpeed;

            // Calculate the potential new position
            Vector2 newPosition = _ghostValues.rigidBody.position + moveDirection * speed * Time.fixedDeltaTime;

            // Check if the new position collides with the walls
            if (!IsCollidingWithWalls(newPosition))
            {
                // Apply movement
                _ghostValues.rigidBody.AddForce(moveDirection * speed - _ghostValues.rigidBody.velocity);;
            }
            else
            {
                // Stop force if colliding with a wall
                _ghostValues.rigidBody.velocity = Vector2.zero;
            }
        }
    }

    private Vector2 MoveDirectionVectorNormalized()
    {
        Vector2 direction = _inputs.GhostMovement();
        //print(direction);
        _gamePadAddedSpeed = direction.magnitude;

        if (direction.x == 0) _ghostValues.aimDirection.x = 0;
        else if (direction.x > 0)
        {
            _ghostValues.spriteRenderer.flipX = false;
            _ghostValues.aimDirection.x = 1;
            _ghostValues.facing = 1;
        }
        else if (direction.x < 0)
        {
            _ghostValues.spriteRenderer.flipX = true;
            _ghostValues.aimDirection.x = -1;
            _ghostValues.facing = -1;
        }

        if (direction.y == 0) _ghostValues.aimDirection.y = 0;
        else _ghostValues.aimDirection.y = direction.y > 0 ? 1 : -1;

        return new Vector2(direction.x, direction.y);
    }

    //void LateUpdate()
    //{
    //    _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    //    Vector3 viewPos = transform.root.position;

    //    _upperBound = new Vector2(_screenBounds.x - _objectWidth, _screenBounds.y - _objectHeight);
    //    _lowerBound = new Vector2(_screenBounds.x - ((_screenBounds.x - Camera.main.transform.position.x) * 2) + _objectWidth, _screenBounds.y - ((_screenBounds.y - Camera.main.transform.position.y) * 2) + _objectHeight);

    //    viewPos.x = Mathf.Clamp(viewPos.x, _screenBounds.x - ((_screenBounds.x - Camera.main.transform.position.x) * 2) + _objectWidth, _screenBounds.x - _objectWidth);
    //    viewPos.y = Mathf.Clamp(viewPos.y, _screenBounds.y - ((_screenBounds.y - Camera.main.transform.position.y) * 2) + _objectHeight, _screenBounds.y - _objectHeight);
    //    transform.root.position = viewPos;
    //}

    private bool IsCollidingWithWalls(Vector2 targetPosition)
    {
        // cast the ghost's collider to check for collisions at the target position
        Collider2D[] hits = Physics2D.OverlapBoxAll(targetPosition, _ghostCollider.size, 0, _wallCollisionLayer);

        // returns true if any of the colliders hit the wall layer
        if (hits.Length > 0 ) return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_ghostCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_ghostValues.rigidBody.position, _ghostCollider.size);
        }

        //Gizmos.DrawSphere(new Vector3(_upperBound.x, _upperBound.y, 0), 1);
        //Gizmos.DrawSphere(new Vector3(_lowerBound.x, _lowerBound.y, 0), 1);
    }

}
