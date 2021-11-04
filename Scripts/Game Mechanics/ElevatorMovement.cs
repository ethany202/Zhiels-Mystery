using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{

    public float speed = 87;
    public Transform elevator;
    public int currentLevel;
    public int direction = 0;   // Either -1(down) or 1(up) or 0(stationary)
    public bool moving;

    public IEnumerator MoveElevator(int distance, float height)
    {
        

        float direction = speed*height;
        Vector3 movement = new Vector3(0, direction, 0);
        for(int i = 1; i <= distance * 50; i++)
        {
            yield return new WaitForSeconds(0.05f);
            elevator.Translate(movement * 0.09f);    
        }

    }
    public int GetLevel()
    {
        return this.currentLevel;
    }
    public void SetLevel(int newLevel)
    {
        this.currentLevel = newLevel;
    }
    public int GetDirection()
    {
        return this.direction;
    }
    public void SetDirection(int dir)
    {
        direction = dir;
    }
    public bool IsMoving()
    {
        return moving;
    }
    public void SetMoving(bool val)
    {
        moving = val;
    }

}
