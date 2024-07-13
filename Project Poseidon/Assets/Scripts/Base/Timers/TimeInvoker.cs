using System;
using UnityEngine;

namespace Base.Timers
{
    public delegate void FrameTimeAction();
    public delegate void UnitTimeAction();

    public class TimeInvoker : MonoBehaviour
    {
        public event FrameTimeAction FrameUpdated;
        public event FrameTimeAction UnscaledFrameUpdated;
        public event UnitTimeAction SecondUpdated;
        public event UnitTimeAction UnscaledSecondUpdated;

        private float _oneSecondTime;
        private float _unscaledOneSecondTime;

        private static TimeInvoker _instance;
        public static TimeInvoker Instance
        {
            get
            {
                if (_instance == null)
                {
                   CreateInstance();
                }
                return _instance;
            }
        }
        
        private static void CreateInstance()
        {
            var gameObject = new GameObject("Time Invoker");
            _instance = gameObject.AddComponent<TimeInvoker>();
            DontDestroyOnLoad(gameObject);
        }
        
        public void UpdateTimer()
        {
            var deltaTime = GetDeltaTime();
            FrameUpdated?.Invoke();

            var timeScale = GetTimeScale();
            _oneSecondTime += deltaTime;
            if (_oneSecondTime >= 1f)
            {
                _oneSecondTime -= 1f;
                SecondUpdated?.Invoke();
            }

            var unscaledDeltaTime = GetUnscaledDeltaTime();
            UnscaledFrameUpdated?.Invoke();

            _unscaledOneSecondTime += unscaledDeltaTime;
            if (_unscaledOneSecondTime >= 1f)
            {
                _unscaledOneSecondTime -= 1f;
                UnscaledSecondUpdated?.Invoke();
            }
        }

        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        public float GetUnscaledDeltaTime()
        {
            return Time.unscaledDeltaTime;
        }

        public float GetTimeScale()
        {
            return Time.timeScale;
        }

        private void OnDestroy()
        {
            FrameUpdated = null;
            SecondUpdated = null;
            UnscaledFrameUpdated = null;
            UnscaledSecondUpdated = null;
            _instance = null;
        }
    }
}