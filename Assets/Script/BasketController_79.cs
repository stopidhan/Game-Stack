using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 10f;
    private float originalMoveSpeed;
    public float minX = -8f;       // Batas kiri layar
    public float maxX = 8f;        // Batas kanan layar

    
    void Start()
    {
        originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        // Ambil input horizontal (A, D atau panah kiri/kanan)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Hitung posisi baru berdasarkan input
        float newX = transform.position.x + horizontalInput * moveSpeed * Time.deltaTime;

        // Batasi posisi basket agar tidak melewati batas
        newX = Mathf.Clamp(newX, minX, maxX);

        // Perbarui posisi basket
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
        public void ResetSpeed()
    {
        moveSpeed = originalMoveSpeed;
    }
    
}
