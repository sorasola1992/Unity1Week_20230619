using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week_20230619.Main.Game1
{
    [Serializable]
    public class Parameter
    {
        public int id;
        public int power;
    }

    public class PlayerParameter : MonoBehaviour
    {
        public Parameter param = new Parameter();

        public void Init()
        {
            param.id = 4;
            param.power = 1;
        }

    }

}