using System;
using UnityEngine;

namespace Unity1Week_20230619.Main.Game0
{
    [Serializable]
    public class Parameter
    {
        public int id;
        public int score;
        public int count;
        public int life;
    }


    public class MoguraParameter : MonoBehaviour
    {
        public Parameter param = new Parameter();

        public void Init()
        {
            param.id = 0;
            param.score = 0;
            param.count = 0;
            param.life = 1;
        }

    }
}