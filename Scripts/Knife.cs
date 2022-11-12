using UnityEngine;

public class Knife : MonoBehaviour
{
    public Transform cam;

    public GameObject bloodEffect;
    public GameObject knifeDecal;

    private void Slash()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 2f))
        {
            if (hit.collider.tag == "Character")
            {
                DealDamage(hit.transform);
                GameObject bloodSplat = Instantiate(bloodEffect, hit.point, Quaternion.identity);

                Destroy(bloodSplat, 5f);
            }
            else
            {
                Vector3 point = hit.point;
                point.x += 0.0002f;
                point.y += 0.0002f;
                point.z += 0.0002f;
                GameObject newKnifeDecal = Instantiate(knifeDecal, point, Quaternion.FromToRotation(Vector3.back, hit.normal));

                //Destroy(newBulletDecal, 60f);
            }

        }
    }
    private void DealDamage(Transform character)
    {
        INPCTemplate npc = character.GetComponent<INPCTemplate>();
        npc.SetHealth(npc.GetHealth() - 15);
    }
}
