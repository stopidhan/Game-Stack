using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject lostTile;
    [SerializeField] AudioClip perfectSound;

    private AudioSource audioSource;
    private float distance;
    private float maxDistance = 5.0f;
    private float stepLength;
    private bool moveForward;
    public int moveDirection; // 0: x, 1: z, 2: -x, 3: -z

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        distance = maxDistance;
        moveForward = false;
        InitializePosition();
    }

    private void InitializePosition()
    {
        Vector3 translation = moveDirection switch
        {
            0 => new Vector3(distance, 0, 0),
            1 => new Vector3(0, 0, distance),
            2 => new Vector3(-distance, 0, 0),
            3 => new Vector3(0, 0, -distance),
            _ => Vector3.zero
        };
        transform.Translate(translation);
    }

    private void Update()
    {
        stepLength = Time.deltaTime * 6.0f * GameManager072.GlobalSpeedMultiplier;
        MoveTile();
    }

    private void MoveTile()
    {
        float moveAmount = moveForward ? stepLength : -stepLength; // Menentukan jumlah gerakan berdasarkan arah gerakan

        if (moveDirection == 0 || moveDirection == 2)  // Jika arah gerakan adalah -x (2), balik arah gerakan
        {
            moveAmount *= (moveDirection == 2) ? -1 : 1;
            transform.Translate(moveAmount, 0, 0);
        }
        else
        {
            moveAmount *= (moveDirection == 3) ? -1 : 1; // Jika arah gerakan adalah -z (3), balik arah gerakan
            transform.Translate(0, 0, moveAmount);
        }

        distance += moveForward ? stepLength : -stepLength;

        if (Mathf.Abs(distance) >= maxDistance) // Jika jarak absolut mencapai atau melebihi jarak maksimum, balik arah gerakan
        {
            moveForward = !moveForward;
        }
    }

    public static void UpdateGlobalSpeed(float addSpeed)
    {
        GameManager072.GlobalSpeedMultiplier += addSpeed;
    }

    public void ScaleTile()
    {
        if (Mathf.Abs(distance) > 0.2f)
        {
            CutTile();
        }
        else
        {
            PerfectPlacement();
        }
        Destroy(this);
    }

    private void CutTile()
    {
        float lostLength = Mathf.Abs(distance);
        bool isXAxis = (moveDirection == 0 || moveDirection == 2);

        if ((isXAxis && transform.localScale.x < lostLength) || // Jika panjang ubin yang hilang lebih besar dari skala ubin, permainan berakhir
            (!isXAxis && transform.localScale.z < lostLength))
        {
            Spawner072.instance.GameOver();
            return;
        }
        // Membuat ubin yang hilang
        GameObject _lostTile = CreateLostPiece(lostLength, isXAxis);

        // Mengurangi skala ubin yang tersisa
        Vector3 scaleReduction = isXAxis ?
            new Vector3(lostLength, 0, 0) :
            new Vector3(0, 0, lostLength);
        transform.localScale -= scaleReduction;

        // Memperbarui posisi ubin yang tersisa
        float moveDir = (moveDirection == 0 || moveDirection == 1) ? -1 : 1;
        float translationAmount = lostLength / 2;

        if (isXAxis)
        {
            transform.Translate(moveDir * translationAmount * (distance > 0 ? 1 : -1), 0, 0);
        }
        else
        {
            transform.Translate(0, 0, moveDir * translationAmount * (distance > 0 ? 1 : -1));
        }
    }

    private GameObject CreateLostPiece(float lostLength, bool isXAxis)
    {
        GameObject _lostTile = Instantiate(lostTile);

        // Mengatur skala ubin yang hilang
        _lostTile.transform.localScale = isXAxis ?
            new Vector3(lostLength, transform.localScale.y, transform.localScale.z) :
            new Vector3(transform.localScale.x, transform.localScale.y, lostLength);

        // Menghitung offset posisi untuk menentukan di mana potongan ubin yang hilang harus ditempatkan relatif terhadap posisi ubin asli
        float moveDir = (moveDirection == 0 || moveDirection == 1) ? 1 : -1;
        float translationAmount = (transform.localScale[isXAxis ? 0 : 2]) / 2;

        // Mengatur posisi ubin yang hilang
        Vector3 position = transform.position;
        if (isXAxis)
        {
            position.x += moveDir * translationAmount * (distance > 0 ? 1 : -1);
        }
        else
        {
            position.z += moveDir * translationAmount * (distance > 0 ? 1 : -1);
        }
        _lostTile.transform.position = position;

        // Mengatur warna
        _lostTile.GetComponent<Renderer>().material.SetColor("_Color",
            GetComponent<Renderer>().material.GetColor("_Color"));

        return _lostTile;
    }

    private void PerfectPlacement()
    {
        audioSource.PlayOneShot(perfectSound);
        Vector3 translation = moveDirection switch
        {
            0 => new Vector3(-distance, 0, 0),
            1 => new Vector3(0, 0, -distance),
            2 => new Vector3(distance, 0, 0),
            3 => new Vector3(0, 0, distance),
            _ => Vector3.zero
        };
        transform.Translate(translation);
    }
}