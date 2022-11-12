using UnityEngine;

public class TileController : MonoBehaviour
{

    public Transform[] walls;
    public RowInstaller rowInstaller;

    public bool isExit;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !rowInstaller.IsLockedIn())
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].GetComponent<Animator>().SetBool("isOpen", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && this.tag == "Exit")
        {
            walls[0].GetComponent<Animator>().SetBool("isOpen", false);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && (!rowInstaller.IsLockedIn()))
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].GetComponent<Animator>().SetBool("isOpen", false);
            }
        }
        
    }*/
}
