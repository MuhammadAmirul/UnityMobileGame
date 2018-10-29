using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    [Header("Manually Assigned Variables")]
    public Slider sliderHP;
    public Slider sliderOverheat;

    public Image fillHP;

    [Header("Runtime Assigned Variables")]
    public Transform tTarget;

    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();    
    }

    public void Recycle()
    {
        rt.anchoredPosition = new Vector2(-1000f, 0f);
        fillHP.color = Color.green;
        gameObject.SetActive(false);
    }

    public void Init(UNITFACTION faction)
    {
        sliderHP.value = 1f;
        sliderOverheat.value = 0f;

        if (faction == UNITFACTION.PLAYER)
        {
            //Turn on Overheat.
            sliderOverheat.gameObject.SetActive(true);
            
        }else if(faction == UNITFACTION.ENEMY)
        {
            sliderOverheat.gameObject.SetActive(false);
        }

        this.gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if(tTarget == null)
        {
            return;
        }
        else
        {
            Vector2 sp = Camera.main.WorldToScreenPoint(tTarget.position);
            //Debug.Log(sp);
            this.transform.position = sp;
        }
    }

    public void UpdateHPSlider(float val)
    {
        sliderHP.value = val;
        if(val <= 0.5f)
        {
            fillHP.color = Color.yellow;
        }
        else if(val <= 0.25f)
        {
            fillHP.color = Color.red;
        }
        else
        {
            fillHP.color = Color.green;
        }
    }
}
