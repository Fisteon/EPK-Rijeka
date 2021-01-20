using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FlyingPathController : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed;

    float distanceTravelled;
    float pathLength;

    public bool finishFlying;

    // Start is called before the first frame update
    void Start()
    {
        finishFlying = false;
        pathLength = pathCreator.path.length;

        StartCoroutine(SlowDown());
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        //transform.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled + 0.01f));
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }

    IEnumerator SlowDown()
    {
        yield return new WaitUntil(() => distanceTravelled > pathLength - 20f);
        float startingPoint = distanceTravelled;

        while (distanceTravelled < pathLength - 7f)
        {
            speed = Mathf.Lerp(5, 1f, (distanceTravelled - startingPoint) / (pathLength - startingPoint));
            yield return new WaitForEndOfFrame();
        }

        finishFlying = true;
    }
}
