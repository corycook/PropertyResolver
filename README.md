# PropertyResolver
Sometimes Dependency Injection just isn't good enough.

Let's explore some current methods for Dependency Injection (by hand):

```C#
class MyClass
{
  IDependency _obj;

  public MyClass(IDependency object)
  {
    _obj = object
  }
}
```

MyClass requires some IDependency object. By using an interface or abstract type and passing the concrete class in to the constructor we avoid having to reference any particular instance of IDependency in MyClass. We effectively give control of the dependency to the calling object (inversion of control).

```C#
class MyClass
{
  public IDependency Obj { get; set; }
}
```

In this instance we can set the IDependency from outside of the class using the setter of the dependent property (setter injection).
