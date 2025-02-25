﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuController : MonoBehaviour
{
    public GameObject logointro;
    public GameObject canvas, background, logo, startGame, options;
    public GameObject slttl, backbtn, sl1, sl2, sl3;
    public GameObject trashbutton, trashbin_closed, trashbin_open;
    public GameObject entername, et_input, et_confirm, et_return;
    private int instatus; // 1 = canvas, 2 = chose game, 3 = options menu
    private int selectedSlot;
    private bool firstrun, inOptions, creatingSlots, trashbinOpen;
    void Start()
    {
        canvas.SetActive(false);
        slttl.SetActive(false);
        sl1.SetActive(false);
        sl2.SetActive(false);
        sl3.SetActive(false);
        trashbin_closed.SetActive(false);
        trashbin_open.SetActive(false);
        backbtn.SetActive(false);
        creatingSlots = false;
        firstrun = true;
        inOptions = false;
        trashbinOpen = false;
        selectedSlot = 0;
        canvasin();
    }


    public void canvasin()
    {
        instatus = 1;
        StartCoroutine(passiveCanvasin());
    }

    public void canvasout()
    {
        StartCoroutine(passiveCanvasout());
    }

    public void choseSlot()
    {
        instatus = 2;
        if (!creatingSlots)
        {
            StartCoroutine(passiveCanvasout());
        }
        GetComponent<datacontrol>().setslotvals();
        StartCoroutine(passiveCallSlot());
    }

    public void backSlot()
    {
        StartCoroutine(passiveExitSlot());
    }

    void optionsbuttonshow()
    {
        options.SetActive(true);
        options.GetComponent<Animator>().Play("options-intro-fade");
    }

    public void optionsclicked()
    {
        if (!inOptions && instatus == 1) {/*StopCoroutine(passiveCanvasin());*/ canvasout(); }
        else if (!inOptions && instatus == 2) {/*StopCoroutine(passiveCallSlot());*/ backSlot(); }
        if (!inOptions) { options.GetComponent<optionsMenu>().entermenu(); inOptions = true; }

    }

    public void returnFromMenu()
    {
        if (instatus == 1) { canvasin(); }
        else if (instatus == 2) { choseSlot(); }
    }

    public void createNewSlot(int slotval)
    {
        backSlot();
        entername.transform.position = new Vector3(entername.transform.position.x, 2000, entername.transform.position.z);
        entername.SetActive(true);
        selectedSlot = slotval;
        creatingSlots = true;
        iTween.MoveTo(entername, iTween.Hash("time", 1, "delay", 1, "y", 0, "oncomplete", "enableEnterGameBool", "oncompletetarget", gameObject));
    }

    public void enableEnterGameBool()
    {
        creatingSlots = true;
        instatus = 3;
    }


    public void enterGameSetReturn()
    {
        entername.SetActive(false);
        creatingSlots = false;
    }

    public void enterNameConfirm()
    {
        if (creatingSlots && et_input.GetComponent<Text>().text != "")
        {
            GetComponent<datacontrol>().createGame(selectedSlot, et_input.GetComponent<Text>().text);
        }
        et_return.GetComponent<Button>().interactable = false;
    }

    public void enterNameReturn()
    {
        if (creatingSlots)
        {
            logo.SetActive(true);
            startGame.SetActive(true);
            iTween.MoveTo(entername, iTween.Hash("time", 1, "y", 2000, "oncomplete", "enterGameSetReturn", "oncompletetarget", gameObject));
            GetComponent<datacontrol>().wipeSlot(selectedSlot);
            canvasin();
        }
    }

    public void trashbinClicked()
    {
        if (instatus == 2)
        {
            if (trashbinOpen)
            {
                trashbin_open.SetActive(true);
                trashbin_closed.SetActive(false);
                trashbinOpen = false;
                GetComponent<datacontrol>().trashBinOpen();
            }
            else
            {
                trashbin_open.SetActive(false);
                trashbin_closed.SetActive(true);
                trashbinOpen = true;
                GetComponent<datacontrol>().trashBinClose();
            }
        }
    }


    // passive programs below -------------------------------------------------

    IEnumerator passiveCanvasin()
    {
        canvas.SetActive(true);
        logo.SetActive(false);
        startGame.SetActive(false);
        if (firstrun) { optionsbuttonshow(); yield return new WaitForSeconds(1); firstrun = false; }
        options.GetComponent<optionsMenu>().nowInAnimation();
        logo.SetActive(true);
        logo.GetComponent<Animator>().Play("logo-intro");
        yield return new WaitForSeconds(1);
        startGame.SetActive(true);
        startGame.GetComponent<Animator>().Play("startgame-intro-fade");
        inOptions = false;
        options.GetComponent<optionsMenu>().AnimationDone();
    }

    IEnumerator passiveCanvasout()
    {
        options.GetComponent<optionsMenu>().nowInAnimation();
        logo.GetComponent<Animator>().Play("logo-outro");
        startGame.GetComponent<Animator>().Play("startgame-outro-fade");
        yield return new WaitForSeconds(1);
        logo.SetActive(false);
        startGame.SetActive(false);
        options.GetComponent<optionsMenu>().AnimationDone();
    }

    IEnumerator passiveCallSlot()
    {
        options.GetComponent<optionsMenu>().nowInAnimation();
        yield return new WaitForSeconds(1);
        slttl.SetActive(true);
        backbtn.SetActive(true);
        trashbin_closed.SetActive(true);
        slttl.GetComponent<Animator>().Play("slttl-intro-fade");
        backbtn.GetComponent<Animator>().Play("slotBack-intro-fade");
        yield return new WaitForSeconds(0.2f);
        sl1.SetActive(true);
        sl1.GetComponent<Animator>().Play("s1-intro-fade");
        yield return new WaitForSeconds(0.2f);
        sl2.SetActive(true);
        sl2.GetComponent<Animator>().Play("s2-intro-fade");
        yield return new WaitForSeconds(0.2f);
        sl3.SetActive(true);
        sl3.GetComponent<Animator>().Play("s3-intro-fade");
        options.GetComponent<optionsMenu>().AnimationDone();
        inOptions = false;
    }

    IEnumerator passiveExitSlot()
    {
        options.GetComponent<optionsMenu>().nowInAnimation();
        GetComponent<datacontrol>().trashBinClose();
        trashbin_closed.SetActive(true);
        trashbin_open.SetActive(false);
        slttl.GetComponent<Animator>().Play("slttl-outro-fade");
        backbtn.GetComponent<Animator>().Play("slotBack-outro-fade");
        yield return new WaitForSeconds(0.2f);
        sl1.GetComponent<Animator>().Play("s1-outro-fade");
        yield return new WaitForSeconds(0.2f);
        sl2.GetComponent<Animator>().Play("s2-outro-fade");
        yield return new WaitForSeconds(0.2f);
        sl3.GetComponent<Animator>().Play("s3-outro-fade");
        yield return new WaitForSeconds(1);
        trashbin_closed.SetActive(false);
        slttl.SetActive(false);
        backbtn.SetActive(false);
        sl1.SetActive(false);
        sl2.SetActive(false);
        sl3.SetActive(false);
        options.GetComponent<optionsMenu>().AnimationDone();
        if (!creatingSlots)
        {
            canvasin();
        }
    }

}
