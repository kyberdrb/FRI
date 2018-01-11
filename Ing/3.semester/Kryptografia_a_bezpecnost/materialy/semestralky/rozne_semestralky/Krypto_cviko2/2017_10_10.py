from collections import defaultdict
import json
import string

MODULO = 26
CIPHER_LENGTH = 4
float
def frekvAnalyza(paFile):
    out = defaultdict(int)
    total = 0
    with open(paFile, 'r') as read_file:
        text = read_file.read().lower()
    for c in string.ascii_lowercase:
        out[c] = text.count(c)
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
        index += (out[c]/total)*((out[c]-1)/(total-1))
    return index

def toGroups(paText, paCipherLength):
    groups = []
    for i in range(0, paCipherLength, 1):
        groups.append(paText[i::paCipherLength])
    return groups

def findCipher(paGroups):
    cipher = ""
    best_key = ""
    referencia = frekvAnalyza('referencia.txt')
    for group in paGroups:
        temp_cipher_prob = float('inf')
        for key in string.ascii_lowercase:
            min_prob = float(0)
            for i in string.ascii_lowercase:
                pi = referencia[i] / float(referencia["total"])
                j = chr(((ord(i)-ord('a')+ord(key)-ord('a')) % MODULO)+ord('a'))
                pj = group.count(j)/float(len(group))
                min_prob += abs(float(pi)-float(pj))
                # print i, cipher, j, pi, pj, min_prob, len(group)#, pi, pj, abs(pi-pj), min_prob
            # print cipher, min_prob
            if min_prob < temp_cipher_prob:
                temp_cipher_prob = min_prob
                best_key = key
        cipher += best_key
        print best_key
    return cipher

def test():
    with open('text.txt', 'r') as read_file:
        intext = read_file.read().lower()
    modified = ""
    for c in intext:
        if c.isalpha():
            modified += c
    groups = toGroups(modified, CIPHER_LENGTH)
    print groups
    return findCipher(groups)







if __name__ == '__main__':
    # indexKoincidencie('referencia.txt')
    print test()