using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Computer : MonoBehaviour{
    
    public GameObject[] targetData;
    public int index=0;

    

    public void UpdateScreen()
    {
        for(int i = 0; i < targetData.Length; i++)
        {
            if (i == index)
            {
                targetData[i].SetActive(true);
            }
            else
            {
                targetData[i].SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("LAKSDJKLASJD");
        //if (other.tag == "Player")// && other.gameObject.GetComponent<PhotonView>().IsMine)
        //{
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("Right");
                index++;
            }
            if (index > targetData.Length)
                index = 0;
            UpdateScreen();
        //}
    }

    public void RefreshTargetData()
    {

    }
}