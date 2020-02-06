using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FakeItEasy;
using FakeItEasy.Core;

namespace UsersService.Tests
{
    static class TestUtilities
    {
        public static bool WasCalled<T>(Expression<Func<T>> method)
        {
            return GetMethodCall(method) != null;
        }

        public static bool WasCalled(Expression<Action> method)
        {
            return GetMethodCall(method) != null;
        }

        public static ICompletedFakeObjectCall GetMethodCall(Expression<Action> method)
        {
            var methodCall = method.Body as MethodCallExpression;
            if (methodCall != null)
            {
                string methodName = methodCall.Method.Name;
                
                var fakeInstance = GetFakeInstance((MemberExpression) methodCall.Object);
                
                return Fake.GetCalls(fakeInstance).FirstOrDefault(call => call.Method.Name.Equals(methodName));
            }

            throw new InvalidOperationException("Argument is not a method");
        }

        public static ICompletedFakeObjectCall GetMethodCall<T>(Expression<Func<T>> method)
        {
            var methodCall = method.Body as MethodCallExpression;
            if (methodCall != null)
            {
                string methodName = methodCall.Method.Name;
                
                var fakeInstance = GetFakeInstance((MemberExpression) methodCall.Object);
                
                return Fake.GetCalls(fakeInstance).FirstOrDefault(call => call.Method.Name.Equals(methodName));
            }

            throw new InvalidOperationException("Argument is not a method");
        }

        public static T Get<T>(this ICompletedFakeObjectCall call, String argumentName)
        {
            return call.Arguments.Get<T>(argumentName);
        }

        private static object GetFakeInstance(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }
    }
}
