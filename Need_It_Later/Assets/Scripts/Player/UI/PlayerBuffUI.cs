using System;
using Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class PlayerBuffUI : MonoBehaviour
    {
        public PlayerBuff TargetPlayerBuff;
        public PlayerBuffUIIcons IconMapping;
        public Image BuffIcon;
        public TMP_Text TargetText;
        public Image RadialIcon;

        public void Init(ItemScriptableObject TargetItem, BuffType BuffStatType, PlayerBuff buffInstance)
        {
            TargetPlayerBuff = buffInstance;
            IconMapping = GetComponentInParent<PlayerBuffUIIcons>();
            BuffIcon.sprite = IconMapping.BuffSpriteMapping[BuffStatType];
        }

        private void Update()
        {
            TargetText.text = (TargetPlayerBuff.Magnitude > 0 ? "+" : "" ) + TargetPlayerBuff.Magnitude.ToString();
            RadialIcon.fillAmount = Mathf.Clamp01(
                1 - (TargetPlayerBuff.ElapsedDuration / TargetPlayerBuff.BaseDuration));
        }

        public void EndBuff(BuffType BuffStatType)
        {
            Destroy(gameObject);
        }
    }
}