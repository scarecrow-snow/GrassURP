using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeFactory : MonoBehaviour
{
    public Collider _collier;
    public int _depth;

    QuadTreeNode _topNode;
    Camera _cam;

    public GameObject[] _cubes;


    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _topNode = new QuadTreeNode(_collier.bounds, _depth, false);

        var bounds = new List<Bounds>();
        var visibleMatrixList = new List<Matrix4x4>();

        // 判定するオブジェクトのMatrix4x4をQuadTreeに格納する
        foreach(var obj in _cubes)
        {
            _topNode.FindLeafForPoint(obj.transform.localToWorldMatrix);
        }


        // カメラの視錐台を取得する
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        System.Array.Resize(ref planes, 4);

        // QuadTreeと視推台の交差判定をとるboundsに交差しているtreeがもつboundsが入る
        _topNode.RetrieveLeaves(planes, bounds, visibleMatrixList);

        foreach(var b in bounds)
        {
            //Debug.Log(b);
        }

        foreach(var m in visibleMatrixList)
        {
            Debug.Log(m);
        }


    }

    void OnDestory()
    {
        _topNode.ClearEmpty();
        
    }



}
