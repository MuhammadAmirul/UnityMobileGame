using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsUI : MonoBehaviour {

    public Transform tUnit;

    public Slider sliderHP;
    public Slider sliderSP;

    private RectTransform rt;
    private CanvasGroup cGroup;
    private bool isHidingUI;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        cGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void FixedUpdate () {
		if(tUnit == null)
        {
            return;
        }
        else
        {
            //Follow.
            Vector2 sp = Camera.main.WorldToScreenPoint(tUnit.position);
            this.transform.position = sp;
        }
	}

    public void UpdateSliderHP(float val)
    {
        cGroup.alpha = 1f;
        if (sliderHP)
        {
            sliderHP.value = val;
        }

        if (!isHidingUI)
        {
            Invoke("HideUI", 1f);
            isHidingUI = true;
        }
    }

    public void UpdateSliderSP(float val)
    {
        if (sliderSP)
        {

        }
    }

    public void HideUI()
    {
        cGroup.alpha = 0f;
        isHidingUI = false;
    }

    public void Deactivate()
    {
        isHidingUI = false;
        cGroup.alpha = 0f;
        tUnit = null;
        gameObject.SetActive(false);
    }
}
