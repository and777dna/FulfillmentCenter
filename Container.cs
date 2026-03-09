namespace FulfillmentCenter;

public class Container
{
    private Dictionary<Type, Type> _registeredTypes;

    public Container()
    {
        _registeredTypes = new Dictionary<Type, Type>();
    }

    public void Register<IInterface, TImplementation>()
    {
        _registeredTypes[typeof(IInterface)] = typeof(TImplementation);
    }
    
    public TInterface Resolve<TInterface>()
    {
        var type = typeof(TInterface);
        
        var implementationType = _registeredTypes[type];
        return (TInterface)Activator.CreateInstance(implementationType);
    }
}