using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{

    private ObjectProperties gunBody;
    public int bulletDamage;

    [Header("Recoil Objects")]
    public Transform camRecoil;
    public Transform physicalCam;

    [Header("VFX Objects")]
    public GameObject bulletDecal;
    public GameObject bloodEffect;
    public VisualEffect nuzzleEffect;

    [Header("Recoil Variables")]
    private Vector3 currentRot;
    private Vector3 targetRot;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    [Header("SFX Objects")]
    public AudioSource gunSource;
    public AudioClip bulletSFX;
    public AudioClip reloadSFX;

    [Header("Ammo Count")]
    public float totalMags;
    public float fullMagAmmo;
    public float currentMagAmmo;

    //private void Start()
    //{
    //    SetGunAmmo();
    //    UpdateAmmoText();
    //}

    private void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRot = Vector3.Lerp(currentRot, targetRot, snappiness * Time.fixedDeltaTime);
        camRecoil.localRotation = Quaternion.Euler(currentRot);
        //cam.GetComponent<TPSMouseLook>().Rotate();

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (totalMags > 1 && currentMagAmmo < fullMagAmmo)
            {
                GetComponent<Animator>().SetTrigger("reload");
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(totalMags >= 1 && currentMagAmmo > 0)
            {
                GetComponent<Animator>().SetBool("isFiring", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("isFiring", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            GetComponent<Animator>().SetBool("isFiring", false);
        }
    }

    private void RecoilFire()
    {
        targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));

        Bullet();
        BulletSFX();
    }

    private void Bullet()
    {
        RaycastHit hit;

        

        if (Physics.Raycast(physicalCam.position, physicalCam.forward, out hit, 50f))
        {

            if (hit.collider.tag == "Character")
            {
                DealDamage(hit.transform);
                GameObject bloodSplat = Instantiate(bloodEffect, hit.point, Quaternion.identity);

                Destroy(bloodSplat, 5f);
            }
            else if (hit.collider.tag == "Breakable")
            {
                Destructible intactObj = hit.collider.GetComponent<Destructible>();
                intactObj.ShatterObject();
            }
            else
            {
                Vector3 point = hit.point;
                point.x += 0.0002f;
                point.y += 0.0002f;
                point.z += 0.0002f;
                GameObject newBulletDecal = Instantiate(bulletDecal, point, Quaternion.FromToRotation(Vector3.back, hit.normal));

                Destroy(newBulletDecal, 30f);
            }
        }
        CountAmmo();
    }

    private void BulletSFX()
    {
        gunSource.PlayOneShot(bulletSFX);
        nuzzleEffect.Play();
    }

    private void ReloadSFX()
    {
        gunSource.PlayOneShot(reloadSFX);

        currentMagAmmo = fullMagAmmo;
        totalMags--;

        UpdateAmmoText();
    }

    private void DealDamage(Transform character)
    {
        INPCTemplate npc = character.GetComponent<INPCTemplate>();
        npc.SetHealth(npc.GetHealth()-20);

        LoadSceneLogic.player.tag = npc.GetTargetName();
    }

    private void CountAmmo()
    {
        currentMagAmmo--;
        if (currentMagAmmo == 0 && totalMags>1)
        {
            GetComponent<Animator>().SetTrigger("reload");
        }

        UpdateGunAmmo();
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        currentMagAmmo = Mathf.Max(0, currentMagAmmo);

        TMP_Text ammoTxt = GetComponentInChildren<TMP_Text>();
        ammoTxt.text = (currentMagAmmo + "/" + (fullMagAmmo * (totalMags - 1)));
    }

    private void UpdateGunAmmo()
    {
        gunBody.SetCurrentMagAmmo(currentMagAmmo);
        gunBody.SetTotalMags(totalMags);
    }

    private void SetGunAmmo()
    {
        this.currentMagAmmo = gunBody.GetCurrentMagAmmo();
        this.totalMags = gunBody.GetTotalMags();
    }

    public void SetGunBody(ObjectProperties gunBody)
    {
        this.gunBody = gunBody;

        SetGunAmmo();
        UpdateAmmoText();
    }
}
