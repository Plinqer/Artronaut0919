using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public TMP_InputField WishInput;
    //public TextMeshProUGUI wishTxt;
    public string userID;
    private DatabaseReference dbReference;
    [SerializeField] Gamemng Gamemng;
    public GameObject[] Otherstmplist;
    public string Mywish;

    int i;
    // Start is called before the first frame update
    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Update()
    {
        Otherstmplist = GameObject.FindGameObjectsWithTag("WishesOfOthers");

    }

    //입력한 소원 db에 저장
    public void CreatWishes()
    {
        User newUser = new User(WishInput.text);
        string json = JsonUtility.ToJson(newUser);
        Mywish = WishInput.text;
        i = Random.Range(1, 70);
        dbReference.Child("User" + i).SetRawJsonValueAsync(json);
    }


    //db에서 입력된 소원 불러와 다른 행성들에 로드
    public IEnumerator LoadWishes()
    {
        Otherstmplist = GameObject.FindGameObjectsWithTag("WishesOfOthers");

        for (int d = 0; d < Otherstmplist.Length; d++)
        {
            i = Random.Range(1, 70);
            var wishData = dbReference.Child("User" + i).Child("Wish").GetValueAsync();
            yield return new WaitUntil(predicate: () => wishData.IsCompleted);
            if (wishData.Exception != null)
            {
                Debug.Log("Fail to fetch");
            }

            else if (wishData.Result.Value == null)
            {
                Debug.Log("No data yet");
            }

            else
            {
                DataSnapshot snapshot = wishData.Result;
                Debug.Log("user " + i + " value:  " + snapshot.Value.ToString());
                Otherstmplist[d].GetComponent<TextMeshPro>().text = snapshot.Value.ToString();
            }
            Gamemng.Instantiated = false;
        }

        yield return new WaitForSeconds(2.5f);

    }
    public void GetWishes()
    {
        StartCoroutine(LoadWishes());
    }

}
