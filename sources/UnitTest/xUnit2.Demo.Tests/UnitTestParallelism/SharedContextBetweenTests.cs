using System;
using System.Collections.Generic;
using Xunit;

namespace xUnit2.Demo.Tests.UnitTestParallelism
{
    //TODO:未看完原文档：https://xunit.github.io/docs/shared-context.html

    public class StackTests : IDisposable
    {
        Stack<int> stack;

        public StackTests()
        {
            stack = new Stack<int>();
        }

        public void Dispose()
        {
            stack.Clear();
            stack = null;
        }

        [Fact]
        public void WithNoItems_CountShouldReturnZero()
        {
            var count = stack.Count;

            Assert.Equal(0, count);
        }

        [Fact]
        public void AfterPushingItem_CountShouldReturnOne()
        {
            stack.Push(42);

            var count = stack.Count;

            Assert.Equal(1, count);
        }
    }
}