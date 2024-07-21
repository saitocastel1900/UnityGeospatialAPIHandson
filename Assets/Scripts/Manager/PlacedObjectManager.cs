using Google.XR.ARCoreExtensions;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacedObjectManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private ARAnchorManager _arAnchorManager;
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GeospatialTrackingManage _geospatialTrackingManage;
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] PlacedObjectProvider _placedObjectProvider;

    /// <summary>
    /// 
    /// </summary>
    private GameObject _cashGameObject;
    
    private void Start()
    {
        _geospatialTrackingManage
            .CurrentTrackingStateProp
            .Where(state=>state==TrackingStatus.State.HighTrackingAccuracy)
            .Subscribe(_=>CreateAnchorAndPlaceObject())
            .AddTo(this.gameObject);
    }

    private void CreateAnchorAndPlaceObject()
    {
        if (_cashGameObject != null) return;
        
        var rotation = Quaternion.AngleAxis(180f - (float)_placedObjectProvider.PlacedObject.Heading,
            Vector3.up);
            
        var anchor = _arAnchorManager.AddAnchor(
            _placedObjectProvider.PlacedObject.Latitude,
            _placedObjectProvider.PlacedObject.Longitude,
            _placedObjectProvider.PlacedObject.Altitude,
            rotation);

        if (anchor == null) return;
        _cashGameObject = _placedObjectProvider.CreatePlacedObject();
        _cashGameObject.transform.SetParent(anchor.transform);
    }
}