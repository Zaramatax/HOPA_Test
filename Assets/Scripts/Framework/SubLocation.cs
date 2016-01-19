using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Framework
{
    public class SubLocation : Location
    {
        protected enum State { Closed, Opening, Open, Closing };

        protected State _state;
        protected Transform mainLocation;
        protected Transform thisSub;

        private GameObject closeButton;

        virtual protected void Awake()
        {
			base.Awake ();
            _state = State.Closed;
        }

        virtual protected void Start() {
            base.Start();

            closeButton = GameObject.FindObjectOfType<CloseSubLocation>().gameObject;
            mainLocation = GameObject.FindGameObjectWithTag("Main").transform;
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

        public bool IsOpen() {
            return State.Open == _state;
        }

        virtual protected void AddTransferZone(ref List<HintInfo> result) {
            if (0 == result.Count) {
                result.Add(HintInfo.CreateHint(closeButton));
            }
        }
    }
}