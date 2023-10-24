using System;
using System.Collections.Generic;

public interface IMessage { }

public class Messages
{
    private static Dictionary<string, MessageHub> messageHubs = new Dictionary<string, MessageHub>();
    private static readonly MessageHub globalHub = new MessageHub();

    public static SType Get<SType>() where SType : IMessage, new()
    {
        return globalHub.Get<SType>();
    }
    public static MessageHub GetHub(string hubName)
    {
        if (messageHubs.ContainsKey(hubName))
        {
            Debug.Log($"Using Hub {hubName}");
            return messageHubs[hubName];
        }
        Debug.Log($"Creating Hub {hubName}");
        var newHub = new MessageHub();
        messageHubs.Add(hubName, newHub);
        return newHub;
    }
}

public class MessageHub
{
    private Dictionary<Type, IMessage> messages = new Dictionary<Type, IMessage>();

    public SType Get<SType>() where SType : IMessage, new()
    {
        Type messageType = typeof(SType);
        IMessage message;

        if(messages.TryGetValue(messageType, out message))
        {
            return (SType)message;
        }
        return (SType)Bind(messageType);
    }

    public IMessage Bind(Type messageType)
    {
        IMessage message;

        if (messages.TryGetValue(messageType, out message))
        {
            return message;
        }

        message = (IMessage)Activator.CreateInstance(messageType);
        messages.Add(messageType, message);
        return message;
    }
}

public abstract class AMessage : IMessage
{
    private Action callback;

    public void AddListener(Action handler)
    {
        callback += handler;
    }

    public void RemnoveListener(Action handler)
    {
        callback -= handler;
    }

    public void Dispatch()
    {
        callback?.Invoke();
    }
}
public abstract class AMessage<T> : IMessage
{
    private Action<T> callback;

    public void AddListener(Action<T> handler)
    {
        callback += handler;
    }

    public void RemoveListener(Action<T> handler)
    {
        callback -= handler;
    }

    public void Dispatch(T arg1)
    {
        callback?.Invoke(arg1);
    }
}
public abstract class AMessage<T, U> : IMessage
{
    private Action<T, U> callback;

    public void AddListener(Action<T, U> handler)
    {
        callback += handler;
    }

    public void RemoveListener(Action<T, U> handler)
    {
        callback -= handler;
    }

    public void Dispatch(T arg1, U arg2)
    {
        callback?.Invoke(arg1, arg2);
    }
}
public abstract class AMessage<T, U, V> : IMessage
{
    private Action<T, U, V> callback;

    public void AddListener(Action<T, U, V> handler)
    {
        callback += handler;
    }

    public void RemoveListener(Action<T, U, V> handler)
    {
        callback -= handler;
    }

    public void Dispatch(T arg1, U arg2, V arg3)
    {
        callback?.Invoke(arg1, arg2, arg3);
    }
}