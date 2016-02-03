using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework {
    public class CollectionItemZone : MonoBehaviour, IPointerClickHandler {
        public string collectionId;
        public string itemId;

        public int scoreValue;

        void Start() {
            if(RewardManager.Instance.IsCollected(collectionId, itemId))
                gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData) {
            RewardManager.Instance.CollectItem(collectionId, itemId);
            RewardManager.Instance.AddScorePoints(scoreValue);
            gameObject.SetActive(false);
        }
    }
}
