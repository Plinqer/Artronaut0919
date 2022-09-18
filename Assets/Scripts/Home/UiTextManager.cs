using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UiTextManager : MonoBehaviour
{
    [SerializeField] GameObject Instruction1;
    [SerializeField] GameObject Instruction2;
    [SerializeField] GameObject Inst;
    [SerializeField] GameObject btnNext;
    [SerializeField] GameObject btnPrevious;
    private int page = 0;
    [SerializeField] Transform PlazaPanel;
    [SerializeField] int yPosmove;
    bool introFinish;
    [SerializeField] Text txt1;
    [SerializeField] GameObject txt1obj;
    [SerializeField] Text txt2;
    [SerializeField] GameObject txt2obj;
    [SerializeField] GameObject txt3_wideobj;
    [SerializeField] Text txt3_wide;
    [SerializeField] GameObject txt4_midobj;
    [SerializeField] Text txt4_mid;
    [SerializeField] GameObject PreviousBtn;
    [SerializeField] Image ProgressBar;
    [SerializeField] Sprite Progress2;
    [SerializeField] Sprite Progress3;
    [SerializeField] Sprite Progress4;
    [SerializeField] Sprite Progress5;
    [SerializeField] Camera cam;
    [SerializeField] GameObject Home;
    [SerializeField] GameObject LoadingPage;
    [SerializeField] GameObject Intro;
    [SerializeField] TokenCheck introFinishsc;
    // Start is called before the first frame update

    private void Start()
    {
        if (introFinishsc._introFinish == false)
        {
            Intro.SetActive(true);
            Home.SetActive(false);
        }
        else if (introFinishsc._introFinish == true)
        {
            Intro.SetActive(false);
            Home.SetActive(true) ;
        }
        txt4_midobj.SetActive(false);
        txt3_wideobj.SetActive(false);
    }
    public void OnNext()
    {
        if (PreviousBtn.activeSelf == false)
        {
            PreviousBtn.SetActive(true);
            ProgressBar.sprite = Progress2;
            txt1.text = "��ü�� �Ҿ����� �� �ֽ��ϴ�.";
            txt2.text = "";
            StartCoroutine(texttimedelay());

            IEnumerator texttimedelay()
            {
                cam.DOShakePosition(4, 2, 10);
                yield return new WaitForSeconds(2);
                txt2.text = "...1";
                yield return new WaitForSeconds(1);
                txt2.text = "...1...2";
                yield return new WaitForSeconds(1);
                txt2.text = "...1...2...3";
                yield return new WaitForSeconds(1);
                txt2.text = "...1...2...3... �����߽��ϴ�.";
            }
        }
        else if(ProgressBar.sprite == Progress2)
        {
            ProgressBar.sprite = Progress3;

            txt1.text = "�긮���ϰڽ��ϴ�.";
            txt2obj.gameObject.SetActive(false);
            txt3_wideobj.gameObject.SetActive(true);
            txt3_wide.text = "Karts���ϰ�� �� ���� �༺��� �̷���� �ֽ��ϴ�. Ž���� ����ϴ� ���� Ŭ���ϸ� �� �Լ��� �׺���̼� ���� ��ȯ�մϴ�.";
        }

        else if(ProgressBar.sprite == Progress3)
        {
            txt1obj.SetActive(false);
            txt4_midobj.SetActive(true);
            txt4_mid.text ="�� Ž�������� Ž���� ������ �� �ִ� ��Ŀ�� ��ġ���ֽ��ϴ�.";
            txt3_wide.text ="���� �Լ��� ���� ��Ŀ�� ��ĵ�ϴ� ����, �ش� Ž���������� Ž���� ���۵˴ϴ�. Ž���� ������ ������ ��ū�� ȹ���մϴ�.";
            ProgressBar.sprite = Progress4;
        }
        else if (ProgressBar.sprite == Progress4)
        {
            ProgressBar.sprite = Progress5;
            txt3_wideobj.SetActive(false);
            txt4_mid.text = "�׷�, Ž���� ���� �غ� �Ǽ̳���?";
            introFinishsc._introFinish = true;
        }
        else if(ProgressBar.sprite = Progress5) 
        {
            LoadingPage.SetActive(true);
            Intro.SetActive(false);
            StartCoroutine(loadingPage());
        }

    }
    IEnumerator loadingPage()
    {
        yield return new WaitForSeconds(6.5f);
        LoadingPage.SetActive(false);
        Home.SetActive(true);
    }


    public void PanelMoveOnNext()
    {
        if(page < 3)
        {
            PlazaPanel.DOMove(new Vector3(PlazaPanel.position.x - yPosmove, PlazaPanel.position.y, PlazaPanel.position.z), 0.2f);
            page ++;
        }
        else
        {
            PlazaPanel.DOMove(new Vector3(PlazaPanel.position.x, PlazaPanel.position.y, PlazaPanel.position.z), 0.2f);
        }
    }
    public void PanelMoveOnBack()
    {
        if(page > 0)
        {
            PlazaPanel.DOMove(new Vector3(PlazaPanel.position.x + yPosmove, PlazaPanel.position.y, PlazaPanel.position.z), 0.2f);
            page--;

        }
        else
        {
            PlazaPanel.DOMove(new Vector3(PlazaPanel.position.x, PlazaPanel.position.y, PlazaPanel.position.z), 0.2f);
        }

    }


    public void Instruction()
    {
        if (Instruction1.activeSelf)
        {
            Instruction2.SetActive(true);
            Instruction1.SetActive(false);

        }
        else if (Instruction2.activeSelf)
        {
            Inst.SetActive(false);
        }
    }

}
