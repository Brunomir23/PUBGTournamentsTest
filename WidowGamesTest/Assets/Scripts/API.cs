using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;

public class API : MonoBehaviour
{
    private const string URL = "https://api.pubg.com/tournaments";    
    private const string api_key = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiI5MzRiMzVkMC1iMzYxLTAxMzgtYmM2MS0wOTdjZTlhNDY4NjMiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNTk1OTgzMDM2LCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6ImJmZXJyYXJpMjMtZ21hIn0.qaJpDmudzNhUk1bhtlpH_DI_KjEl8ZiL7mk0JKZENGc";
    public Text countPageText, actualPageText;
    public string json;
    private List<Lista> ListaTorneos;

    public Transform listaContainer;
    public Transform listaTemplate;
    public int pag, pagCount;
  
    private void Start()
    {
        Request();
        pag = 0;
    }
    public void Request()
    {
        WWWForm form = new WWWForm();

        Dictionary<string, string> headers = form.headers;
        headers["accept"] = "application/vnd.api+json";
        headers["Authorization"] = "Bearer " + api_key;

        WWW request = new WWW(URL,null, headers);
        StartCoroutine(OnResponse(request));
    }

    private IEnumerator OnResponse(WWW req)
    {
        yield return req;              
        json = req.text;
        File.WriteAllText(Application.dataPath + "listaJson.json", json);

        json = File.ReadAllText(Application.dataPath + "listaJson.json");        
       
        JSONNode data = JSON.Parse(json);

        pagCount = (data["data"].Count / 10)+1;
        actualPageText.text = (pag + 1).ToString();
        countPageText.text = (pagCount).ToString();
        
        
        JSONArray list = data["data"].AsArray;
        ListaTorneos = new List<Lista>();
        foreach (JSONNode record in data["data"])
        {
            Lista lista = new Lista();
            lista.id = record["id"].Value;
            lista.createdAt = record["attributes"]["createdAt"];
            ListaTorneos.Add(lista);          
           
                    
        }
       
        getLista();
    }
    public void getLista()
    {
        int count = 0;
        int i = 0;
        destroyAllText();
        

        foreach (Lista item in ListaTorneos)
        {
            if (count>= pag * 10 && count < (pag * 10) + 10)
            {
                if (i < 10)
                {
                    Transform listaTransform = Instantiate(listaTemplate, listaContainer);
                    listaTransform.tag = "ListaContainer";
                    RectTransform listaRectTransform = listaTransform.GetComponent<RectTransform>();
                    listaRectTransform.anchoredPosition = new Vector2(0, -60f * i);
                    listaTransform.Find("IdTorneo").GetComponent<Text>().text = item.id;
                    listaTransform.Find("FechaCreacion").GetComponent<Text>().text = item.createdAt;
                    
                    i++;
                }
                //Debug.Log("count: " + count + "item: " + item.id);
            }
            count++;

        }
    }
    public void pageNext()
    {
        if (pag < pagCount-1)
        {
            pag++;
            actualPageText.text = (pag + 1).ToString();
            getLista();
        }
    }
    public void pagePrev()
    {
        if (pag > 0)
        {
            pag--;
            actualPageText.text = (pag + 1).ToString();
            getLista();
        }
    }
    public void destroyAllText()
    {
        GameObject[] texts;

        texts = GameObject.FindGameObjectsWithTag("ListaContainer");
        
        foreach (GameObject text in texts)
        {
            Destroy(text);
        }

    }
}

[System.Serializable]
public class Lista
{
    public string id, createdAt;    
}
