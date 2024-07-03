using System;

namespace Infrastructure
{
    public class MessageBus
    {
        public Action OnCardFlip;
        public Action<bool> OnCheckMatch;
    }
}