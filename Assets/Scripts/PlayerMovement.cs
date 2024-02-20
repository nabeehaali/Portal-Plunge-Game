using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private MazeGenerator _mazeGenerator;
    private ScoreManager _scoreManager;
    private Rigidbody _rigidbody;
    private Animator _animator;

    public TextMeshProUGUI enemySpeedUI;

    public List<GameObject> portals;
    public float playerSpeed = 1;

    [SerializeField]
    private int multiplier = 6;
    [SerializeField]
    private GameObject doorParticle;
    [SerializeField]
    private Material OGPortal;
    [SerializeField]
    private Material CoolPortal;
    [SerializeField]
    private GameObject playerCrashParticle;

    bool hitDoor = false;
    public bool hitFragment = false;
    GameObject doorToDestory;
    GameObject fragmentToMove;

    private Vector3 velocity = Vector3.zero;
    int randLocation;

    void Start() 
    {   
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        _mazeGenerator = GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>();
        
        //Make the player start at a random location in the maze
        gameObject.transform.position = new Vector3(Random.Range(0, _mazeGenerator._mazeWidth), 0f, Random.Range(0, _mazeGenerator._mazeDepth));
        gameObject.transform.position *= multiplier;
    }

    void FixedUpdate()
    {
        //Player movement (physics-based)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontalInput, 0, verticalInput).normalized;
        _rigidbody.AddForce(move * playerSpeed, ForceMode.Acceleration);
    }

    private void Update()
    {
        //Breaks the door when the player hits space and is touching a door
        if(hitDoor && Input.GetKey(KeyCode.Space))
        {
            Instantiate(doorParticle, doorToDestory.transform.position, Quaternion.identity);
            Destroy(doorToDestory);
            doorToDestory = null;
            //increase speed of the enemies
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                go.GetComponent<NavMeshAgent>().speed += 3;
                enemySpeedUI.SetText("" + go.GetComponent<NavMeshAgent>().speed);
            }
            hitDoor = false;
        }
        //Moves the fragment towards the portal
        if(hitFragment)
        {
            fragmentToMove.gameObject.transform.position = Vector3.SmoothDamp(fragmentToMove.gameObject.transform.position, GameObject.Find("FinalPortal").transform.position, ref velocity, 0.5f);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Player is able to break the door if they are touching it
        if (collision.gameObject.tag == "Door")
        {
            hitDoor = true;
            doorToDestory = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Player is unable to break the door if they are NOT touching it
        if (collision.gameObject.tag == "Door")
        {
            hitDoor = false;
            doorToDestory = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Adds point for collecting a star
        if(collision.gameObject.tag == "Fragment")
        {
            _scoreManager.AddScore();
            hitFragment = true;
            fragmentToMove = collision.gameObject;
        }
        //Animates player falling into the hole and coming out of a new one
        if(collision.gameObject.tag == "Portal")
        {
            _animator.ResetTrigger("Grow");
            _animator.SetTrigger("Shrink");
            StartCoroutine(BallAnim(collision.gameObject));
        }
        //Break the player, lose a life, and instantiate the player in a new random place
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Water")
        {
            Instantiate(playerCrashParticle, transform.position, Quaternion.identity);
            StartCoroutine(MovePlayer());
            Debug.Log("Player Lost a life!");
            _scoreManager.LoseLife();
        }
        //Player wins the game, do an animation before switching scenes
        if (collision.gameObject.tag == "FinishLine")
        {
            StartCoroutine(EndGame(collision.gameObject));
        }
    }
    
    //Main portal opens up before switching to next scene
    IEnumerator EndGame(GameObject finishline)
    {
        finishline.GetComponent<Animator>().SetTrigger("Grow");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("WinScreen");
    }

    //Move the player to a random portal
    IEnumerator BallAnim(GameObject portal)
    {
        yield return new WaitForSeconds(0.5f);
        portals.Remove(portal);
        randLocation = Random.Range(0, portals.Count);
        StartCoroutine(IgnorePortals(portal, portals[randLocation]));
        
    }
    //Player dies and reinstantiates in new area
    IEnumerator MovePlayer()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(Random.Range(0, _mazeGenerator._mazeWidth), 3.1f, Random.Range(0, _mazeGenerator._mazeDepth));
        transform.position *= multiplier;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }
    //Ignore colission on portal that the player comes out of for a little bit (cooldown)
    IEnumerator IgnorePortals(GameObject portalFrom, GameObject portalTo)
    {
        portalTo.GetComponent<MeshRenderer>().material = CoolPortal;
        //int randLocation = Random.Range(0, portals.Count);
        transform.position = new Vector3(portals[randLocation].transform.position.x, 3.1f, portals[randLocation].transform.position.z);
        Physics.IgnoreCollision(portalTo.GetComponent<Collider>(), GetComponent<Collider>());
        _animator.ResetTrigger("Shrink");
        _animator.SetTrigger("Grow");
        yield return new WaitForSeconds(3);
        portalTo.GetComponent<MeshRenderer>().material = OGPortal;
        Physics.IgnoreCollision(portalTo.GetComponent<Collider>(), GetComponent<Collider>(), false);
        portals.Add(portalFrom);
    }
}
