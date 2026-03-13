using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knife : MonoBehaviour
{
    public float speed = 10f; // Kecepatan melesatnya pisau
    private bool isFlying = false;
    private Vector3 flyDirection;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position; // Menyimpan posisi awal pisau
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlying)
        {
            // Menghadapkan pisau ke kursor
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0f;

            Vector3 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Melesatkan pisau ketika klik kiri
            if (Input.GetMouseButtonDown(0))
            {
                isFlying = true;
                flyDirection = direction.normalized; // Menyimpan arah terbang
            }
        }

        // Gerakan pisau menuju arah yang ditentukan
        if (isFlying)
        {
            transform.position += flyDirection * speed * Time.deltaTime;

            // Menghapus pisau jika melebihi luas kamera
            if (IsOutOfCameraBounds())
            {
                SpawnNewKnife();
                Destroy(gameObject);
            }
        }
    }

    private bool IsOutOfCameraBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height;
    }

    private void SpawnNewKnife()
    {
        GameObject newKnife = Instantiate(gameObject, initialPosition, Quaternion.identity);
        newKnife.GetComponent<knife>().isFlying = false; // Reset status isFlying pada pisau baru

        // Perbarui referensi knifeObject di Blade
        Blade blade = FindObjectOfType<Blade>();
        if (blade != null)
        {
            blade.SetKnifeObject(newKnife);
        }
    }
}