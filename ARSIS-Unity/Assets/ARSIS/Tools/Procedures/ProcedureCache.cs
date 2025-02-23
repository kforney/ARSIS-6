using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class ProcedureCache : MonoBehaviour
{
    public static ProcedureCache Instance { get; private set; }
    private Dictionary<string, ProcedureEvent> procedureCache;
    private WaitForSeconds procedurePollingDelay = new WaitForSeconds(1.0f);
    public int numberOfProcedures = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<ProcedureEvent>(proccessProcedureEvent);
        procedureCache = new Dictionary<string, ProcedureEvent>();
        fetchProcedure("Mock Procedure");
        StartCoroutine(PollProcedureApi());
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int Count(){
        return procedureCache.Count;
    }

    public ProcedureEvent getProcedure(string name){
        return procedureCache.GetValueOrDefault(name, null);
    }

    IEnumerator PollProcedureApi() {
        while(procedureCache.Count < 1){
            StartCoroutine(fetchProcedure("Mock Procedure"));
            yield return procedurePollingDelay;
        }
    }

    //TODO this should select an actual procedure
    IEnumerator fetchProcedure(string procedureName){
        ProcedureGet pg = new ProcedureGet(procedureName);
        EventManager.Trigger(pg);
        yield return null;
    }

    void proccessProcedureEvent(ProcedureEvent pe){
        procedureCache[pe.name] = pe;
        numberOfProcedures = Count();
    }
}
