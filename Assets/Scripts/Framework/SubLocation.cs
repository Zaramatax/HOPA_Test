using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Framework
{
    public class SubLocation : Location
    {
        protected enum State { Closed, Opening, Open, Closing };

        protected State _state;
        protected Transform mainLocation;
        protected Transform thisSub;

        virtual protected void Awake()
        {
			base.Awake ();
            _state = State.Closed;
        }

        virtual protected void Start() {
            base.Start();

            mainLocation = transform.parent.Find("Main");
            thisSub = transform;
        }

        public void Open()
        {
            if (_state == State.Closed)
            {
                _state = State.Opening;
				GetComponent<Animator>().SetTrigger("open_sub");
				_state = State.Open;
            }
        }

        public void Close()
        {
            if (_state == State.Open)
            {
                _state = State.Closing;
				GetComponent<Animator>().SetTrigger("close_sub");
				_state = State.Closed;
            }
        }
    }
}