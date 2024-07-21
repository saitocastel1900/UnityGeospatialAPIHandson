using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class PlaneDetectionManager : MonoBehaviour
{
    /// <summary>
    /// ARPlaneManager
    /// </summary>
    [SerializeField] private ARPlaneManager _planeManager;

    /// <summary>
    /// ARRaycastManager
    /// </summary>
    [SerializeField] private ARRaycastManager _raycastManager;

    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<ARRaycastHit> OnRaycastHit　=> _onRaycastHit;
    private ReactiveProperty<ARRaycastHit> _onRaycastHit = new ReactiveProperty<ARRaycastHit>();

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private PlacedObjectTapPointManager _placedObjectManager;

    /// <summary>
    /// 
    /// </summary>
    [Inject] private IInputEventProvider _input;


    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        _placedObjectManager
            .IsPlacedObject
            .Where(isPlacedObject => isPlacedObject = true)
            .Subscribe(_ =>
            {
                foreach (var plane in _planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
            })
            .AddTo(this.gameObject);

        _input
            .InputTapPosition
            .SkipLatestValueOnSubscribe()
            .Subscribe(position =>
            {
                var hits = new List<ARRaycastHit>();

                if (_raycastManager.Raycast(position, hits, TrackableType.Planes))
                {
                    _onRaycastHit.Value = hits[0];
                }
            })
            .AddTo(this.gameObject);
    }
}