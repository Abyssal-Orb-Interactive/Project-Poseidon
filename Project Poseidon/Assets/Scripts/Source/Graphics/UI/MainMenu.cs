using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Graphics.UI
{
   public class MainMenu : MonoBehaviour
   {
      public void StartPlaying()
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }

      public void QuiteGame()
      {
         Application.Quit();
      }
   }
}