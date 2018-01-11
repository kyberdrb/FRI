from collections import defaultdict

###plain text(priamy) --> sifra --> cipher text (sifrovany)
#P = p1, p2, p3
#C = c1, c2, c3
# Ek(p) -> C
# Dk(C) -> P

# caesar -> c[i] = p[i]+ K mod 26
# c[i] = p[i] + k mod 26; p[i] = c[i] + k`; k+k` mod 26=0 4

a = 'a'
print chr(ord(a)+1)

# afinna sifra
# c[i] = k1 * p[i] + k2 mod 26 K=(k1, k2); K`=(k1`,k2`); k1 = 1,2,4..12,14..26; NSD(k1, 26) = 1
# p[i] = k[1]^-1 * (c[i]+(-k2)) mod 26
# p[i] = k1`*c1+k2`
MODULO = 27

sifrovana = "LIYGTOGDPOAUPDFQNVPVDAQV"
def frekv(paText):
    analysis = defaultdict(int)
    for i in paText:
        analysis[i] += 1
    return analysis

def toNumber(paChar):
    c = paChar.lower()
    number = ord(c) - ord('a') if c != ' ' else MODULO-1
    return number

def toChar(paNumber):
    c = chr(paNumber+ord('a')) if paNumber != MODULO-1 else ' '
    return c

def toText(paNumList):
    text = ""
    for i in paNumList:
        text += toChar(i)
    return text
def toNumberList(paText):
    numbers = []
    for i in paText:
        numbers.append(toNumber(i))
    return numbers

def egcd(a, b):
    if a == 0:
        return (b, 0, 1)
    else:
        g, y, x = egcd(b % a, a)
        return (g, x - (b // a) * y, y)

def modinv(a, m):
    g, x, y = egcd(a, m)
    if g != 1:
        # raise Exception('modular inverse does not exist')
        return -1
    else:
        return x % m

k1 = [x for x in range(0, MODULO) if x%3!=0]
k2 = list(range(1,MODULO-1))
# k1_inverse = [modinv(x, MODULO) for x in k1]
# k2_inverse = [modinv(x, MODULO) for x in k2]
def encrypt(paInputNumbers, paK1, paK2):
    return [(paK1*number+paK2)%MODULO for number in paInputNumbers]

def bruteForce(paInput, paK1, paK2):
    input_numbers = toNumberList(paInput)
    # for c in paInput:
    #     input_numbers.append(toNumber(c))

    zoznam = []
    for k1 in paK1:
        for k2 in paK2:
            if modinv(k1, MODULO) != -1:
                text = encrypt(input_numbers, k1, k2)
                k1_inv = modinv(k1, MODULO)
                k2_inv = (input_numbers[0]-(k1_inv*text[0])) % 27
                zoznam.append({'k1': k1, 'k2': k2, 'k1_inv': k1_inv, 'k2_inv': k2_inv, 'text': text}) # alebo decrypted, tu je to symetricke
    return zoznam

output = bruteForce(sifrovana, k1, k2)
for l in output:
    print "k1: {0}({1})\tk2: {2}({3})\t{4} ({5})".format(l['k1'], l['k1_inv'], l['k2'], l['k2_inv'], toText(l["text"]), toText(encrypt(l['text'], l['k1_inv'], l['k2_inv'])))



