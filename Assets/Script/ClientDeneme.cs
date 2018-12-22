using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientDeneme : MonoBehaviour
{
	private User me;
	private Dictionary<long, User> users;
	private Dictionary<long, User> notInitiated;
	private Dictionary<long, User> usersToBeDeleted;
	public GameObject PlayerPrefab;
	
	// Use this for initialization
	void Start()
	{
		users = new Dictionary<long, User>();
		notInitiated = new Dictionary<long, User>();
		usersToBeDeleted = new Dictionary<long, User> ();        
	}
	
	// Update is called once per frame
	void Update()
	{
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
		// pass
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