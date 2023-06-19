using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week_20230619.Main.Game2
{
    public class MiniGameController2 : MiniGameControllerBase, IController, IInitializa
    {
        public void Init()
        {
            base.Init();
            Announce.text = "MiniGameController2 \nに、チャレンジ！";
        }

        public void Control()
        {
            base.Control();

            if (gameState != GameState.Play) return;

            // TODO
            if (Input.GetButtonDown("Submit") || timerController.ElapsedTime <= 0f)
            {
                gameState = GameState.End;
            }

            if (gameState == GameState.End && drawResultCoroutine == null)
            {
                drawResultCoroutine = StartCoroutine(DrawResult(66,2));
            }
        }

    }
}

