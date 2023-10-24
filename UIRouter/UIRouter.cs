using Godot;
using System;
using System.Collections.Generic;


[GlobalClass]
public partial class UIRouter : Resource
{
    Dictionary<string, UIRoute> routes = new Dictionary<string, UIRoute>();
    Stack<string> routesOpened = new Stack<string>();

    Action OnAllRoutesClosed;
    Action<string> OnRouteOpened;
    Action<string> OnRouteClosed;
    [Signal] public delegate void OnRouteOpenedSignalEventHandler(string route);
    [Signal] public delegate void OnRouteClosedSignalEventHandler(string route);
    [Signal] public delegate void OnAllRoutesClosedSignalEventHandler();
    
    public bool IsRouteOpen(string routeSegment)
    {
        ValidateRouter();
        if (routesOpened.Count == 0)
        {
            return false;
        }

        if (routesOpened.Peek() == routeSegment.ToLower())
        {
            return true;
        }

        return false;
    }

    public bool IsRouteInRouteStack(string routeSegment)
    {
        ValidateRouter();
        if (routesOpened.Count == 0)
        {
            return false;
        }

        if (routesOpened.Contains(routeSegment.ToLower()))
        {
            return true;
        }
        return false;
    }

    public void RegisterRoute(string routeSegment, UIRoute reference)
    {
        ValidateRouter();
        GD.Print(routeSegment);
        if (routes.ContainsKey(routeSegment.ToLower()))
        {
            routes[routeSegment.ToLower()] = reference;
        }
        else
        {
            routes.Add(routeSegment.ToLower(), reference);
        }
    }

    public void UnregisterRoute(string routeSegment)
    {
        ValidateRouter();
        if (routes.ContainsKey(routeSegment.ToLower()))
        {
            routes.Remove(routeSegment.ToLower());
        }
    }

    public void OpenRoute(string routeSegment)
    {
        ValidateRouter();
        routeSegment = routeSegment.ToLower();
        if (routes.ContainsKey(routeSegment))
        {
            if (routesOpened.Count > 0)
            {
                var rs = routesOpened.Peek();
                routes[rs].CloseRoute();
                OnRouteClosed?.Invoke(rs);
                EmitSignal(SignalName.OnRouteClosedSignal, rs);
            }
            routesOpened.Push(routeSegment);
            routes[routeSegment].OpenRoute();

            OnRouteOpened?.Invoke(routeSegment);
            EmitSignal(SignalName.OnRouteOpenedSignal, routeSegment);
        }
        else
        {
            Debug.LogError("No Route with segment: " + routeSegment);
        }
    }

    public void OpenRouteExclusive(string routeSegment)
    {
        ValidateRouter();
        if (routes.ContainsKey(routeSegment.ToLower()))
        {
            CloseAll();
            routesOpened.Push(routeSegment.ToLower());
            routes[routeSegment.ToLower()].OpenRoute();
        }
        else
        {
            Debug.LogError("No Route with segment: " + routeSegment.ToLower());
        }
    }

    public void CloseAll()
    {
        ValidateRouter();
        if (routesOpened.Count > 0)
        {
            var rs = routesOpened.Peek();
            routes[rs].CloseRoute();
            OnRouteClosed?.Invoke(rs);
            EmitSignal(SignalName.OnRouteClosedSignal, rs);
        }
        OnAllRoutesClosed?.Invoke();
        EmitSignal(SignalName.OnAllRoutesClosedSignal);
        routesOpened.Clear();
    }

    public void Back()
    {
        ValidateRouter();
        if (routesOpened.Count > 0)
        {
            var rs = routesOpened.Pop();
            if (routes.ContainsKey(rs))
            {
                routes[rs].CloseRoute();
                OnRouteClosed?.Invoke(rs);
                EmitSignal(SignalName.OnRouteClosedSignal, rs);
            }
            if (routesOpened.Count > 0)
            {
                var peekRoute = routesOpened.Peek();
                if (routes.ContainsKey(peekRoute))
                {
                    routes[peekRoute].OpenRoute();
                    OnRouteOpened?.Invoke(peekRoute);
                    EmitSignal(SignalName.OnRouteOpenedSignal, peekRoute);
                }
            }
        }
        if (routesOpened.Count <= 0)
        {
            OnAllRoutesClosed?.Invoke();
            EmitSignal(SignalName.OnAllRoutesClosedSignal);
        }
    }

    public void ValidateRouter()
    {
        if (routes == null)
        {
            routes = new Dictionary<string, UIRoute>();
        }
        if (routesOpened == null)
        {
            routesOpened = new Stack<string>();
        }
    }

    public int OpenRouteCount()
    {
        ValidateRouter();
        return routesOpened.Count;
    }

    public void AddRouteChangedListener(Action<string> routeListener)
    {
        OnRouteOpened += routeListener;
        OnRouteClosed += routeListener;
    }

    public void AddRoutesClosedListener(Action allRoutesClosedCallback)
    {
        OnAllRoutesClosed += allRoutesClosedCallback;
    }

    public void RemoveRouteChangedListener(Action<string> routeListener)
    {
        OnRouteOpened -= routeListener;
        OnRouteClosed -= routeListener;
    }

    public void RemoveRoutesClosedListener(Action allRoutesClosedCallback)
    {
        OnAllRoutesClosed -= allRoutesClosedCallback;
    }

}