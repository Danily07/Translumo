using Translumo.Translation.Configuration;

namespace Translumo.Translation.Yandex
{
    public sealed class YandexContainer : TranslationContainer
    {
        public YandexReaderProxy Reader { get; set; }
        public string Sid { get; set; }

        private int _requestNumber = -1;

        public YandexContainer(Proxy proxy = null, bool isPrimary = false) : base(proxy, isPrimary)
        {
            Reader = new YandexReaderProxy(proxy);
        }

        public int GetRequestNumber()
        {
            lock (Obj)
            {
                return ++_requestNumber;
            }
        }

        public override void Reset()
        {
            base.Reset();
            if (FailUsesCounter >= 2)
            {
                Sid = null;
                _requestNumber = -1;
                Reader = new YandexReaderProxy();
            }
        }
    }
}
