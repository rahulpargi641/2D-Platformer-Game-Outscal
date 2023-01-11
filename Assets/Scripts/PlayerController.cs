using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ScoreController scoreController;
    [SerializeField] GameOverController gameOverController;
    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] Vector2 m_PlayerDeadIfBelowPos;
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_JumpSpeed;
 
    CapsuleCollider2D m_PlayerCapsuleCollider2D;
    BoxCollider2D m_PlayerFeetBoxCollider2D;
    Rigidbody2D m_PlayerRigidbody2D;
    Animator m_Animator;

    Vector2 m_CapsuleColliderSize;
    Vector2 m_CapsuleColliderOffset;

    bool m_Alive;
    bool m_PlayerOnTheGround;
    int m_HeartCount;

    private void Awake()
    {
        m_PlayerRigidbody2D = GetComponent<Rigidbody2D>();
        m_PlayerCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_PlayerFeetBoxCollider2D = GetComponentInChildren<BoxCollider2D>();
        m_Animator = GetComponent<Animator>();
        
        m_CapsuleColliderSize = m_PlayerCapsuleCollider2D.size;
        m_CapsuleColliderOffset = m_PlayerCapsuleCollider2D.offset;

        m_Alive = true;
        m_PlayerOnTheGround = true;
        m_HeartCount = 10;
    }

    private void Update()
    {
        if (!m_Alive) return;

        float speedX = Input.GetAxisRaw("Horizontal");
        float speedY = Input.GetAxisRaw("Vertical");
        ProcessPlayerRun(speedX);
        ProcessPlayerRunAnimation(speedX);
        ProcessPlayerJump(speedY);
        ProcessPlayerJumpAnimation(speedY);
        ProcessPlayerCrouch();
        ProcessInCasePlayerJumpsOffFromPlatform();
    }

    private void FixedUpdate()
    {
        m_PlayerOnTheGround = IsPlayerGrounded();
    }

    private void ProcessInCasePlayerJumpsOffFromPlatform()
    {
        if (transform.position.y < m_PlayerDeadIfBelowPos.y)
        {
            m_Alive = false;
        }
    }
    private void ProcessPlayerRun(float speedX)
    {
        if (m_PlayerOnTheGround)
        {
            Vector3 position = transform.position;
            position.x = transform.position.x + speedX * m_MoveSpeed * Time.deltaTime;
            transform.position = position;
        }
        else  // in the air slow movement in x direction
        {
            Vector3 position = transform.position;
            position.x = transform.position.x + speedX * m_MoveSpeed / 2 * Time.deltaTime;
            transform.position = position;
        }
    }

    private void ProcessPlayerJump(float speedY)
    {
        if (speedY > 0 && IsPlayerGrounded())
        {
            m_PlayerRigidbody2D.AddForce(new Vector2(0f, m_JumpSpeed), ForceMode2D.Impulse);
        }
    }

    private bool IsPlayerGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(m_PlayerFeetBoxCollider2D.bounds.center, m_PlayerFeetBoxCollider2D.bounds.size, 0f, Vector2.down, 0.5f, platformLayerMask);
        //Debug.Log(raycastHit2d.collider);   
        return raycastHit2d.collider != null; 
    }

    private void ProcessPlayerRunAnimation(float speedX)
    {
        m_Animator.SetFloat("Speed", Mathf.Abs(speedX));
        Vector3 scale = transform.localScale;
        if (speedX < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (speedX > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    private void ProcessPlayerJumpAnimation(float speedY)
    {
        if(speedY > 0 && IsPlayerGrounded())
        {
            m_Animator.SetBool("Jump", true);
        }
        else
        {
            m_Animator.SetBool("Jump", false);
        }
    }

    private void ProcessPlayerCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            m_Animator.SetBool("Crouch", true);
            m_PlayerCapsuleCollider2D.size = new Vector2(m_CapsuleColliderSize.x, m_CapsuleColliderSize.y / 2);
            m_PlayerCapsuleCollider2D.offset = new Vector2(m_CapsuleColliderOffset.x, m_CapsuleColliderOffset.y - m_CapsuleColliderOffset.y / 2);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            m_PlayerCapsuleCollider2D.size = m_CapsuleColliderSize;
            m_PlayerCapsuleCollider2D.offset = m_CapsuleColliderOffset;
            m_Animator.SetBool("Crouch", false);
        }
    }

    internal void ProcessPlayerPickingupKey()
    {
        Debug.Log("Key Picked up");
        scoreController.IncreaseScore(10);
    }
    public void ProcessDamage()
    {
        if (!m_Alive) return;

        m_HeartCount -= 1;
        Debug.Log("Player had been hurt , hearts: " + m_HeartCount);
        SoundManager.Instance.PlayPlayerRelatedSound(ESounds.PlayerHurt);

        HurtAnimation();

        if (m_HeartCount < 0)
        {
            PlayerDead();
        }
    }
    void HurtAnimation()
    {
        m_Animator.SetTrigger("Hurt");
    }
    private void PlayerDead()
    {
        Debug.Log("Player Dead!");
        m_Alive = false;
        SoundManager.Instance.PlayPlayerRelatedSound(ESounds.PlayerDie);
        DeadAnimation();
        gameOverController.ActivateGameOverPanel();
        this.enabled = false;
    }
    private void DeadAnimation()
    {
        m_Animator.SetTrigger("Dead");
    }

    void HandlePlayerFootsteps() // in the animation
    {
        SoundManager.Instance.PlayPlayerFootstepsSound(ESounds.PlayerRun);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyController>() != null)
        {
            Debug.Log(collision.gameObject.name);
            ProcessDamage();
        }
    }
}

