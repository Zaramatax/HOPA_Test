using UnityEngine;
using System.Collections;

namespace Framework {
    public class CommentZone : MonoBehaviour {

        [LocalizationString]
        public string comment;

        private CommentManager commentManager;

        void Start() {
            commentManager = CommentManager.instance;
        }

        void OnMouseDown() {
            commentManager.Show(comment);
        }
    }
}