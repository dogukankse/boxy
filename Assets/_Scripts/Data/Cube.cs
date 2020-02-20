using System;
using UnityEngine;

namespace _Scripts.Data
{
    public enum CubeType
    {
        POINT,
        OBSTACLE,
        BOOSTER
    }

    [Serializable]
    public struct Cube
    {
        public GameObject gameObject { get; }
        public CubeType cubeType { get; }
        public int DOTweenCallID { get; }

        public Cube(GameObject gameObject, CubeType cubeType, int doTweenCallId)
        {
            this.gameObject = gameObject;
            this.cubeType = cubeType;
            DOTweenCallID = doTweenCallId;
        }
    }
}