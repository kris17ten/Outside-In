using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class ARController : MonoBehaviour {
    //will contain ARCore detected planes
    private List<TrackedPlane> NewTrackedPlanes = new List<TrackedPlane>();

    public GameObject Grid;
    public GameObject Portal;
    public GameObject ARCamera;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        //return;
        //check ARCore session status
        if(Session.Status != SessionStatus.Tracking) {
            return;
        }

        //store ARCore detected planes in the list
        Session.GetTrackables<TrackedPlane>(NewTrackedPlanes, TrackableQueryFilter.New);

        //instantiate grid for each TrackedPlane
        for (int i = 0; i < NewTrackedPlanes.Count; i++) {
            GameObject grid = Instantiate(Grid, Vector3.zero, Quaternion.identity, transform);

            grid.GetComponent<GridVisualizer>().Initialize(NewTrackedPlanes[i]);
        }

        //check for user touch
        Touch touch;
        if(Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) {
            return;
        }

        //check if user touches any of the tracked planes
        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit) && !Portal.active) {
            //place the portal
            Portal.SetActive(true);

            //create anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            //set position and rotation of portal
            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            //set portal to face camera
            Vector3 cameraPosition = ARCamera.transform.position;

            //the portal should only rotate on y axis
            cameraPosition.y = hit.Pose.position.y;

            Portal.transform.LookAt(cameraPosition, Portal.transform.up);

            Portal.transform.parent = anchor.transform;
        }
    }
}
