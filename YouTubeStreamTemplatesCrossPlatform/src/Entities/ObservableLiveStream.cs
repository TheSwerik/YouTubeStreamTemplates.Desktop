using System;
using System.Collections.Generic;
using YouTubeStreamTemplates.Exceptions;
using YouTubeStreamTemplates.LiveStreaming;

namespace YouTubeStreamTemplatesCrossPlatform.Entities
{
    public class ObservableLiveStream : LiveStream, IObservable<LiveStream>
    {
        private readonly List<IObserver<LiveStream>> _observers = new();
        private readonly List<Unsubscriber> _unsubscribers = new();
        public LiveStream? CurrentLiveStream { get; private set; }

        public IDisposable Subscribe(IObserver<LiveStream> observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            var unsubscriber = new Unsubscriber(_observers, observer);
            _unsubscribers.Add(unsubscriber);
            return unsubscriber;
        }

        public void OnNext(LiveStream liveStream)
        {
            CurrentLiveStream = liveStream;
            foreach (var observer in _observers) observer.OnNext(liveStream);
        }

        public void OnNext()
        {
            if (CurrentLiveStream == null) throw new NoCurrentObservableStreamException();
            foreach (var observer in _observers) observer.OnNext(CurrentLiveStream);
        }

        public void Dispose()
        {
            foreach (var unsubscriber in _unsubscribers) unsubscriber.Dispose();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly IObserver<LiveStream> _observer;
            private readonly List<IObserver<LiveStream>> _observers;

            public Unsubscriber(List<IObserver<LiveStream>> observers, IObserver<LiveStream> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer)) _observers.Remove(_observer);
            }
        }
    }
}