using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerControlGate : MonoBehaviour
    {
        private readonly HashSet<object> locks = new();

        public bool IsLocked => locks.Count > 0;

        public object Acquire()
        {
            object token = new object();
            locks.Add(token);
            return token;
        }

        public void Release(object token)
        {
            if (token == null)
                return;

            locks.Remove(token);
        }
    }
}