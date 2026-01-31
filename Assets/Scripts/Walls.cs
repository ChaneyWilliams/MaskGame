using UnityEngine;

public class Walls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "MyGameObject")
            {
                //If the GameObject's name matches the one you suggest, output this message in the console
                Debug.Log("Do something here");
            }

            //Check for a match with the specific tag on any GameObject that collides with your GameObject
            if (collision.gameObject.tag == "MyGameObjectTag")
            {
                //If the GameObject has the same tag as specified, output this message in the console
                Debug.Log("Do something else here");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
