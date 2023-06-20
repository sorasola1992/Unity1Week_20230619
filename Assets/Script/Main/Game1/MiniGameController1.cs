
using UnityEngine;

namespace Unity1Week_20230619.Main.Game1
{
    public class MiniGameController1 : MiniGameControllerBase, IController, IInitializa
    {

        public void Init()
        {
            base.Init();
            Announce.text = "MiniGameController1 \nに、チャレンジ！";
        }

        public void Control()
        {
            base.TimeUpdate();

            if (gameState != GameState.Play) return;

            // TODO
            if (Input.GetButtonDown("Submit") || timerController.ElapsedTime <= 0f)
            {
                gameState = GameState.End;
            }

            if (gameState == GameState.End && drawResultCoroutine == null)
            {
                drawResultCoroutine = StartCoroutine(DrawResult(22, 1));
            }

        }


    }
}
