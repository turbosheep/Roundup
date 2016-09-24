using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircleController : MonoBehaviour
{

    public float boarder = 10;
    public float cooldown = 2;
    public float goal = 10;
    public GameObject circlePrefab;
    public GameObject scoreText;
    public GameObject losePanel;

    protected Rect screenRect;
    protected GameObject circle;
    protected float clock;
    protected bool isCooldown;
    protected int score;
    protected int lives;
    protected Collider[] fish;
    protected bool isAlive;

	// Use this for initialization
	void Start ()
    {
        screenRect = new Rect(boarder, boarder, Screen.width - (2*boarder), Screen.height - (2*boarder));
        clock = 0;
        isCooldown = false;
        score = 0;
        lives = 3;
        isAlive = true;
        losePanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(isAlive)
        {
            clock -= Time.deltaTime;
            if (circle == null && clock < 0)
            {
                if (!isCooldown)
                {
                    clock = cooldown;
                    isCooldown = true;
                }
                else
                {
                    circle = GenCircle();
                    isCooldown = false;
                }
            }

            CheckCircle();
        }
	}

    float RandX()
    {
        return Random.Range(screenRect.xMin, screenRect.xMax);
    }

    float RandY()
    {
        return Random.Range(screenRect.yMin, screenRect.yMax);
    }

    GameObject GenCircle()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(RandX(), RandY()));
        pos.z = 0;
        return (GameObject)Instantiate(circlePrefab, pos, new Quaternion(0, 0, 0, 0));
    }

    void CheckCircle()
    {
        if(circle != null)
        {
            fish = Physics.OverlapSphere(circle.transform.position, circle.GetComponent<SphereCollider>().radius * 2);
            int fish_count = fish.Length -1;
            if (fish_count >= goal)
            {
                int circ_color = circle.GetComponent<SpriteChanger>().GetIndex();
                if (circ_color == 1)
                {
                    score = score + 3;
                }
                else if (circ_color == 2)
                {
                    score = score + 2;
                }
                else
                {
                    score++;
                }
                scoreText.GetComponent<Text>().text = "Score: " + score + "\nLives: " + lives;
                Destroy(circle);
                ScatterFish();
            }
        }
    }

    void ScatterFish()
    {
        foreach(Collider col in fish)
        {
            if(col.gameObject.tag == "Fishy")
            {
                Vector3 randpos = Camera.main.ScreenToWorldPoint(new Vector3(RandX(), RandY()));
                col.gameObject.GetComponent<BoidMouseAI>().enabled = false;
                col.gameObject.GetComponent<GoToPos>().Restart(randpos);
            }
        }
    }

    public void LoseLife()
    {
        lives--;
        scoreText.GetComponent<Text>().text = "Score: " + score + "\nLives: " + lives;
        if(lives <= 0)
        {
            isAlive = false;
            losePanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
