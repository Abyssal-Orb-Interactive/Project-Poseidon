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
        private Timer _timer;
        private float _remainingTimeInPrecents;

        private void OnValidate()
        {
            _timeBar ??= GetComponent<Image>();
            _timerText ??= GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Initialize(Timer timer)
        {
            _timer = timer;
            Subscribe();
            _timer.TimerPaused += Unsubscribe;
            _timer.TimerResumed += Subscribe;
            _timer.TimerFinished += Unsubscribe;
            _timerText.text = _timer.DelayTimeInSeconds.ToString("F2", CultureInfo.InvariantCulture);
        }

        public void UpdateTimeBar()
        {
            _remainingTimeInPrecents = _timer.RemainingTime / _timer.DelayTimeInSeconds;
            _timeBar.fillAmount = _remainingTimeInPrecents;
            _timerText.text = _timer.RemainingTime.ToString("F2", CultureInfo.InvariantCulture);
        }

        private void Subscribe()
        {
            _timer.TimerTick += UpdateTimeBar;
        }

        private void Unsubscribe()
        {
            _timer.TimerTick -= UpdateTimeBar;
        }
    }
}