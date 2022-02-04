using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;        // player movement speed
    [SerializeField]
    private float jumpForce;        // player jump force
    [SerializeField]
    private GameObject groundCheck; // ground check gameobject
    [SerializeField]
    private LayerMask groundLayer;  // layer mask to use as ground

    [SerializeField]
    private Transform spawn;

    private float horizontalAxis;
    private bool isGrounded;

    private Rigidbody2D _rb;
    private Transform tr;

    [SerializeField]
    private Text life;

    int lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();      // get the rigidbody component
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // for non physics objects
    }

    // better optimized for physics objects
    void FixedUpdate() {
        // translate
        // ignores physics
        // _rb.transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f));    

        // add force
        // obeys physics and speed increases gradually
        // _rb.AddForce(new Vector2(moveSpeed, 0.0f), ForceMode2D.Force);

        // velocity
        // obeys physics and instant movement
        _rb.velocity = new Vector2(moveSpeed * horizontalAxis, _rb.velocity.y);

        // check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, groundLayer);

        if(tr.position.x >= 97f)
        {
            SceneManager.LoadScene("winScreen");
        }
    }

    // movement input
    public void OnMove(InputAction.CallbackContext context) {
        horizontalAxis = context.ReadValue<float>();
    }

    // jump input
    public void OnJump(InputAction.CallbackContext context) {
        if(context.performed && isGrounded) {
            _rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            lives--;
            if(lives == 0)
            {
                SceneManager.LoadScene("loseScreen");
            }
            tr.position = spawn.position;
            life.text = lives.ToString();
        }
    }
}