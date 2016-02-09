using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Framework
{
    public class SubLocation : Location
    {
        protected enum State { Closed, Opening, Open, Closing };

        protected State state;
        protected Transform mainLocation;
        protected Transform thisSub;

        private GameObject closeButton;

        virtual protected void Awake()
        {
			base.Awake ();
            state = State.Closed;
        }

        virtual protected void Start() {
            base.Start();

            closeButton = GameObject.FindObjectOfType<CloseSubLocation>().gameObject;
            mainLocation = GameObject.FindGameObjectWithTag("Main").transform;
            thisSub = transform;
        }

        public void Open()
        {
            if (state == State.Closed)
            {
                state = State.Opening;
				GetComponent<Animator>().SetTrigger("open_sub");
				state = State.Open;
            }
        }

        public void Close()
        {
            if (state == State.Open)
            {
                state = State.Closing;
				GetComponent<Animator>().SetTrigger("close_sub");
				state = State.Closed;

                gameObject.SetActive(false);
            }
        }

        public void Disable() {
            GetComponent<Animator>().Play("disable_sub");
        }

        public void Enable() {
            GetComponent<Animator>().Play("enable_sub");
        }

        public bool IsOpen() {
            return State.Open == state;
        }

        override protected void AddTransferZone(List<HintInfo> result) {
            if (0 == result.Count) {
                result.Add(HintInfo.CreateHint(closeButton));
            }
        }
    }
}