using UnityEngine;
using Cinemachine;

public class ParallaxController : MonoBehaviour
{
    Transform cam;
    Vector3 lastCamPos; // Stores last camera position for delta movement calculation
    float distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;
    float farthestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position; // Initialize last camera position

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            float depth = backgrounds[i].transform.position.z - cam.position.z;
            if (depth > farthestBack)
            {
                farthestBack = depth;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void FixedUpdate() // Changed from LateUpdate() to FixedUpdate() for smoother motion
    {
        Vector3 deltaMovement = cam.position - lastCamPos;
        lastCamPos = cam.position;

        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            Vector2 newOffset = mat[i].GetTextureOffset("_MainTex") + new Vector2(deltaMovement.x, 0) * speed;

            // Reset offset within a range to avoid infinite scrolling
            newOffset.x %= 1.0f; // Keeps the offset between 0 and 1
            mat[i].SetTextureOffset("_MainTex", newOffset);
        }
    }
}
