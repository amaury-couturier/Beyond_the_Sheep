using UnityEngine;

public class ObjectCollisionChecker : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float moveOutDistance = 0.7f;

    private void Update()
    {
        if (CheckCollision(groundLayer))
        {
            MoveOutOfGround();
            MoveOutOfWall();
        }

        if (CheckCollision(wallLayer))
        {
            MoveOutOfWall();
        }
    }

    private bool CheckCollision(LayerMask layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, layerMask);

        return hit.collider != null;
    }

    private void MoveOutOfGround()
    {
        transform.Translate(Vector2.up * moveOutDistance);
    }

    private void MoveOutOfWall()
    {
        transform.Translate(Vector2.left * moveOutDistance);
    }
}
