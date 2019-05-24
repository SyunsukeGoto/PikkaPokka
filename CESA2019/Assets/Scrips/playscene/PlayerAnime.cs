using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makoto
{
    public class PlayerAnime : MonoBehaviour
    {
        [SerializeField]
        private Animator _anime;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Idle()
        {
            _anime.SetTrigger("Idle");
        }

        public void Walk()
        {
            _anime.SetTrigger("Walk");
        }

        public void SwingDown()
        {
            _anime.SetTrigger("SwingDown");
        }

        public void Masturbation()
        {
            _anime.SetTrigger("Masturbation");
        }

        public void WeakAttack()
        {
            _anime.SetTrigger("WeakAttack");
        }

        public void NomalAttack()
        {
            _anime.SetTrigger("NomalAttack");
        }

        public void StrengthAttack()
        {
            _anime.SetTrigger("StrengthAttack");
        }
    }
}
