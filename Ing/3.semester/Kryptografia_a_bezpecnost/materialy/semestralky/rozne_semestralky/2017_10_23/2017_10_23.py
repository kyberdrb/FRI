from collections import defaultdict
import json
import string
# import random

ALPHABET_MOD = 26
CIPHER_LENGTH = 4
SEED = 0
A = 84589
B = 45989
MOD = 217728

class RandomGenerator:
    def __init__(self, paSeed, paA, paB, paMod):
        self.a = paA
        self.b = paB
        self.mod = paMod
        self.next_rand = paSeed

    def nextRandom(self):
        self.next_rand = (self.a * self.next_rand + self.b) % self.mod
        return self.next_rand / float(self.mod)

def toNumber(paChar):
    c = paChar.lower()
    number = ord(c) - ord('a')
    return number

def toChar(paNumber):
    c = chr(paNumber+ord('a'))
    return c

def frekvAnalyza(paText):
    out = defaultdict(int)
    total = 0
    for c in string.ascii_lowercase:
        out[c] = paText.count(c)
        total += out[c]
    out["total"] = total
    with open('index.json', 'w') as write_file:
        json.dump(out, write_file, indent=2)
    return out

def indexKoincidencie(paText):
    out = defaultdict(int)
    total = 0
    for c in paText:
        if c.lower().isalpha():
            out[c] += 1
            total += 1
    index = 0
    for c in out:
        index += (out[c]/float(total))*((out[c]-1)/float((total-1)))
    return index

def encryptStreamCipher(paSeed, paText):
    out = ""
    rand = RandomGenerator(paSeed, A, B, MOD)
    # random.seed(100)
    for c in paText:
        if c.isalpha():
            # key = random.randint(0, 25)
            key = int(ALPHABET_MOD*rand.nextRandom())
            out += toChar((toNumber(c)+key) % ALPHABET_MOD)
        else:
            out += c
    return out

def decryptStreamCipher(paSeed, paText):
    out = ""
    rand = RandomGenerator(paSeed, A, B, MOD)
    for c in paText:
        if c.isalpha():
            # key = random.randint(0, 25)
            key = int(ALPHABET_MOD*rand.nextRandom())
            out += toChar((toNumber(c)-key) % ALPHABET_MOD)
        else:
            out += c
    return out

# with open('text.txt', 'r') as read_file:
#     intext = read_file.read().lower()

# zasifrovane = encryptStreamCipher(intext)
# print zasifrovane
# print indexKoincidencie(intext), indexKoincidencie(zasifrovane)
# for i in range(0,100000):
#     random.seed(i)
#     decrypted = decryptStreamCipher(zasifrovane)
#     if indexKoincidencie(decrypted) > 0.03:
#         print i, indexKoincidencie(decrypted), decrypted
#     if decrypted == intext:
#         exit(0)

def findKey(paInFile):
    with open(paInFile, 'r') as read_file:
        encrypted = read_file.read().lower()
    for key in range(MOD):
        # print(key)
        decrypted = decryptStreamCipher(key, encrypted)
        if indexKoincidencie(decrypted) > 0.06:
            print key
            print decrypted
            print '----------------------------------------------------'
            # if encryptStreamCipher(key, decrypted) == encrypted



if __name__ == '__main__':
    print findKey('text1_enc.txt')
    # print findKey('text2_enc.txt')
    # print findKey('text3_enc.txt')
    # print findKey('text4_enc.txt')
# def test():
#     with open('text.txt', 'r') as read_file:
#         intext = read_file.read().lower()
#     modified = ""
#     for c in intext:
#         if c.isalpha():
#             modified += c
#     groups = toGroups(modified, CIPHER_LENGTH)
#     print groups
#     return findCipher(groups)

# if __name__ == '__main__':
#     # indexKoincidencie('referencia_svk.txt')
#     print test()