using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movment_variables
    public float movespeed;
    float x_input;
    float y_input;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Attack_variables
    public float Damage;
    public float attackspeed = 1;
    float attackTimer;
    public float hitboxtiming;
    public float endanimationtiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    public Slider HPSlider;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();

        attackTimer = 0;

        anim = GetComponent<Animator>();

        currHealth = maxHealth;

        HPSlider.value = currHealth / maxHealth;
    }

    private void Update()
    {
        if (isAttacking)
        {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0)
        {
            Attack();
        } else
        {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Interact();
        }
    }
    #endregion

    #region Movment_functions

    private void Move()
    {
        anim.SetBool("Moving", true);
        if (x_input > 0)
        {
            PlayerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
        } 
        else if (x_input < 0)
        {
            PlayerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }
        else if (y_input > 0)
        {
            PlayerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        }
        else if (y_input < 0)
        {
            PlayerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);
    }

    public void increaseSpeed(float value)
    {
        movespeed += value;
        Debug.Log("Speed permanently increased to " + movespeed.ToString());
    }
    #endregion

    #region Attack_functions
    private void Attack()
    {
        Debug.Log("Attacking now");
        Debug.Log(currDirection);
        attackTimer = attackspeed;
        //handles animations and hit boxes
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;

        anim.SetTrigger("Attacking");

        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        yield return new WaitForSeconds(hitboxtiming);
        Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Tons of damage");
                hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
            }
        }
        yield return new WaitForSeconds(hitboxtiming);
        isAttacking = false;

        yield return null;
    }
    #endregion

    #region Health_functions

    public void TakeDamage(float value)
    {
        FindObjectOfType<AudioManager>().Play("PlayerHurt");

        currHealth -= value;
        Debug.Log("Health is now " + currHealth.ToString());

        HPSlider.value = currHealth / maxHealth;
        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float value)
    {
        currHealth += value;
        currHealth = Mathf.Min(currHealth, maxHealth);
        Debug.Log("Health is now " + currHealth.ToString());
        HPSlider.value = currHealth / maxHealth;
    }

    private void Die()
    {
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }
    #endregion

    #region Interact_functions
    private void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0f);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
    }
    #endregion
}
