using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Playermovement : MonoBehaviour
{

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private int score; //how many foods are consumed
    private bool food; //did snake eat a food?
    private float time; //timer used for movement
    private List<Transform> tail = new List<Transform>(); //a list with all the tail components in it
    private bool gamestarted; //is the game in progress
    private bool reset; //can the game be reset by a tap
    private int foodcount;

    //public Rigidbody rb;
    public Transform snakehead;
    public GameObject foodPrefab;
    public GameObject tailPrefab;
    public int range; //range of food spawn
    public float speed; //speed of game
    public GameObject start; //start button
    public GameObject tutorial; //tutorial button
    public GameObject title; //title text
    public GameObject gameover; //game over text
    public Text scoretext;
    public GameObject linePrefab;
    public GameObject xfood;
    public GameObject yfood;
    public GameObject zfood;
    public Vector3 movedirection; //the direction the snake will move on the next tick
    //make text elements for the ui so u can change them. also make a tutorial and make all the buttons and shit toggle

    void Start()
    {
        gamestarted = false;
        /*
        for (int xd = -10; xd<10; xd++)
        {
            for (int yd = -10; yd<10; yd++)
            {
                Instantiate(linePrefab, new Vector3(xd, yd, -11), Quaternion.identity);
            }
        }
        */

        gameObject.GetComponent<Renderer>().material.color = Color.green;

        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

        //rb = GetComponent<Rigidbody>();

        snakehead = GetComponent<Transform>();
        snakehead.position = new Vector3(0, 0, 0);
        movedirection = new Vector3(-1, 0, 0);

        start.SetActive(true);
        tutorial.SetActive(true);
        title.SetActive(true);
        gameover.SetActive(false);

        score = 0;
        time = 0f;
        food = false;
        reset = false;
        speed = 0.4f;

        Spawnfood();
    }

    public void Startgame()
    {
        start.SetActive(false);
        tutorial.SetActive(false);
        title.SetActive(false);
        gamestarted = true;
        reset = false;
    }

    private void Spawnfood()
    {
        int x = (int)Random.Range(-range, range);
        int y = (int)Random.Range(-range, range);
        int z = (int)Random.Range(-range, range);

        GameObject foodPrefabclone = Instantiate(foodPrefab, new Vector3(x, y, z), Quaternion.identity);
        foodPrefabclone.GetComponent<Renderer>().material.color = Color.magenta;
        xfood.transform.position = foodPrefabclone.transform.position;
        yfood.transform.position = foodPrefabclone.transform.position;
        zfood.transform.position = foodPrefabclone.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "pickup")
        {
            Destroy(other.gameObject);
            score += 1;
            food = true;
            speed = speed*0.9f;

            Spawnfood();
            
            scoretext.text = "Score: " + score.ToString();
        }
        else if (other.gameObject.tag == "border" || other.gameObject.tag == "tail")
        {
            gameover.SetActive(true);
            gamestarted = false;
            reset = true;
            //hit tail or the border, so u dead ||to do- game over||
        }
        else
        {
            //what
        }
    }

    void Update()
    {
        if (reset)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                reset = false;
                start.SetActive(true);
                tutorial.SetActive(true);
                title.SetActive(true);
                gameover.SetActive(false);

                snakehead = GetComponent<Transform>();
                snakehead.position = new Vector3(0, 0, 0);
                movedirection = new Vector3(-1, 0, 0);
            }
        }
        if (gamestarted)
        {
            time += Time.deltaTime;
            if (time > speed)
            {
                //do movement
                Vector3 headposition = transform.position; //sets the position of the head at beginning of update

                snakehead.transform.position += movedirection; //moves head
                time = 0f; //resets timer

                if (food)
                {
                    foodcount = 3;
                    food = false;
                }
                
                if (foodcount > 0)
                {
                    GameObject newtail = (GameObject)Instantiate(tailPrefab, headposition, Quaternion.identity); //instantiate tail where the head was
                    newtail.GetComponent<Renderer>().material.color = Color.yellow;

                    tail.Insert(0, newtail.transform); //put the new tail piece in the tail list
                    foodcount -= 1;
                }
                else if (tail.Count > 0)
                {
                    tail.Last().position = headposition; //moves last piece of the tail to the space the head occupied before
                    tail.Insert(0, tail.Last()); //puts the last tail piece in the first location of the tail list
                    tail.RemoveAt(tail.Count - 1); //removes the last piece from the end of the tail
                }

            }

            //test
            //test
            //test
            
            if (Input.GetKeyDown("e"))
            {
                if (movedirection != new Vector3(1, 0, 0))
                {
                    //between 0 and 60 degrees
                    //rb.velocity = new Vector3(-1, 0, 0);
                    movedirection = new Vector3(-1, 0, 0);
                }
            }
            if (Input.GetKeyDown("w"))
            {
                if (movedirection != new Vector3(0, -1, 0))
                {
                    //between 60 and 120 degrees
                    //rb.velocity = new Vector3(0, 1, 0);
                    movedirection = new Vector3(0, 1, 0);
                }
            }
            if (Input.GetKeyDown("q"))
            {
                if (movedirection != new Vector3(0, 0, 1))
                {
                    //between 120 and 180 degrees
                    //rb.velocity = new Vector3(0, 0, -1);
                    movedirection = new Vector3(0, 0, -1);
                }
            }


            if (Input.GetKeyDown("a"))
            {
                if (movedirection != new Vector3(-1, 0, 0))
                {
                    //between 180 and 240 degrees
                    //rb.velocity = new Vector3(1, 0, 0);
                    movedirection = new Vector3(1, 0, 0);
                }
            }
            if (Input.GetKeyDown("s"))
            {
                if (movedirection != new Vector3(0, 1, 0))
                {
                    //between 240 and 300 degrees
                    //rb.velocity = new Vector3(0, -1, 0);
                    movedirection = new Vector3(0, -1, 0);
                }
            }
            if (Input.GetKeyDown("d"))
            {
                if (movedirection != new Vector3(0, 0, -1))
                {
                    //between 300 and 360 degrees
                    //rb.velocity = new Vector3(0, 0, 1);
                    movedirection = new Vector3(0, 0, 1);
                }
            }

            if (Input.GetKeyDown("r"))
            {
                start.SetActive(true);
                tutorial.SetActive(true);
                title.SetActive(true);
                gameover.SetActive(false);
                reset = false;
            }
            //test
            //test
            //test

            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;  //last touch position. Ommitted if you use list

                    //Check if drag distance is greater than 15% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (lp.y - fp.y > 0)
                        {
                            //It's up
                            if ((lp.y - fp.y) / (lp.x - fp.x) < Mathf.Sqrt(3) && (lp.y - fp.y) / (lp.x - fp.x) > 0)
                            {
                                if (movedirection != new Vector3(1, 0, 0))
                                {
                                    //between 0 and 60 degrees
                                    //rb.velocity = new Vector3(-1, 0, 0);
                                    movedirection = new Vector3(-1, 0, 0);
                                }
                            }
                            else if ((lp.y - fp.y) / (lp.x - fp.x) > Mathf.Sqrt(3) || (lp.y - fp.y) / (lp.x - fp.x) < -Mathf.Sqrt(3))
                            {
                                if (movedirection != new Vector3(0, -1, 0))
                                {
                                    //between 60 and 120 degrees
                                    //rb.velocity = new Vector3(0, 1, 0);
                                    movedirection = new Vector3(0, 1, 0);
                                }
                            }
                            else if ((lp.y - fp.y) / (lp.x - fp.x) > -Mathf.Sqrt(3) && (lp.y - fp.y) / (lp.x - fp.x) < 0)
                            {
                                if (movedirection != new Vector3(0, 0, 1))
                                {
                                    //between 120 and 180 degrees
                                    //rb.velocity = new Vector3(0, 0, -1);
                                    movedirection = new Vector3(0, 0, -1);
                                }
                            }
                            else
                            {
                                //error
                            }
                        }
                        else
                        {
                            //It's down
                            if ((lp.y - fp.y) / (lp.x - fp.x) < Mathf.Sqrt(3) && (lp.y - fp.y) / (lp.x - fp.x) > 0)
                            {
                                if (movedirection != new Vector3(-1, 0, 0))
                                {
                                    //between 180 and 240 degrees
                                    //rb.velocity = new Vector3(1, 0, 0);
                                    movedirection = new Vector3(1, 0, 0);
                                }
                            }
                            else if ((lp.y - fp.y) / (lp.x - fp.x) > Mathf.Sqrt(3) || (lp.y - fp.y) / (lp.x - fp.x) < -Mathf.Sqrt(3))
                            {
                                if (movedirection != new Vector3(0, 1, 0))
                                {
                                    //between 240 and 300 degrees
                                    //rb.velocity = new Vector3(0, -1, 0);
                                    movedirection = new Vector3(0, -1, 0);
                                }
                            }
                            else if ((lp.y - fp.y) / (lp.x - fp.x) > -Mathf.Sqrt(3) && (lp.y - fp.y) / (lp.x - fp.x) < 0)
                            {
                                if (movedirection != new Vector3(0, 0, -1))
                                {
                                    //between 300 and 360 degrees
                                    //rb.velocity = new Vector3(0, 0, 1);
                                    movedirection = new Vector3(0, 0, 1);
                                }
                            }
                            else
                            {
                                //error
                            }
                        }
                    }
                    else
                    {
                        if(reset)
                        {
                            start.SetActive(true);
                            tutorial.SetActive(true);
                            title.SetActive(true);
                            gameover.SetActive(false);
                            reset = false;
                        }
                        //It's a tap as the drag distance is less than 15% of the screen height
                        //maybe a pause function?
                    }
                }
            }
        }
    }
}