using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Timer = Base.Timers.Timer;

namespace Source.Graphics.UI
{
    public class TimerLine : MonoBehaviour
    {
        [SerializeField] private Image _timeBar;
        [SerializeField] private TextMeshProUGUI _timerText;
        private TimeToTurnTracker _timer;
        private float _remainingTimeInPercentage;

        private void OnValidate()
        {
            _timeBar ??= GetComponent<Image>();
            _timerText ??= GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Initialize(TimeToTurnTracker timer)
        {
            _timer = timer;
            Subscribe();
            _timer.SubscribeToTimePaused(Unsubscribe);
            _timer.SubscribeToTimeResumed(Subscribe);
            _timerText.text = _timer.DelayTime.ToString("F2", CultureInfo.InvariantCulture);
        }

        private void UpdateTimeBar()
        {
            _remainingTimeInPercentage = _timer.RemainingTime / _timer.DelayTime;
            _timeBar.fillAmount = _remainingTimeInPercentage;
            _timerText.text = _timer.RemainingTime.ToString("F2", CultureInfo.InvariantCulture);
        }

        private void Subscribe()
        {
            _timer.SubscribeToTimeTicked(UpdateTimeBar);
        }

        private void Unsubscribe()
        {
            _timer.UnSubscribeToTimeTicked(UpdateTimeBar);
        }

        private void OnDestroy()
        {
            Unsubscribe();
            _timer.Dispose();
            _timerText = null;
            _timeBar = null;
        }
    }
}