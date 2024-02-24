using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public interface IMessage
{
    void AddUser();
    void RemoveUser();
    int GetUserCount();
}

public class Messages
{
    private static Dictionary<string, MessageHub> messageHubs = new Dictionary<string, MessageHub>();
    private static readonly MessageHub globalHub = new MessageHub();

    public static SType Get<SType>() where SType : IMessage, new()
    {
        return globalHub.Get<SType>();
    }
    public static void Return<SType>() where SType : IMessage, new()
    {
        globalHub.Return<SType>();
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
    public static void ReturnHub(string hubName)
    {
        if (messageHubs.ContainsKey(hubName))
        {
            messageHubs[hubName].RemoveUser();
            if (messageHubs[hubName].UserCount == 0)
            {
                messageHubs.Remove(hubName);
            }
        }
    }
}

public class MessageHub
{
    public int UserCount { get; private set; }
    private Dictionary<Type, IMessage> messages = new Dictionary<Type, IMessage>();

    public SType Get<SType>() where SType : IMessage, new()
    {
        Type messageType = typeof(SType);
        IMessage message;

        if(messages.TryGetValue(messageType, out message))
        {
            message.AddUser();
            return (SType)message;
        }
        var newMessage = (SType)Bind(messageType);
        newMessage.AddUser();
        return newMessage;
    }
    public void Return<SType>() where SType : IMessage, new()
    {
        Type messageType = typeof(SType);
        IMessage message;

        if (messages.TryGetValue(messageType, out message))
        {
            message.RemoveUser();
            if (message.GetUserCount() == 0)
            {
                messages.Remove(messageType);
            }
        }
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
    public void AddUser()
    {
        ++UserCount;
    }
    public int GetUserCount()
    {
        return UserCount;
    }
    public void RemoveUser()
    {
        --UserCount;
    }
}
public abstract class ABaseSignal : IMessage
{
    public int UserCount { get; private set; }
    public void AddUser()
    {
        ++UserCount;
    }
    public int GetUserCount()
    {
        return UserCount;
    }
    public void RemoveUser()
    {
        --UserCount;
    }
}

public abstract class AMessage : ABaseSignal
{
    private Action callback;

    public void AddListener(Action handler)
    {
        callback += handler;
    }

    public void RemoveListener(Action handler)
    {
        callback -= handler;
    }

    public void Dispatch()
    {
        callback?.Invoke();
    }
}
public abstract class AMessage<T> : ABaseSignal
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
public abstract class AMessage<T, U> : ABaseSignal
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
public abstract class AMessage<T, U, V> : ABaseSignal
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