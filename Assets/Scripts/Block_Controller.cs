using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections;
public class Block_Controller : MonoBehaviour
{
    public enum Direction{Up,Down,Left,Right};
    private Direction Current_State = Direction.Right;
    public int commPort = 0;
    public bool controllerActive = false;
    private SerialPort serial = null;
    private bool Connected = false;
    private int Y_Acc;
    private int Z_Acc;
    private int Y_Gyro;
    private bool Is_Down_Neg;

    private void Start()
    {
        ConnectToSerial();
    }

    void ConnectToSerial()
    {
        Debug.Log("Attempting Serial: " + commPort);
        serial = new SerialPort("\\\\.\\COM" + commPort, 9600);
        serial.ReadTimeout = 50;
        serial.Open();
    }

    void WriteToArduino(string message)
    {
        serial.WriteLine(message);
        serial.BaseStream.Flush();
    }

    public string ReadFromArduino(int timeout = 0)
    {
        serial.ReadTimeout = timeout;
        try
        {
            return serial.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerActive)
        {
            WriteToArduino("I");                // Ask for the positions
            String value = ReadFromArduino(50); // read the positions

            if (value != null)                  // check to see if we got what we need
            {
                // EXPECTED VALUE FORMAT: "0-1023"
                string[] values = value.Split(',');     // split the values
                Y_Acc = int.Parse(values[0]);
                Z_Acc = int.Parse(values[1]);
                Y_Gyro = int.Parse(values[2]);
            }
        }
        Change_State();
    }

    private void Change_State()
    {
        if ((Current_State == Direction.Right ^ Current_State == Direction.Left) && Y_Gyro > 8000 || Y_Gyro < -8000)
        {
            if (Y_Gyro < -8000)
            {
                Is_Down_Neg = true;
            }
            else
            {
                Is_Down_Neg = false;
            }
        }
        if (Y_Acc < -6000)
        {
            if (Z_Acc > 8000 && Current_State != Direction.Right)
            {
                To_Right();
            }
            else if (Z_Acc < -8000 && Current_State != Direction.Left)
            {
                To_Left();
            }
        }
        else
        {
            if (Is_Down_Neg)
            {
                if (Z_Acc < -24000)
                {
                    To_Down();
                }
                else if (Z_Acc > -10000)
                {
                    To_Up();
                }
            }
            else
            {
                if (Z_Acc > 24000)
                {
                    To_Down();
                }
                else if (Z_Acc < 10000)
                {
                    To_Up();
                }
            }
        }
    }

    private void To_Down()
    {
        Current_State = Direction.Down;
        transform.position = new Vector2(0, -1f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void To_Right()
    {
        Current_State = Direction.Right;
        transform.position = new Vector2(1f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    private void To_Left()
    {
        Current_State = Direction.Left;
        transform.position = new Vector2(-1f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    private void To_Up()
    {
        Current_State = Direction.Up;
        transform.position = new Vector2(0, 1f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
