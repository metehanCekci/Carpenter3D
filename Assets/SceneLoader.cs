using UnityEngine;
using System.Collections.Generic;

public class SceneReloader : MonoBehaviour
{
    // A class to store the initial state of each object
    [System.Serializable]
    public class ObjectState
    {
        public GameObject gameObject;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public Vector3 initialScale;

        public ObjectState(GameObject obj)
        {
            gameObject = obj;
            initialPosition = obj.transform.position;
            initialRotation = obj.transform.rotation;
            initialScale = obj.transform.localScale;
        }
    }

    // List to store initial states of all objects
    public List<ObjectState> initialStates = new List<ObjectState>();

    void Start()
    {
        // Find all objects with the tag "CheckpointObject"
        Tags[] checkpointObjects = FindObjectsOfType(typeof(Tags)) as Tags[];;

        // Store their initial states
        foreach (Tags obj in checkpointObjects)
        {
            if(obj.HasTag("Reset"))
            initialStates.Add(new ObjectState(obj.gameObject));
        }
    }

    // Function to reset all objects to their initial states
    public void ResetToCheckpoint()
    {
        foreach (ObjectState state in initialStates)
        {
            state.gameObject.transform.position = state.initialPosition;
            state.gameObject.transform.rotation = state.initialRotation;
            state.gameObject.transform.localScale = state.initialScale;
            state.gameObject.SetActive(true);

            // Optional: Reset any other components (e.g., Rigidbody, Animator) if needed
            Rigidbody rb = state.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Animator animator = state.gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Rebind();
                animator.Update(0f);
            }
        }
    }
}
