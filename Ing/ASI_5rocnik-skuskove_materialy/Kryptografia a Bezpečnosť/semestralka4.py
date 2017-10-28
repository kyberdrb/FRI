#!/usr/bin/python

def extEuclideanAlg(a, b) :
    """
    Computes a solution  to a x + b y = gcd(a,b), as well as gcd(a,b)
    """
    if b == 0 :
        return 1,0,a
    else :
        x, y, gcd = extEuclideanAlg(b, a % b)
        return y, x - y * (a // b),gcd
 
def modInvEuclid(a,m) :
    """
    Computes the modular multiplicative inverse of a modulo m,
    using the extended Euclidean algorithm
    """
    x,y,gcd = extEuclideanAlg(a,m)
    if gcd == 1 :
        return x % m
    else :
        return None

n = 504768094323783192829417

# prvocisla (z wolfram alpha: factor n)
p = 692392207123
q = 729020472979

# zasifrovana sprava
y = 326828339872899881465916
print("Zasifrovana sprava: " + str(y))

# verejny exponent
e = 65537

FiN = (p-1)*(q-1)
print ("FiN: " + str(FiN))

# d - pre velke pow() je zly vypocet
# d = 417049647853384281726737
# d = pow(e, (FiN-1), FiN)
d = modInvEuclid(e,FiN)
print ("d: " + str(d))

print ("verejny kluc: (" + str(e) + " , " + str(n) + ")")
print ("privatny kluc: (" + str(d) + " , " + str(n) + ")")

# desifrovana sprava
x = pow(y, d, n)
print("Desifrovana sprava: " + str(x))