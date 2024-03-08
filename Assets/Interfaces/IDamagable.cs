using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Interfaces
{
    public  interface IDamagable
    {
        public float Health {  get; set; }

        public bool IsHitable { get; set; }

        public void OnHit(float damage, Vector2 knockDirection);

        public void OnHit(float damage);

        public void KillObject();

    }
}
