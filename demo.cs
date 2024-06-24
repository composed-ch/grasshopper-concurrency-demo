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

class Result
{
    public List<uint> Primes { get; set; }
}

uint loops = (uint)Math.Floor((double)nThreads);
uint max = (uint)Math.Floor((double)maxNumber);

var tasks = new List<Task<Result>>();
for (uint i = 0; i < loops; i++)
{
    var task = Task<Result>.Factory.StartNew(() =>
        new Result { Primes = PrimesUpTo(max) }
    );
    tasks.Add(task);
}

foreach (var task in tasks)
{
    var primeStr = string.Join(", ", task.Result.Primes);
    Console.WriteLine($"primes: {primeStr}");
}
//var foobar = string.Join(',', PrimesUpTo(max));
//Console.WriteLine($"primes: {foobar}");