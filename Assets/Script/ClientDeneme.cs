using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TCPHandler.ClientSide;

public class ClientDeneme : MonoBehaviour
{
	private Client client;
	private User me;
	private Dictionary<long, User> users;
	private Dictionary<long, User> notInitiated;
	private Dictionary<long, User> usersToBeDeleted;
	public GameObject PlayerPrefab;
	
	// Use this for initialization
	void Start()
	{
		client = new Client("192.168.1.6", 1234);
		if (client.Connect())
		{
			print("Connected");
			client.SendMessage("0&mindfog" + this.gameObject.name);
		}
		else
			print("FAIL");
		
		client.NewAction += client_NewAction;
		
		users = new Dictionary<long, User>();
		notInitiated = new Dictionary<long, User>();
		usersToBeDeleted = new Dictionary<long, User> ();        
	}
	
	// Update is called once per frame
	void Update()
	{
		SendPositionRotation();  
		if (notInitiated.Count > 0)
		{
			List<long> keyToDel = new List<long>();
			foreach (KeyValuePair<long, User> pair in notInitiated)
			{
				pair.Value.gameObject = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				keyToDel.Add(pair.Key);
				users.Add(pair.Key, pair.Value);                
			}
			
			foreach (long i in keyToDel)
			{
				notInitiated.Remove(i);
			}
		}
		
		if (usersToBeDeleted.Count > 0)
		{
			List<long> keyToDel = new List<long>();
			foreach (KeyValuePair<long, User> pair in usersToBeDeleted)
			{
				Destroy(pair.Value.gameObject);
				keyToDel.Add(pair.Key);
				users.Remove(pair.Key);
			}
			
			foreach (long i in keyToDel)
			{
				usersToBeDeleted.Remove(i);
			}
		}
		
		lock (users)
		{
			foreach (KeyValuePair<long, User> u in users)
			{
				try
				{
					string[] p = u.Value.Position.Split(',');
					string[] r = u.Value.Rotation.Split(',');          
					// TODO: SET ANIMATION 
					u.Value.gameObject.transform.position = new Vector3(float.Parse(p[0]), float.Parse(p[1]), float.Parse(p[2]));
					u.Value.gameObject.transform.rotation = Quaternion.Euler(float.Parse(r[0]), float.Parse(r[1]), float.Parse(r[2]));                    
					
					// FIRE
					if (u.Value.FireProjectile.Fired)
					{
						FireOther(u.Value);
						u.Value.FireProjectile.Fired = false;
					}
				}
				catch
				{                    
				}
				print ("enemy layer= " + u.Value.Layer + "your layer =" + ColorBars.circularDummy);
				if (u.Value.Layer != ColorBars.circularDummy)
				{	
					u.Value.gameObject.GetComponent<ShootControl>().enabled = false;
					u.Value.gameObject.SetActive(false);

				}
				else 
				{
					u.Value.gameObject.GetComponent<ShootControl>().enabled = true;
					u.Value.gameObject.SetActive(true);

				}
			}
		}
	}
	
	void OnApplicationQuit()
	{
		this.client.Disconnect();
	}
	
	void client_NewAction(TCPHandler.ClientEvents e)
	{
		string[] parameters = DecodeMessage(e.Message);
		
		if (parameters == null)
		{            
			return;
		}
		
		switch (parameters[0])
		{
		case "0":
			// New user login                
			NewUserLogin(long.Parse(parameters[1]), parameters[2]);
			break;
		case "1":
			
			break;
		case "2":
			//print(parameters[2]);
			SetPosRot(long.Parse(parameters[1]), parameters[2], parameters[3], parameters[4], parameters[5]);
			break;
		case "3":
			// Set layer
			SetLayer(long.Parse(parameters[1]), int.Parse(parameters[2]));
			break;
		case "4":
			// Fire
			PlayerFired(long.Parse(parameters[1]), parameters[2], parameters[3], parameters[4]);
			break;
		case "5":
			// This player got shot
			// parameters[1] = Vuran kisinin idsi
			// parameters[2] = dmg
			GotShot(long.Parse(parameters[1]), float.Parse(parameters[2]));
			break;
		case "99":
			// Someone left
			UserLeft(long.Parse(parameters[1]));
			break;
		}
	}
	
	private void GotShot(long id, float dmg)
	{
		if(ColorBars.circularDummy == 0)
		{
			ColorBars.redAmount -= dmg;
		}
		else if(ColorBars.circularDummy == 1)
		{
			ColorBars.greenAmount -= dmg;
		}
		else
		{
			ColorBars.blueAmount -= dmg;
		}
	}
	
	public void PlayerHit(GameObject go, float dmg)
	{
		long id = -1;
		lock (users)
		{
			foreach (KeyValuePair<long, User> u in users)
			{
				if (u.Value.gameObject == go)
					id = u.Value.Id;
				print (u.Value.Layer);
			}
		}
		
		if (id != -1)
			client.SendMessage("5&" + id.ToString() + "&" + dmg.ToString());
	}
	
	public void Fire(Vector3 startPoint, Vector3 direction, float damage)
	{
		client.SendMessage("4&" + startPoint.x.ToString() + "," + startPoint.y.ToString() + "," + startPoint.z.ToString() + "&" +
		                   direction.x.ToString() + "," + direction.y.ToString() + "," + direction.z.ToString() + "&" + damage.ToString());
	}
	
	private void FireOther(User u)
	{
		u.gameObject.GetComponent<ShootControl> ().ShootPlayer (u.FireProjectile.StartPoint, u.FireProjectile.Direction, u.FireProjectile.Damage);
	}
	
	private void NewUserLogin(long id, string nick)
	{
		User user = new User(id, nick, 0);
		notInitiated.Add(id, user);
	}
	
	private void UserLeft(long id)
	{
		lock (users)
		{
			usersToBeDeleted.Add(id, users[id]);
			//users.Remove(id);
		}
	}
	
	private void PlayerFired(long id, string start, string direction, string damage)
	{
		lock (users)
		{
			users[id].FireProjectile.SetStartingPoint(start);
			users[id].FireProjectile.SetDirection(direction);
			users[id].FireProjectile.Damage = float.Parse(damage);
			users[id].FireProjectile.Fired = true;
		}
	}
	
	private void SendPositionRotation()
	{
		//print("send " + this.gameObject.name);
		string strPos = this.transform.position.x.ToString() + ',' + this.transform.position.y.ToString() + ',' + this.transform.position.z.ToString();
		string strRot = this.transform.rotation.eulerAngles.x.ToString() + ',' + this.transform.rotation.eulerAngles.y.ToString() + ',' + this.transform.rotation.eulerAngles.z.ToString();
		string strCam = "0,0,0";
		string anim = "0";
		// ANIMATION?
		client.SendMessage("2&" + strPos + "&" + strRot + "&" + strCam + "&" + anim);
	}
	
	private void SetPosRot(long id, string strPos, string strRot, string strCam, string anim)
	{
		string[] c = strCam.Split(',');
		
		lock (users)
		{
			if (users.ContainsKey(id))
			{           
				users[id].Position = strPos;
				users[id].Rotation = strRot;
			}
		}
	}
	
	private void SetLayer(long id, int layer)
	{
		print (layer);
		lock (users) 
		{
			users[id].Layer = layer;
		}
	}

	public void SendLayer()
	{
		client.SendMessage ("3&" + ColorBars.circularDummy.ToString());
	}
	
	private string[] DecodeMessage(string msg)
	{
		try
		{
			if (msg.Contains("&"))
				return msg.Split('&');
			else
			return new string[1] { msg };
		}
		catch
		{
			return null;
		}
	}
}