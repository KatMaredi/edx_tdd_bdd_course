using System.Collections;
using NUnit.Framework;

namespace writing_test_assertions.tests;

[TestFixture]
public class StackTestCases
{
    private Stack _stack;

    [SetUp]
    public void Setup()
    {
        _stack = new Stack();
    }

    [TearDown]
    public void TearDown()
    {
        _stack = null;
    }

    [Test]
    public void TestPushingItemToTheStack()
    {
        throw new Exception("Not Implemented");
    }

    [Test]
    public void TestPoppingAnItemOffTheStack()
    {
        throw new Exception("Not Implemented");
    }

    [Test]
    public void TestPeekingAtTheTopOfTheStack()
    {
        _stack.Push(6);
        _stack.Push(7);
        Assert.That(7,Is.EqualTo(_stack.Peek()));
    }

    [Test]
    public void TestIfTheStackIsEmpty()
    {
        Assert.That(_stack.IsEmpty,Is.True);
        _stack.Push(5);
        Assert.That(_stack.IsEmpty,Is.False);
    }
}