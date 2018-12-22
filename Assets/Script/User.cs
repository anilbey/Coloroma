using UnityEngine;
using System.Collections;

public class User
{
    private string nick;
    public string Nick
    {
        get { return nick; }
        set { nick = value; }
    }

    private long id;
    public long Id
    {
        get { return id; }
        set { id = value; }
    }

    private bool isAlive;
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    private string _pos;
    public string Position
    {
        get { return _pos; }
        set { _pos = value; }
    }

    private string _rot;
    public string Rotation
    {
        get { return _rot; }
        set { _rot = value; }
    }

    private GameObject _gameObject;
    public GameObject gameObject
    {
        get { return _gameObject; }
        set { _gameObject = value; }
    }

    private int _layer;
    public int Layer
    {
        get { return _layer; }
        set { _layer = value; }
    }

    public User(long id, string nick, int layer)
    {
        this.id = id;
        this.nick = nick;
        this._layer = layer;
        this.dmg = new Projectile();
    }

    private Projectile dmg;
    public Projectile FireProjectile
    {
        get { return dmg; }
    }

    public class Projectile
    {
		private float damage;
		public float Damage
		{
			get { return damage; }
			set { damage = value; }
		}

        private Vector3 startingPoint;
        public Vector3 StartPoint
        {
            get { return startingPoint; }
        }

        private bool fired;
        public bool Fired
        {
            get { return fired; }
            set { fired = value; }
        }

        private Vector3 fireDirection;
        public Vector3 Direction
        {
            get { return fireDirection; }
        }

        public void SetStartingPoint(string str)
        {            
            string[] strP = str.Split(',');
            startingPoint.x = float.Parse(strP[0]);
            startingPoint.y = float.Parse(strP[1]);
            startingPoint.z = float.Parse(strP[2]);
        }

        public void SetDirection(string str)
        {
            string[] strP = str.Split(',');
//            fireDirection = Quaternion.EulerAngles(float.Parse(strP[0]), float.Parse(strP[1]), float.Parse(strP[2]));
			fireDirection = new Vector3(float.Parse(strP[0]), float.Parse(strP[1]), float.Parse(strP[2]));
        }

        public Projectile()
        {
            startingPoint = Vector3.zero;
            fireDirection = Vector3.zero;
			damage = 0;
            fired = false;
        }
    }
}
