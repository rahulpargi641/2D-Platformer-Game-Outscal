using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] LayerMask m_PlatformLayerMask;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform parentTransform;
    public Rigidbody2D parentRigidbody2D;
    public bool CanMove = true;
    private void Update()
    {
        if (CanMove)
        {
            ProcessEnemyTranslation();
        }
    }

    public void ProcessEnemyTranslation()
    {
        if (parentTransform.localScale.x > Mathf.Epsilon)
        {
            parentRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
        }
        else if (parentTransform.localScale.x < Mathf.Epsilon)
        {
            parentRigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            ProcessEnemyFlip();
        }
    }

    private void ProcessEnemyFlip()
    {
        parentTransform.localScale = new Vector2(-Mathf.Sign(parentRigidbody2D.velocity.x), Mathf.Sign(parentRigidbody2D.velocity.y));
    }
}
