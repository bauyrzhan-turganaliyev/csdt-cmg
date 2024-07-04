using System;

namespace Infrastructure
{
    public class MessageBus
    {
        public Action OnCardFlip;
        public Action<bool> OnCheckMatch;
        public Action OnForceSaveGameData;
        public Action<bool> OnGameOver;
        public Action OnClearProgress;
        
        public Action OnForceSave;
    }
}