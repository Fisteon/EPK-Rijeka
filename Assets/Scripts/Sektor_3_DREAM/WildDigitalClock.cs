using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WildDigitalClock : MonoBehaviour
{
    public TextMeshProUGUI clock;

    int h = 24;
    int m = 0;

    // Start is called before the first frame update
    void Start()
    {
        clock = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        clock.text = IncreaseClockSecond();
    }

    string IncreaseClockSecond()
    {
        m++;
        if (m == 60)
        {
            m = 0;
            h++;
        }
        if (h == 24)
        {
            h = 0;
        }
        return "" + (h.ToString().Length == 1 ? "0" + h.ToString() : h.ToString()) + ":" + (m.ToString().Length == 1 ? "0" + m.ToString() : m.ToString());
    }
}
