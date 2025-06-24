using System.ComponentModel.DataAnnotations;

namespace practicing_tdd.models;

public class Counter
{
    public string Name { get; set; }
    public int Value { get; set; } = 0;
}