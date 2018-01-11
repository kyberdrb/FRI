from fractions import gcd


def xgcd(b, n):
    # g je najvacsi spolocny delitel, x0, x1 si Bezoutove koeficienty
    x0, x1, y0, y1 = 1, 0, 0, 1
    while n != 0:
        q, b, n = b // n, n, b % n
        x0, x1 = x1, x0 - q * x1
        y0, y1 = y1, y0 - q * y1
    return  b, x0, y0

def mulinv(b, n):
    # vrati modularny inverzny prvok
    # b*n mod n == 1
    g, x, _ = xgcd(b, n)
    if g == 1:
        return x % n
#
# print xgcd(7, 17)
# print mulinv

# def phi(a):
#     b=a-1
#     c=0
#     while b:
#         if not xgcd(a,b)[0]-1:
#             c+=1
#             print c
#         b-=1
#     return c

def pow_mod(x, y, z):
    "Calculate (x ** y) % z efficiently."
    number = 1
    while y:
        if y & 1:
            number = (number * x) % z
        y >>= 1
        x = (x * x) % z
    return number

# print(pow_mod(7,2,10))


# def decryptRSA(paNum):
#     pass
#
# def encryptRSA(paNum):
#     pass
#
# x = 1234567890
e = 65537

y = [6029832903, 22496913456008, 17014716723435111315, 5077587957348826939798388, 357341101854457993054768343508]
n = [13169004533, 1690428486610429, 56341958081545199783, 6120215756887394998931731, 514261067785300163931552303017]
phi_n = [13168773228, 1690428403441440, 56341958066486836800, 6120215756882377775019792, 514261067785298709212537255760]
d = []
x = []

# bla = Divisors(100000000000)
# print bla.phi(n[0])


def pollards_rho(n):
    x = 2; y = 2; d = 1
    f = lambda x: (x**2 + 1) % n
    while d == 1:
        x = f(x); y = f(f(y))
        d = gcd(abs(x-y), n)
    if d != n: return d



for i in range(len(y)):
    p = pollards_rho(n[i])
    q = n[i]/p
    phi_n[i] = (p-1) * (q-1)
    d.append(mulinv(e, phi_n[i]))
    x.append(pow_mod(y[i], d[i], n[i]))
    print d[i]
    print x[i]




