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

    /// <summary>
    /// Gets a message object of the type passed and adds a user count
    /// 
    /// If you use it imediately and don't store a reference use Messages.GetNoCount
    /// Ex: Messages.GetNoCount<SType>().Dispatch(); <- No reference is stored.
    /// 
    /// This gets it from a global hub, if you need to differentiate it
    /// consider using GetHub("SomeIdentifier").Get<SType>() instead
    /// 
    /// Make sure to return the reference so memory can be freed if no references exist using:
    /// Messages.Return<SType>();
    /// </summary>
    /// <typeparam name="SType"></typeparam>
    public static SType Get<SType>() where SType : IMessage, new()
    {
        return globalHub.Get<SType>();
    }
    /// <summary>
    /// Gets a message object of the type passed DOES NOT add a reference count
    /// 
    /// If you are holding onto a reference use Messages.Get to ensure the memory it isn't freed
    /// Ex: Messages.Get<SType>(); <- Reference is counted.
    /// 
    /// This gets it from a global hub, if you need to differentiate it
    /// consider using GetHub("SomeIdentifier").Get<SType>() instead
    /// </summary>
    /// <typeparam name="SType"></typeparam>
    /// <returns></returns>
    public static SType GetNoCount<SType>() where SType : IMessage, new()
    {
        return globalHub.Get<SType>();
    }
    /// <summary>
    /// Returns a copy of the message, by returning it we can 
    /// manage the memory of used messages, if there's no,
    /// users actively holding a reference. 
    /// </summary>
    /// <typeparam name="SType"></typeparam>
    public static void Return<SType>() where SType : IMessage, new()
    {
        globalHub.Return<SType>();
    }
    /// <summary>
    /// get's a message without changing user count
    /// used for intentionally only getting it to dispatch once
    /// </summary>
    /// <typeparam name="SType">The Type of the message</typeparam>
    /// <returns>The message object</returns>
    public static SType GetOnce<SType>() where SType : IMessage, new()
    {
        return globalHub.GetOnce<SType>();
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

    public SType GetNoCount<SType>() where SType : IMessage, new()
    {
        Type messageType = typeof(SType);
        IMessage message;

        if (messages.TryGetValue(messageType, out message))
        {
            return (SType)message;
        }
        var newMessage = (SType)Bind(messageType);
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
    /// <summary>
    /// get's a message without changing user count
    /// used for intentionally only getting it to dispatch once
    /// </summary>
    /// <typeparam name="SType">The Type of the message</typeparam>
    /// <returns>The message object</returns>
    public SType GetOnce<SType>() where SType : IMessage, new()
    {
        Type messageType = typeof(SType);
        IMessage message;

        if (messages.TryGetValue(messageType, out message))
        {
            return (SType)message;
        }
        var newMessage = (SType)Bind(messageType);
        return newMessage;
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