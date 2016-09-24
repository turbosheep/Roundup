using UnityEngine;
using System.Collections;

public class SpriteChanger : MonoBehaviour
{
    public float timer = 3;
    public Sprite[] sprites;

    protected int sprite_index;
    protected float clock;
	// Use this for initialization
	void Start ()
    {
        sprite_index = 0;
        clock = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        clock -= Time.deltaTime;
        if (clock < 0)
        {
            clock = timer;
            GetSprite();
        }
    }

    public int GetIndex()
    {
        return sprite_index;
    }

    void GetSprite()
    {
        if (sprite_index < sprites.Length)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[sprite_index];
            sprite_index++;
        }
        else
        {
            GameObject.Find("Game Manager").GetComponent<CircleController>().LoseLife();
            Destroy(this.gameObject);
        }
    }
}
