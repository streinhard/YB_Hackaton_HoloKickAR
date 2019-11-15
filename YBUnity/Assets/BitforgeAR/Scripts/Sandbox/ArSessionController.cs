using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

// ReSharper disable SuggestBaseTypeForParameter

public class ArSessionController : MonoBehaviour
{
    private static readonly List<ARPlane> Planes = new List<ARPlane>();

    public Material PlaneFillMaterial;
    public Material PlaneLineMaterial;
    public Material PlaneInvisibleMaterial;

    private ARSessionOrigin _sessionOrigin;
    private ARPointCloudManager _pointCloudManager;
    private ARPlaneManager _planeManager;
    private ARReferencePointManager _referencePointManager;

    private readonly ColorGenerator _colorGenerator = new ColorGenerator();

    private bool _showArEnvironment = true;

    private void Awake()
    {
        _sessionOrigin = GetComponent<ARSessionOrigin>();
        _pointCloudManager = GetComponent<ARPointCloudManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        _referencePointManager = GetComponent<ARReferencePointManager>();
    }

    private void OnEnable()
    {
        _planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        _planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs e)
    {
        if(e.added.Count > 0){
            var plane = e.added[0];
            if (_showArEnvironment) {
                ColorizePlane(plane);
            }
            else {
                HidePlane(plane);
            }
        }
    }

    public void ToggleArEnvironmentVisibility()
    {
        _showArEnvironment = !_showArEnvironment;
        _pointCloudManager.gameObject.SetActive(_showArEnvironment);

        if (_showArEnvironment) {
            ShowAllPlanes();
        }
        else {
            HideAllPlanes();
        }
    }

    public void ShowAllPlanes()
    {
        foreach (var plane in _planeManager.trackables) {
            ColorizePlane(plane);
        }
    }

    public void HideAllPlanes()
    {
        foreach (var plane in _planeManager.trackables) {
            HidePlane(plane);
        }
    }

    private void ColorizePlane(ARPlane plane)
    {
        var meshRender = plane.GetComponent<MeshRenderer>();
        var lineRenderer = plane.GetComponent<LineRenderer>();
        meshRender.material = PlaneFillMaterial;
        lineRenderer.material = PlaneLineMaterial;

        var color = _colorGenerator.GetNext();
        var colorWithAlpha = color;
        colorWithAlpha.a = 0.2f;

        meshRender.materials[0].color = colorWithAlpha;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    private void HidePlane(ARPlane plane)
    {
        var meshRender = plane.gameObject.GetComponent<MeshRenderer>();
        var lineRenderer = plane.gameObject.GetComponent<LineRenderer>();
        meshRender.material = PlaneInvisibleMaterial;
        lineRenderer.material = PlaneInvisibleMaterial;
    }
}
