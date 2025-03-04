using UnityEngine;

public class InteractionsScene : MonoBehaviour
{
    private Animator phoneAnimatorRef;
    private bool isShowing = false;

    void Awake()
    {
        // Cache the Animator reference once
        phoneAnimatorRef = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Toggle the state

            // Update the Animator parameter
            phoneAnimatorRef.SetBool("f_pressed?", isShowing);
            isShowing = !isShowing;
            Debug.Log("F pressed and now: " +  isShowing);
        }
    }
}

