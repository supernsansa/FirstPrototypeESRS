using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    private bool isShooting;
    public GameObject shotPrefab;
    //TODO: some kind of weapon type variable (probably enum)

    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    { }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            Shoot();
        }
    }

    void OnShoot(InputValue value)
    {
        print("Shooting now");
        isShooting = true;
    }

    private void Shoot()
    {
        isShooting = false;
        Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
    }
}
