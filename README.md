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

In this instance we can set the IDependency from outside of the class using the setter of the dependent property (setter injection). This method of dependency injection is a bit counter intuitive since it is not clear that the dependency needs to be set before the functionality is available. However, this method may be necessary if you need to have a parameterless constructor (i.e. custom XAML controls).

```C#
class MyClass
{
  public void SomeMethod(IDependency obj)
  {
    // Do something with obj
  }
}
```

Method injection introduces the dependency at the method level and only passes it when it is used. This method is more cumbersome; however, it is also more robust and allows the dependency to be changed between calls, multiple dependency instances, or dependencies to be shared across multiple instances of MyClass.

# The Problem

Dependency injection falls short when you want to access properties and methods of similar types that do not derive from the same base class.

```C#
interface IDependency
{
  object Property { get; set; }
}

class DependencyWrapper : IDependency
{
  readonly Dependency _dependency;
  
  public object Property { get { return _dependency.getProperty(); } set { _dependency.setProperty(value); } }
  
  public DependencyWrapper(Dependency dependency)
  {
    _dependency = dependency;
  }
}
```

Using a wrapper class we can implement the properties necessary to conform our target class (Dependency) to the interface that our class (MyClass) is expecting (IDependency).

