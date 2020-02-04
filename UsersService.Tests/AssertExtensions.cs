using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UsersService.Tests
{
    public static class AssertExtensions
    {
        public static void All(this Assert assert, params Action[] assertionsToRun)
        {
            var errorMessages = new List<Exception>();
            foreach (var action in assertionsToRun)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exc)
                {
                    errorMessages.Add(exc);
                }
            }

            if (errorMessages.Any())
            {
                var separator = string.Format("{0}{0}", Environment.NewLine);
                string errorMessageString = string.Join(separator, errorMessages);

                Assert.Fail($"The following conditions failed:{Environment.NewLine}{errorMessageString}");
            }
        }
    }
}