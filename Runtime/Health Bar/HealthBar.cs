using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hybel.HealthSystem
{
    //// TODO: Actually make this :D
    public class HealthBar : MonoBehaviour
    {
        private List<Slider> _healthSliders;
        private Dictionary<Slider, GameObject> _sliderObjectsBySlider;

        private void Awake()
        {
            _healthSliders = new List<Slider>();
            _sliderObjectsBySlider = new Dictionary<Slider, GameObject>();
        }

        private void AddSlider()
        {
            var sliderObject = new GameObject("Slider", typeof(Slider));
            Slider slider = sliderObject.GetComponent<Slider>();

            _sliderObjectsBySlider.Add(slider, sliderObject);
            _healthSliders.Add(slider);
        }

        private bool RemoveSlider(Slider slider)
        {
            if (_sliderObjectsBySlider.Remove(slider) is false)
                return false;

            if (!_healthSliders.Remove(slider))
                throw new System.InvalidOperationException("Shit went wrong!");

            return true;
        }
    }
}
