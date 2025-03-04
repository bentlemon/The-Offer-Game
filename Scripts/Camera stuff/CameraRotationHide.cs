using UnityEngine;

public class CameraRotationHide : MonoBehaviour
{
    private float x;
    private float y;
    public float sensitivity = -1f;
    private Vector3 rotate;

    void Start()
    {
        // Kolla så den inte skapar problem
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x, y*sensitivity, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;
    }
}
