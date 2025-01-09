using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    private Camera cam;
    public Rigidbody2D rb;

    void Start()
    {

        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 mousePosition = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotationLength = Mathf.Sqrt((Mathf.Pow(rotation.x, 2f) + Mathf.Pow(rotation.y, 2f) + Mathf.Pow(rotation.z, 2f)));
        Vector3 rotationUnit = new Vector3(rotation.x / rotationLength, rotation.y / rotationLength, rotation.z / rotationLength);
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        
        // Debug.Log(rotationLength);
        // Debug.Log(rotationUnit.x + ", " + rotationUnit.y + ", " + rotationUnit.z);
        // transform.position = rotationUnit * 0.8f;

        Debug.DrawLine(transform.position, mousePosition, Color.white, Time.deltaTime);
    }
}
