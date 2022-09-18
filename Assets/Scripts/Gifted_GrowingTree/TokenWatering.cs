using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TokenWatering : MonoBehaviour
{
    public GameManager_GrowingTree gamemanager;
    [SerializeField] GameObject Sprout;
    [SerializeField] GameObject Seed;
    [SerializeField] GameObject Trunk;
    [SerializeField] GameObject SmallTree;
    [SerializeField] GameObject BigTree;
    // Start is called before the first frame update
    void Start()
    {
        gamemanager = FindObjectOfType<GameManager_GrowingTree>();

        this.transform.DORotate(Vector3.right, 2f).SetEase(Ease.InOutQuad).SetLoops(-1,LoopType.Yoyo);
        this.GetComponent<MeshRenderer>().material.DOFade(0, 5).SetDelay(3f);
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "emptybox")
        {
            Seed.SetActive(true);
            Seed.transform.Find("seed").GetComponent<MeshRenderer>().material.DOFade(1, 4f).SetDelay(3f);
            Destroy(other);
        }

        else if (gamemanager.stage == GameManager_GrowingTree.Stage.one && other.gameObject.name == "emptybox1")
        {
            Seed.transform.Find("seed").GetComponent<MeshRenderer>().material.DOFade(0, 3f);
            Sprout.SetActive(true);
            gamemanager.stage = GameManager_GrowingTree.Stage.two;
            Debug.Log(other.gameObject.name);
            this.GetComponent<MeshRenderer>().material.DOFade(0, 3f);
        }

        else if(other.gameObject.name == "emptybox2")
        {
            Sprout.SetActive(false);
            Trunk.SetActive(true);
            gamemanager.stage = GameManager_GrowingTree.Stage.three;
            Debug.Log(gamemanager.stage);
            this.GetComponent<MeshRenderer>().material.DOFade(0, 3f);
        }

        else if(other.gameObject.name == "emptybox3")
        {
            Trunk.SetActive(false);
            SmallTree.SetActive(true);
            gamemanager.stage = GameManager_GrowingTree.Stage.four;
            Debug.Log(gamemanager.stage);
            this.GetComponent<MeshRenderer>().material.DOFade(0, 3f);
        }

        else if(other.gameObject.name == "emptybox4")
        {
            SmallTree.SetActive(false);
            BigTree.SetActive(true);
            gamemanager.stage = GameManager_GrowingTree.Stage.five;
            Debug.Log(gamemanager.stage);
            this.GetComponent<MeshRenderer>().material.DOFade(0, 3f);
        }
    }

}
