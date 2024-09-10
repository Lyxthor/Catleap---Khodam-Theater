using System;
using System.Collections.Generic;

public class EventHandler<T>
{
    public event Action<T> eventHandler;
    public void Subscribe(Action<T> action) => eventHandler += action;
    public void Unsubscribe(Action<T> action) => eventHandler -= action;
    public void Trigger(T arg=default(T)) => eventHandler?.Invoke(arg);
}
namespace Events
{
    public class EventList
    {
        public static EventHandler<bool> OnUpdateStats = new EventHandler<bool>();
        public static EventHandler<int> OnPurchasedStand = new EventHandler<int>();
        public static EventHandler<int> OnCollectStandIncome = new EventHandler<int>();
        public static EventHandler<int> OnUpgradeStand = new EventHandler<int>();
        public static EventHandler<int> OnUpdateStand = new EventHandler<int>();
        public static EventHandler<bool> OnGatherNpc = new EventHandler<bool>();
        public static EventHandler<bool> OnDispatchNpc = new EventHandler<bool>();
        public static EventHandler<bool> OnChangeGameState = new EventHandler<bool>();
    }
}