using UnityEngine;

public class RowInstaller : MonoBehaviour
{

    public bool lockedIn = false;
    public Transform tilesParent;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lockedIn = true;
            //TilesAnimation();
        }
    }

    public bool IsLockedIn()
    {
        return lockedIn;
    }

    public void SetLockedIn(bool value)
    {
        lockedIn = value;
        //TilesAnimation();
    }

    private void TilesAnimation()
    {
        Animator[] tiles = tilesParent.GetComponentsInChildren<Animator>();
        for (int j = 0; j < tiles.Length; j++)
        {
            tiles[j].SetBool("lockedIn", lockedIn);
        }
    }
}
