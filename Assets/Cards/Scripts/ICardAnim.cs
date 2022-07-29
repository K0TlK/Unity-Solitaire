using UnityEngine;

namespace Cards
{
    interface ICardAnim
    {
        /// <summary>
        /// Play this anim, when user take card
        /// </summary>
        public void OnFirstTake();

        /// <summary>
        /// Play this anim, when user take stack cards
        /// </summary>
        public void OnTake();

        /// <summary>
        /// Play this anim, when user put stack cards
        /// </summary>
        public void OnPut();

        /// <summary>
        /// Card flip animation. As a result, The card will be face up.
        /// </summary>
        public void OnUnlock();

        /// <summary>
        /// Card flip animation. As a result, the card will be hidden.
        /// </summary>
        public void OnLock();

        /// <summary>
        /// Set new position
        /// </summary>
        /// <param name="newpos">New position</param>
        public void OnNewPos(Vector3 newpos);
    }

}
