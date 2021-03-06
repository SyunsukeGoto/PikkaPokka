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
            _anime.SetBool("Idle", true);
            _anime.SetBool("Walk", false);
            _anime.SetBool("FrontSwing", false);
            _anime.SetBool("Masturbation", false);
        }

        public void Walk()
        {
            _anime.SetBool("Idle", false);
            _anime.SetBool("Walk",true);
            _anime.SetBool("FrontSwing", false);
            _anime.SetBool("Masturbation", false);
        }

        public void FrontSwing()
        {
            _anime.SetBool("Idle", false);
            _anime.SetBool("Walk", false);
            _anime.SetBool("FrontSwing", true);
            _anime.SetBool("Masturbation", false);
        }

        public void Masturbation()
        {
            _anime.SetBool("Idle", false);
            _anime.SetBool("Walk", false);
            _anime.SetBool("FrontSwing", false);
            _anime.SetBool("Masturbation", true);
        }
    }
}
