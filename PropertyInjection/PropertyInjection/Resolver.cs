using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertyInjection
{
	public static class Resolver<T>
	{
		class PKey
		{
			Type Type { get; set; }
			string PropertyName {get;set;}

			public PKey(Type type, string propertyName)
			{
				Type = type;
				PropertyName = propertyName;
			}

			public class PKeyComparer : IEqualityComparer<PKey>
			{
				#region IEqualityComparer implementation
				public bool Equals (PKey x, PKey y)
				{
					return x.Type == y.Type && x.PropertyName == y.PropertyName;
				}
				public int GetHashCode (PKey obj)
				{
					return obj.PropertyName.GetHashCode () + obj.Type.GetHashCode ();
				}
				#endregion
			}
		}
		static readonly Dictionary<PKey, Expression> _getters;

		static Resolver()
		{
			_getters = new Dictionary<PKey, Expression> (new PKey.PKeyComparer ());
		}

		public static void Register<TIn, TOut>(Expression<Func<T, TOut>> property, Expression<Func<TIn, TOut>> getter)
		{
			var name = property.Body as MemberExpression;
			_getters.Add (new PKey (typeof(TIn), name.Member.Name), getter);
		}

		public static TOut Resolve<TIn, TOut>(TIn target, Expression<Func<T, TOut>> property)
		{
			var name = property.Body as MemberExpression;
			var key = new PKey (target.GetType(), name.Member.Name);
			Expression e;
			if (_getters.TryGetValue(key, out e)) {
				var param = Expression.Parameter (typeof(TIn), "x");
				var expression = Expression.Invoke (e, Expression.Convert (param, target.GetType ()));
				var lambda = Expression.Lambda<Func<TIn, TOut>> (expression, param);
				var compiled = lambda.Compile ();
				return compiled.Invoke (target);
			}
			return default(TOut);
		}
	}
}

