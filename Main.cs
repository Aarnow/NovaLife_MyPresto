using Life;
using UnityEngine;

namespace MyPresto
{
    public class Main : Plugin
    {
        public Main(IGameAPI api): base(api)
        {
            Debug.Log($"Plugin \"MyPresto\" initialisé avec succès.");
        }
    }
}