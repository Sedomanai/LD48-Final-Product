using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ilang {
    public class EventMgr : Singleton<EventMgr> {
        Dictionary<string, Action> events;

        // Use this for initialization
        new void Awake() {
            base.Awake();
            name = "[Event Manager]";
            events = new Dictionary<string, Action>();
        }

        public void Invoke(string eventName) {
            if (events.ContainsKey(eventName)) {
                if (events[eventName] != null)
                    events[eventName].Invoke();
            }

            else Debug.Log("Event name " + eventName + " does not exist.");
        }

        public void Subscribe(string eventName, Action act) {
            if (events.ContainsKey(eventName)) {
                events[eventName] -= act;
                events[eventName] += act;
            }

            else events[eventName] = new Action(act);
        }

        public void Unsubscribe(string eventName, Action act) {
            if (events.ContainsKey(eventName))
                events[eventName] -= act;
        }

        public void Remove(string eventName) {
            events.Remove(eventName);
        }

        public void Clear() {
            events.Clear();
        }
    }
}
