using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam_Controller : MonoBehaviour
{
    private Vector2 Target = new Vector2(0.0f, 0.0f);
    private GameObject Event_Manager;

    private void Awake()
    {
        Event_Manager = GameObject.FindWithTag("Event");
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target, 0.1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Heart")
        {
            Event_Manager.GetComponent<Event_Management>().Life_Loss();
        }
        else
        {
            if (collision.tag == "Blocker")
            {
                Event_Manager.GetComponent<Event_Management>().Score_Gain();
            }      
        }
        Destroy(gameObject);
    }
}
