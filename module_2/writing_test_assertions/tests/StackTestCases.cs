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
        _stack.Push(7);
        _stack.Push(9);
        Assert.That(_stack.Pop(), Is.EqualTo(9));

        Assert.That(_stack.Peek(), Is.EqualTo(7));
        _stack.Pop();
        Assert.That(_stack.IsEmpty(), Is.EqualTo(true));
    }

    [Test]
    public void TestPeekingAtTheTopOfTheStack()
    {
        _stack.Push(6);
        _stack.Push(7);
        Assert.That(_stack.Peek(), Is.EqualTo(7));
    }

    [Test]
    public void TestIfTheStackIsEmpty()
    {
        Assert.That(_stack.IsEmpty(), Is.True);
        _stack.Push(5);
        Assert.That(_stack.IsEmpty(), Is.EqualTo(false));
    }
}