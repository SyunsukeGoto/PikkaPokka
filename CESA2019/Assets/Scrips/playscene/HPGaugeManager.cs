using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Makoto
{
    public class HPGaugeManager : MonoBehaviour
    {
        [SerializeField, Header("緑のゲージ")]
        private Image _hpGreen;

        [SerializeField, Header("赤のゲージ")]
        private Image _hpRed;

        public Image RED
        {
            get { return _hpRed; }
        }

        [SerializeField, Header("何回に分けて赤のゲージを減らすか")]
        private int _count;

        [SerializeField, Header("何秒に一回赤のゲージを減らすか")]
        private float _time;

        /// <summary>
        /// HPの表示を減らす処理
        /// </summary>
        /// <param name="currentHP">減少後のHP</param>
        /// <param name="lastHP">減少前のHP</param>
        /// <param name="maxHP">最大HP</param>
        public void HPDown(float currentHP, float lastHP, float maxHP)
        {
            _hpGreen.fillAmount = currentHP / maxHP;
            StartCoroutine(GraduallyDown(currentHP, lastHP));
        }

        private IEnumerator GraduallyDown(float currentHP, float lastHP)
        {
            yield return new WaitForSeconds(1f);

            float value = 0f;

            for (int i = 0; i < _count; i++)
            {
                value = (lastHP - currentHP) / 100f / _count;
                _hpRed.fillAmount -= value;

                yield return new WaitForSeconds(_time);
            }
        }
    }
}
