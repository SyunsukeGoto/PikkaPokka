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
            _animator.SetTrigger("Idle");
        }

        public void Walk()
        {
            _animator.SetTrigger("Walk");
        }

        public void FrontSwing()
        {
            _animator.SetTrigger("FrontSwing");
        }

        public void Masturbation()
        {
            _animator.SetTrigger("Masturbation");
        }
    }
}
