from threading import Thread
from random import randint
from datetime import datetime


def print_factors(n):
    factors = factorize(n)
    print('factors of {}: {}'.format(n, factors))


def primes_up_to(n):
    primes = []
    for i in range(2, int(n)+1):
        is_prime = True
        for j in range(2, i):
            if i % j == 0:
                is_prime = False
                break
        if is_prime:
            primes.append(i)
    return primes


def factorize(n):
    factors = []
    primes = primes_up_to(n/2)
    i = 0
    while n > 1 and i < len(primes):
        f = primes[i]
        if n % f == 0:
            factors.append(f)
            n /= f
        else:
            i += 1
    if not factors:
        return [n]
    return factors


# input parameters
global max_number
global n_threads

# output parameters
global factors
global time

started = datetime.now()
threads = []
for i in range(int(n_threads)):
    n = randint(int(max_number / 10), max_number)
    thread = Thread(target=print_factors, args=[n])
    thread.run()
    threads.append(thread)
finished = datetime.now()
time = finished - started