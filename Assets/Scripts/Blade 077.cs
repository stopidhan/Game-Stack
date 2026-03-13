using UnityEngine;

public class Blade : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;
    public float sliceDistanceThreshold = 0.5f; // Jarak maksimum untuk slicing

    private Camera mainCamera;
    private Collider sliceCollider;
    private TrailRenderer sliceTrail;

    public Vector3 direction { get; private set; }
    public bool slicing { get; private set; }

    public GameObject knifeObject; // Referensi ke objek pisau

    private void Awake()
    {
        mainCamera = Camera.main;
        sliceCollider = GetComponent<Collider>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopSlice();
    }

    private void OnDisable()
    {
        StopSlice();
    }

    private void Update()
    {
        // Pastikan knifeObject masih ada sebelum mengaksesnya
        if (knifeObject != null)
        {
            // Menggunakan posisi pisau untuk slicing
            Vector3 knifePosition = knifeObject.transform.position;
            knifePosition.z = 0f;
            transform.position = knifePosition;

            // Debug.Log("Pisau posisi: " + knifePosition); // Debug tambahan

            if (slicing)
            {
                ContinueSlice();
            }

            // Memeriksa apakah ada buah dalam jarak tertentu dari pisau
            CheckForSlice();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("OnTriggerEnter called"); // Debug tambahan
        if (other.CompareTag("Fruit"))
        {
            // Debug.Log("menyentuh");
            StartSlice();
            SliceFruit(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("OnTriggerExit called"); // Debug tambahan
        if (other.CompareTag("Fruit"))
        {
            StopSlice();
        }
    }

    private void StartSlice()
    {
        slicing = true;
        sliceCollider.enabled = true;
        sliceTrail.enabled = true;
        sliceTrail.Clear();
    }

    private void StopSlice()
    {
        slicing = false;
        sliceCollider.enabled = false;
        sliceTrail.enabled = false;
    }

    private void ContinueSlice()
    {
        if (knifeObject != null)
        {
            Vector3 newPosition = knifeObject.transform.position;
            newPosition.z = 0f;
            direction = newPosition - transform.position;

            float velocity = direction.magnitude / Time.deltaTime;
            sliceCollider.enabled = velocity > minSliceVelocity;

            transform.position = newPosition;
        }
    }

    private void SliceFruit(GameObject fruit)
    {
        // Implementasikan logika untuk membelah buah di sini
        // Debug.Log("Buah terbelah: " + fruit.name);
        // Contoh: Menghancurkan objek buah
        // Destroy(fruit);
        // Logika slicing
        Rigidbody fruitRigidbody = fruit.GetComponent<Rigidbody>();
        if (fruitRigidbody != null)
        {
            Vector3 sliceDirection = (fruit.transform.position - transform.position).normalized;
            fruitRigidbody.AddForce(sliceDirection * sliceForce, ForceMode.Impulse);
        }
    }

    private void CheckForSlice()
    {
        // Debug.Log("CheckForSlice called"); // Debug tambahan
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sliceDistanceThreshold);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Fruit"))
            {
                // Debug.Log("menyentuh");
                // Debug.Log("Posisi buah: " + hitCollider.transform.position); // Debug tambahan
                // Debug.Log("Posisi pisau: " + transform.position); // Debug tambahan
                // Debug.Log("Jarak: " + Vector3.Distance(transform.position, hitCollider.transform.position)); // Debug tambahan
                StartSlice();
                SliceFruit(hitCollider.gameObject);
            }
        }
    }

    public void SetKnifeObject(GameObject newKnifeObject)
    {
        knifeObject = newKnifeObject;
    }
}