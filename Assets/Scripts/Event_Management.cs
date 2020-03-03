using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event_Management : MonoBehaviour
{
    private float Attack_Speed;
    private int Score;
    private int Life = 5;
    private int New_Beam_Position;
    [SerializeField]
    private GameObject Beam;
    [SerializeField]
    private Text Life_Display;
    [SerializeField]
    private Text Score_Display;
    private bool Game_Over;
    // Start is called before the first frame update
    void Start()
    {
        Life_Display.text = "Life: " + Life.ToString();
        Score_Display.text = "Score: " + Score.ToString();
        Attack_Speed = 1.5f;
        StartCoroutine(Send_Beams());
    }

    IEnumerator Send_Beams()
    {
        Initialise_Beam();
        yield return new WaitForSeconds(Attack_Speed);
        if (Attack_Speed > 0.5f)
        {
            Attack_Speed -= 0.025f;
        }
        if (Game_Over == false)
        {
            StartCoroutine(Send_Beams());
        }
        
    }

    private void Initialise_Beam()
    {
        New_Beam_Position = Random.Range(0, 4);
        switch (New_Beam_Position)
        {
            case 0:
                Instantiate(Beam, new Vector2(0, 13.5f), Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                Instantiate(Beam, new Vector2(0, -13.5f), Quaternion.Euler(0, 0, 180));
                break;
            case 2:
                Instantiate(Beam, new Vector2(13.5f, 0), Quaternion.Euler(0, 0, 270));
                break;
            case 3:
                Instantiate(Beam, new Vector2(-13.5f, 0), Quaternion.Euler(0, 0, 90));
                break;
        }
    }
    
    public void Score_Gain()
    {
        Score += 1;
        Score_Display.text = "Score: " + Score.ToString();
    }

    public void Life_Loss()
    {
        Life -= 1;
        Life_Display.text = "Life: " + Life.ToString();
        if (Life < 1)
        {
            Game_Over = true;
        }
    }
}
