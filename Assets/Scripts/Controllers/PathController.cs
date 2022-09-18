using UnityEngine;
using PathCreation;
using System.Collections.Generic;
using Core;

public class PathController : MonoSingleton<PathController>
{
    #region Components

    private PathCreator _pathController;
    private PathCreator pathCreator => _pathController ??= GetComponent<PathCreator>();

    #endregion

    #region SerializeFields

    [SerializeField]
    private GameObject scorePointObject;

    [SerializeField]
    private int pointCount;

    #endregion

    #region Variables

    private List<GameObject> scorePoints = new List<GameObject>();

    #endregion

    #region Props

    public int PointCount => pointCount;
    public Vector3 FinalPointPosition => pathCreator.path.GetPointAtDistance(pathCreator.path.length - 1);
    public PathCreator PathCreator => pathCreator;

    #endregion

    #region Unity Methods

    private void Awake() 
    {
        GameManager.AfterStateChanged += OnAfterStateChanged;
    }

    private void SpawnPoints() 
    {
        for (int i = 0; i < pointCount; i++)
        {
            var newPointObject = SimplePool.Spawn
            (
                scorePointObject,
                pathCreator.path.GetPointAtDistance(pathCreator.path.length / pointCount * i), 
                pathCreator.path.GetRotationAtDistance(pathCreator.path.length / pointCount * i)
            );
            
            newPointObject.GetComponent<PointController>().Setup();

            scorePoints.Add(newPointObject);
        }
    }

    private void ClearPoints()
    {
        foreach (var point in scorePoints)
        {
            SimplePool.Despawn(point);
        }

        scorePoints.Clear();
    }

    #endregion

    #region Callbacks

    private void OnAfterStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                ClearPoints();
                SpawnPoints();
                break;
        }
    }

    #endregion
}
