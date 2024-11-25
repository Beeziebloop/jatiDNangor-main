using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class CharMovement : MonoBehaviour
{
    [SerializeField] private bool isRepeatedMovement = false;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float gridSize = 1f;
    private bool isMoving = false;
    public LogicScript logic;
    public bool playerIsAlive = true;

    private Animator animator;
    private UnityEngine.Vector2 movement;
    private Rigidbody2D rb;
    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("logic").GetComponent<LogicScript>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isMoving && playerIsAlive)
        {
            System.Func<KeyCode, bool> inputFunction;
            if (isRepeatedMovement)
            {
                inputFunction = Input.GetKey;
            }
            else
            {
                inputFunction = Input.GetKeyDown;
            }

            if (inputFunction(KeyCode.UpArrow))
            {
                StartCoroutine(Move(UnityEngine.Vector2.up));
            }
            else if (inputFunction(KeyCode.DownArrow))
            {
                StartCoroutine(Move(UnityEngine.Vector2.down));
            }
            else if (inputFunction(KeyCode.LeftArrow))
            {
                StartCoroutine(Move(UnityEngine.Vector2.left));
            }
            else if (inputFunction(KeyCode.RightArrow))
            {
                StartCoroutine(Move(UnityEngine.Vector2.right));
            }
        }

        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        if(movement != UnityEngine.Vector2.zero)
        {
            animator.SetFloat(lastHorizontal, movement.x);
            animator.SetFloat(lastVertical, movement.y);
        }
    }
 

    private IEnumerator Move(UnityEngine.Vector2 direction)
    {
        isMoving = true;

        UnityEngine.Vector2 startPosition = transform.position;
        UnityEngine.Vector2 endPosition = startPosition + (direction * gridSize);

        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = UnityEngine.Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        transform.position = endPosition;

        isMoving = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
        logic.gameOver();
        playerIsAlive = false;
        Debug.Log("gameover true");
        }
    }
    void OnCollisionEnter(Collision collision)
{
    Debug.Log(collision.collider.name);
}

}
