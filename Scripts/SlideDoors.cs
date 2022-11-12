using UnityEngine;

public class SlideDoors : MonoBehaviour
{
    public Animator leftSlideDoor;
    public Animator rightSlideDoor;
    private int bodyCount;

    private void OnTriggerEnter(Collider other)
    {
        bodyCount++;
        if (bodyCount == 1)
        {
            leftSlideDoor.SetBool("slide", true);
            rightSlideDoor.SetBool("slide", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bodyCount--;
        if (bodyCount == 0)
        {
            leftSlideDoor.SetBool("slide", false);
            rightSlideDoor.SetBool("slide", false);
        }
    }
}
