using Item;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemStatsDisplay : MonoBehaviour
    {
        [SerializeField] internal TextMeshProUGUI statsTextTMP;
        
        public void SetStatsText(ItemScriptableObject item)
        {
            statsTextTMP.text = Bold(item.Name) + "\n"
                                                + Bold("Fire Mode:") + "\n" + item.FireMode + "\n"
                                                + Bold("Duration:") + "\n" + item.Duration + "\n"
                                                + Bold("(+) Boosts:") + "\n"
                                                + "Range: " + item.AddRange + "\n"
                                                + "Projectile Speed: " + item.AddProjectileSpeed + "\n"
                                                + "Fire Rate: " + item.AddFireRate + "\n"
                                                + "Projectile Size: " + item.AddProjectileSize + "\n"
                                                + "Damage: " + item.AddDamage + "\n"
                                                + "Accuracy: " + item.AddAccuracy + "\n"
                                                + Bold("(x) Multipliers:") + "\n"
                                                + "Range: " + item.MultRange + "\n"
                                                + "Projectile Speed: " + item.MultProjectileSpeed + "\n"
                                                + "Fire Rate: " + item.MultFireRate + "\n"
                                                + "Projectile Size: " + item.MultProjectileSize + "\n"
                                                + "Damage: " + item.MultDamage + "\n"
                                                + "Accuracy: " + item.MultAccuracy + "\n";
        }

        //This is barely faster than typing it out haha
        public string Bold(string text)
        {
            return "<b>" + text + "</b>";
        }
    }
}
