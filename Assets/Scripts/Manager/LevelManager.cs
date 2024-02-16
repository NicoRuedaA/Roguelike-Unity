using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class LevelManager : MonoBehaviour
    {
    private static LevelManager _instance;
    public static LevelManager instance{
        get{
            if(_instance == null){
                Debug.Log("Level Manager is Null!!!");
            }
            return _instance;
        }
    }


    //public GameObject roomsGenerated;
    int numEnemies, roomsCompleted, bridgeSelected;
    public int NumEnemies { get { return numEnemies; }}
    GameObject actualRoom, _player;
    public GameObject ActualRoom => actualRoom;
    [SerializeField] List<GameObject> roomsArray= new List<GameObject>();
    public List<GameObject> doors = new List<GameObject>();
    
    private int a, b, c, d;
    
    public int A => a;
    public int B => b;
    public int C => c;
    public int D => d;


    [SerializeField] GameObject BossDoor;  
    
    
    private void Awake(){
        _instance = this;
    }
    
    private void Start() {
        actualRoom = GameObject.Find("LobbyBeta");
        numEnemies = 0; roomsCompleted = 0;
    }

public void CreateRoom(GameObject lastRoomCreated){
        actualRoom = lastRoomCreated;
        roomsArray.Add(lastRoomCreated);
    }
    
    /*public void enemiesAlive(){

        if(numEnemies<=0)
        {
            //ESTO PUEDE DAR BUGS
            numEnemies = 0;
            AbrirPuertas();
        }
    }*/


    public void AddEnemy(){
        numEnemies++;
    }

    public void SubstractEnemy(){
        numEnemies--;
        //enemigo_muere.Play();
        //enemiesAlive();
    }
    
    public void GenerateRoom()
    {
        int min = 0, max = 2;
        this.a=Random.Range(min, max);
        this.b=Random.Range(min, max);
        this.c=Random.Range(min, max);
        this.d=Random.Range(min, max);
    }
    

    public void InsertDoor(GameObject x)
    {
        doors.Add(x);
    }
    
    public void DeleteDoors()
    {

        if(roomsCompleted == 0) doors.RemoveAt(0);
        else doors.RemoveAt(3);

        roomsCompleted++;
        Debug.Log(roomsCompleted);
        CloseDoors();
        doors.Clear();
        if(roomsCompleted>1) {
            OpenBossDoor();
            Debug.Log(" SE ABRIO LA PUERRRRRRRRRRRRRRRRRRRTA ");
        }
    }
    
    

    //entramos en el room
    public void CloseDoors()
    {
        foreach(GameObject objet in doors)
        {
            objet.SetActive(true);
        }
    }
    
    //matamos a todos
    public void OpenDoors()
    {               
        foreach(GameObject objet in doors)
        {

            objet.SetActive(false);
        }
    }

    void OpenBossDoor(){
        BossDoor.SetActive(false);
    }

	}


