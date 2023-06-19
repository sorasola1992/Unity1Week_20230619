using UnityEngine;

namespace Unity1Week_20230619.Main
{
    public class MiniGameControllerTemplate : MiniGameControllerBase, IController, IInitializa
    {
        public void Init()
        {
            base.Init();
            Announce.text = "MiniGameControllerTemplate \nに、チャレンジ！";
        }

        public void Control()
        {
            base.Control();

            if (gameState != GameState.Play) return;

            // TODO
            if (Input.GetButtonDown("Submit"))
            {
                gameState = GameState.End;
            }

            if (gameState == GameState.End && drawResultCoroutine == null)
            {
                drawResultCoroutine = StartCoroutine(DrawResult(55,0));
            }
        }
    }
}
