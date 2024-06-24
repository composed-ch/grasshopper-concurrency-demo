using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

Console.WriteLine($"maxNumber: {maxNumber}, nThreads: {nThreads}");

List<uint> PrimesUpTo(uint n)
{
    var primes = new List<uint>();
    for (uint i = 2; i <= n / 2; i++)
    {
        bool isPrime = true;
        for (uint j = 2; j < i; j++)
        {
            if (i % j == 0)
            {
                isPrime = false;
                break;
            }
        }
        if (isPrime)
        {
            primes.Add(i);
        }
    }
    return primes;
}

List<uint> Factorize(uint n)
{
    var factors = new List<uint>();
    var primes = PrimesUpTo(n / 2);
    for (var i = 0; n > 1 && i < primes.Count;)
    {
        var factor = primes[i];
        if (n % factor == 0)
        {
            factors.Add(factor);
            n /= factor;
        }
        else
        {
            i++;
        }
    }
    if (factors.Count == 0)
    {
        return new List<uint>() { n };
    }
    return factors;
}

class Result
{
    public uint Number { get; set; }
    public List<uint> Factors { get; set; }
}

uint loops = (uint)Math.Floor((double)nThreads);
uint max = (uint)Math.Floor((double)maxNumber);
var rand = new Random();

var before = DateTime.Now;

var tasks = new List<Task<Result>>();
for (uint i = 0; i < loops; i++)
{
    var n = (uint)(rand.NextInt64() % max);
    var task = Task<Result>.Factory.StartNew(() =>
        new Result { Number = n, Factors = Factorize(n) }
    );
    tasks.Add(task);
}

foreach (var task in tasks)
{
    var primeStr = string.Join(", ", task.Result.Factors);
    Console.WriteLine($"factors of {task.Result.Number}: [{primeStr}]");
}

var after = DateTime.Now;
time = $"{after - before}";