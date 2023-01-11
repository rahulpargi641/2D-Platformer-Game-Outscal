//using System;
//using System.Collections;
//using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ScoreController scoreController;
    [SerializeField] GameOverController gameOverController;
    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] Vector2 playerDeadIfBelowPos;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;

    CapsuleCollider2D playerCapsuleCollider2d;
    Rigidbody2D playerRigidbody2d;
    Animator animator;

    Vector2 boxCollidersize;
    Vector2 boxCollideroffset;

    bool alive = true;
    int hearts = 3;

    private void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        playerCapsuleCollider2d = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        
        boxCollidersize = playerCapsuleCollider2d.size;
        boxCollideroffset = playerCapsuleCollider2d.offset;
    }

    void Update()
    {
        if (!alive) return;

        float speedX = Input.GetAxisRaw("Horizontal");
        float speedY = Input.GetAxisRaw("Vertical");
        PlayerMovement(speedX, speedY);
        PlayerMovementAnimation(speedX, speedY);
        DidPlayerJumpOff();
    }

    private void PlayerMovement(float speedX, float speedY)
    {
        PlayerHorizontalMovement(speedX);
        PlayerVerticalMovement(speedY);
    }

    private void PlayerMovementAnimation(float speedX, float speedY)
    {
        RunAnimation(speedX);
        JumpAnimation(speedY);
        CrouchAnimation();
    }

    private void DidPlayerJumpOff()
    {
        if (transform.position.y < playerDeadIfBelowPos.y)
        {
            alive = false;
        }
    }
    private void PlayerHorizontalMovement(float speedX)
    {
        if (IsGrounded())
        {
            Vector3 position = transform.position;
            position.x = transform.position.x + speedX * moveSpeed * Time.deltaTime;
            transform.position = position;
        }
        else  // in the air slow movement in x direction
        {
            Vector3 position = transform.position;
            position.x = transform.position.x + speedX * moveSpeed / 3 * Time.deltaTime;
            transform.position = position;
        }
    }
    private void PlayerVerticalMovement(float speedY)
    {
        if(speedY > 0 && IsGrounded())
        {
            playerRigidbody2d.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(playerCapsuleCollider2d.bounds.center, playerCapsuleCollider2d.bounds.size, 0f, Vector2.down, 0.6f, platformLayerMask);
        Debug.Log(raycastHit2d.collider);   
        return raycastHit2d.collider != null; 
    }

    private void RunAnimation(float speedX)
    {
        animator.SetFloat("Speed", Mathf.Abs(speedX));
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

    private void JumpAnimation(float speedY)
    {
        if(speedY > 0 && IsGrounded())
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }

    private void CrouchAnimation()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool("Crouch", true);
            playerCapsuleCollider2d.size = new Vector2(boxCollidersize.x, boxCollidersize.y / 2);
            playerCapsuleCollider2d.offset = new Vector2(boxCollideroffset.x, boxCollideroffset.y - boxCollideroffset.y / 2);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerCapsuleCollider2d.size = boxCollidersize;
            playerCapsuleCollider2d.offset = boxCollideroffset;
            animator.SetBool("Crouch", false);
        }
    }

    internal void PickupKey()
    {
        Debug.Log("Key Picked up");
        scoreController.IncreaseScore(10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyController>()!=null)
        {
            DealDamage();
        }
    }
     void DealDamage()
    {
        if (!alive) return;

        hearts -= 1;
        Debug.Log("hearts: " + hearts);

        HurtAnimation();

        if (hearts < 1)
        {
            PlayerDead();
        }
    }
    void HurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }
    private void PlayerDead()
    {
        alive = false;
        DeadAnimation();
        gameOverController.ActivateGameOverPanel();
        this.enabled = false;
    }
    private void DeadAnimation()
    {
        animator.SetTrigger("Dead");
    }
}

