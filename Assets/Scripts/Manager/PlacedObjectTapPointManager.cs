using System;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacedObjectTapPointManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private ARAnchorManager _arAnchorManager;

    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<bool> IsPlacedObject　=> _isPlacedObject;
    private BoolReactiveProperty _isPlacedObject = new BoolReactiveProperty(false);

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GeospatialTrackingManage _geospatialTrackingManage;

    /// <summary>
    /// /
    /// </summary>
    [SerializeField] private PlaneDetectionManager _planeDetectionManager;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] PlacedObjectProvider _placedObjectProvider;

    /// <summary>
    /// 
    /// </summary>
    private GameObject _cashGameObject;
    
    /// <summary>
    /// 
    /// </summary>
    string anchorKey = "earth_data";
    
    /// <summary>
    /// 
    /// </summary>
    string positionKey = "local_position";

    private void Start()
    {
        _geospatialTrackingManage
            .CurrentTrackingStateProp
            .Where(state => state == TrackingStatus.State.HighTrackingAccuracy)
            .SelectMany(_=>_planeDetectionManager.OnRaycastHit.SkipLatestValueOnSubscribe())
            .FirstOrDefault()
            .Where(_=>_cashGameObject==null)
            .Subscribe(_ => PlacedObject())
            .AddTo(this.gameObject);
        
        _geospatialTrackingManage
            .CurrentTrackingStateProp
            .Where(state => state == TrackingStatus.State.HighTrackingAccuracy)
            .SelectMany(_=>_planeDetectionManager.OnRaycastHit.SkipLatestValueOnSubscribe())
            .Where(_=>_cashGameObject!=null)
            .Subscribe(hit => CreateAnchor(hit,_geospatialTrackingManage.CurrentGeospatialPose))
            .AddTo(this.gameObject);
    }

    private void CreateAnchor(ARRaycastHit hit,GeospatialPose pose)
    {
        //置く場所はレイキャストで得た位置情報を設定
        _cashGameObject.transform.position = hit.pose.position;
        
        //角度を補正（北向を基準に）
        Quaternion quaternion = pose.EunRotation;

        //アンカー生成
        ARGeospatialAnchor anchor = _arAnchorManager.AddAnchor(pose.Latitude, pose.Longitude, pose.Altitude, quaternion);
        
        //アンカーを生成出来たら、、
        if (anchor != null)
        {
            //アンカーを新しいやつと古いやつを入れ替える
            Transform prevAnchor = _cashGameObject.transform.parent;
            _cashGameObject.transform.SetParent(anchor.transform);

            //古いアンカーは削除する
            if (prevAnchor != null)
            {
                Destroy(prevAnchor.gameObject);
            }

            //データを保存する
            //異常終了したらデータは書き込まれない(Application.Quit()でゲームを終了しないなど)
            GeospatialAnchorHistory history = new GeospatialAnchorHistory(DateTime.Now, pose.Latitude, pose.Longitude,
                pose.Altitude, AnchorType.Terrain, pose.EunRotation);
            PlayerPrefs.SetString(positionKey, JsonUtility.ToJson(_cashGameObject.transform.localPosition));
            PlayerPrefs.SetString(anchorKey, JsonUtility.ToJson(history));
            PlayerPrefs.Save();
        }
    }

    private void PlacedObject()
    {
        _cashGameObject = _placedObjectProvider.CreatePlacedObject();
        
        //4m前方にオブジェクトを置く
        _cashGameObject.transform.position = new Vector3(0, 0, 4);
        _cashGameObject.transform.rotation = Quaternion.identity;
        
        //前回のデータが存在していた場合
        if (PlayerPrefs.HasKey(anchorKey) && PlayerPrefs.HasKey(positionKey))
        {
            //PlayerPrefsで保存されていたデータをGeospatialAnchorHistoryオブジェクトに変換
            GeospatialAnchorHistory history =
                JsonUtility.FromJson<GeospatialAnchorHistory>(PlayerPrefs.GetString(anchorKey));

            //角度の補正（北向き）
            Quaternion rotation = history.EunRotation;

            //アンカーの生成
            ARGeospatialAnchor anchor = _arAnchorManager.AddAnchor(
                    history.Latitude,
                    history.Longitude,
                    history.Altitude,
                    rotation);

            //アンカーが生成出来たら、アンカーの生成場所を過去のデータの位置情報に設定
            if (anchor != null)
            {
                _cashGameObject.transform.SetParent(anchor.transform);
                _cashGameObject.transform.localPosition =
                    JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(positionKey));
            }
        }
    }
}