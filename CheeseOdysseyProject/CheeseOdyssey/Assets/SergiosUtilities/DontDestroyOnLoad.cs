using UnityEngine;
namespace NicoUtilities
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}