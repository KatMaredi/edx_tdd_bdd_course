using Microsoft.AspNetCore.Mvc;
using practicing_tdd.models;

namespace practicing_tdd.controllers;

[ApiController]
[Route("counters")]
public class CounterController : ControllerBase
{
    public static readonly Dictionary<string, Counter> Counters = new();

    [HttpPost("{name}")]
    public IActionResult CreateCounter(string name)
    {
        Console.WriteLine($"Request to create counter: {name}");

        if (Counters.ContainsKey(name))
        {
            return Conflict(new { message = $"Counter {name} already exists" });
        }
        var counter = new Counter { Name = name };
        Counters[name] = counter;

        return CreatedAtAction(nameof(CreateCounter),
            new { name = counter.Name },
            new { counter.Name, counter.Value });
    }

    [HttpGet("{name}")]
    public IActionResult GetCounter(string name)
    {
        Console.WriteLine($"Request to get counter: {name}");

        var counter = Counters[name];

        return Ok(new { counter.Name, counter.Value });
    }
}