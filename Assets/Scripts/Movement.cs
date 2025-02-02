using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 8f;
    private bool facingRight = true;
    private Rigidbody2D rb;
    private Animator anim;
    public HealthManager healthManager;
    public ArrowsLogic arrowsLogic;
    public Collectable collectable;
    public Vector2 lastCheckpointPos;
    public bool hasCheckpoint = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lastCheckpointPos = transform.position;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();

        if(healthManager.healthBar.fillAmount <= 0)
        {
            anim.SetBool("die", true);
            if(collectable.LifeCount())
                CheckPoint();
            else
                Invoke("LoadMainMenu", 6f);
        }   
        else
            anim.SetBool("die", false);

    }
    void LoadMainMenu()
    {
        if(healthManager.healthBar.fillAmount <= 0)
            SceneManager.LoadScene("MainMenu");
    }
    void CheckPoint()
    {
        anim.SetBool("die", false);
        transform.position = lastCheckpointPos;
        collectable.ChangeLifeCnt(-1);
        healthManager.ResetHealthBar();
    }

    void HandleMovement()
    {
        float xValue = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xValue * speed, rb.velocity.y);

        anim.SetBool("run", xValue != 0);

        if ((xValue < 0 && facingRight) || (xValue > 0 && !facingRight))
        {
            Flip();
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("jump", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("ground")) 
            anim.SetBool("jump", false);    

        if (other.gameObject.CompareTag("cutter")) 
        {
            healthManager.TakeDamage(30f);
        }

        // if (other.gameObject.CompareTag("obstacle")) 
        //     rb.AddForce(Vector3.left * speed);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("clock"))
        {
            other.gameObject.SetActive(false);
            collectable.ChangeClockCnt(1);
        }  
        else if (other.gameObject.CompareTag("key"))
        {
            Debug.Log("Key");
            other.gameObject.SetActive(false);
            collectable.ChangeKeyCnt(1);
        }
        else if (other.gameObject.CompareTag("heart"))
        {
            other.gameObject.SetActive(false);
            collectable.ChangeLifeCnt(1);
            lastCheckpointPos = transform.position; 
            hasCheckpoint = true;
        }
        else if (other.gameObject.CompareTag("arrow_base"))
        {
            arrowsLogic.ArrowAttack();
        }
        else if (other.gameObject.CompareTag("arrows"))
        {
            healthManager.TakeDamage(100f);
        }
        else if (other.gameObject.CompareTag("door"))
        {
            if(collectable.KeyCount())
            {
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                    SceneManager.LoadScene(nextSceneIndex); 
                else
                    SceneManager.LoadScene("MainMenu"); 
            }
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
}