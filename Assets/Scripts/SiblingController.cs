using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiblingController : MonoBehaviour
{
    public GameObject sibling;
    private GameObject family;

    private Material m_material;

    public Season seasonBorn;

    private string characterTag = "Sibling";

    public float baseScale = 0.1f;
    private float currentScale = 0.0f;

    private float priority = 0.0f;

    public float spawnTimer = 0.0f;
    public float spawnTimerIn = 0.4f;

	void Start ()
    {
        spawnTimer = spawnTimerIn;

        family = GameObject.FindGameObjectWithTag("Family");

        m_material = GetComponent<Renderer>().material;

        //initialise tag and scale
        gameObject.tag = characterTag;
        currentScale = baseScale;

        //set the siblings priority by logging the time at this point, the older it is the higher the priority
        priority = Time.time;

        switch (seasonBorn)
        {
            case Season.summer:
                {
                    m_material.color = Color.yellow;
                    break;
                }
            case Season.autumn:
                {
                    m_material.color = Color.red;
                    break;
                }
            case Season.winter:
                {
                    m_material.color = Color.blue;
                    break;
                }
            case Season.spring:
                {
                    m_material.color = Color.green;
                    break;
                }
        }
	}

	void Update ()
    {
        if (currentScale < baseScale)
        {
           // currentScale = baseScale;
        }

        spawnTimer -= Time.deltaTime;
        //update scale of object on start
        this.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    public bool collisionExit = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (spawnTimer < 0.0f)
        {
            if (collision.gameObject.tag == characterTag && collisionExit)
            {
                collisionExit = false;

                SiblingController collisionSiblingController = collision.gameObject.GetComponent<SiblingController>();

                float _priority = priority;

                if (currentScale > collisionSiblingController.currentScale)
                {
                    //NOTE: there might be problems here if both were spawned at the same time and both have the same scale

                    //sets the priority to the "highest" if this sibling is bigger than the other one
                    _priority = -0.1f;
                    Debug.Log("siblings different sizes");
                }

                //checks if the siblings are opposite and if they are then they die
                if ((int)seasonBorn <= 1)
                {
                    if (seasonBorn + 2 == collisionSiblingController.seasonBorn)
                    {
                        Die();
                        return;
                    }
                }
                else if ((int)seasonBorn >= 2)
                {
                    if (seasonBorn - 2 == collisionSiblingController.seasonBorn)
                    {
                        Die();
                        return;
                    }
                }

                if (_priority < collisionSiblingController.priority)
                {
                    if (seasonBorn == collisionSiblingController.seasonBorn)
                    {
                        Debug.Log("absorb " + seasonBorn.ToString() + priority);
                        Absorb(collisionSiblingController.currentScale);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        spawnTimer = spawnTimerIn;
                        Debug.Log("split " + seasonBorn.ToString());
                        Split((currentScale / 2) + (collisionSiblingController.currentScale / 2));

                        // divide the two parents by 2 after creating a new sibling
                        //currentScale /= 2;
                        //collisionSiblingController.currentScale /= 2;
                    }
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionExit = true;
    }

    void Split(float childsBaseScale)
    {
        GameObject child = Instantiate(sibling, this.transform.position, this.transform.rotation, family.transform);
        child.GetComponent<SiblingController>().baseScale = childsBaseScale;
        child.GetComponent<SiblingController>().seasonBorn = GameController.Instance().currentSeason;

        //Debug.Log("split test");
    }

    void Absorb(float collisionScale)
    {
        currentScale += collisionScale;

        //Debug.Log("absorb test");
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
