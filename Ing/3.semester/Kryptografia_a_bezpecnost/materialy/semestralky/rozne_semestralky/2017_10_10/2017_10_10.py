from collections import defaultdict
import json
import string
import os
import re

clear = lambda: os.system('cls' if os.name=='nt' else 'clear')

MODULO = 26
CIPHER_LENGTH = 4
float

def toNumber(paChar):
    c = paChar.lower()
    number = ord(c) - ord('a')
    return number

def toChar(paNumber):
    c = chr(paNumber+ord('a'))
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

def decryptCaesar(plaintext, shift):
    alphabet = string.ascii_lowercase
    shifted_alphabet = alphabet[shift:] + alphabet[:shift]
    table = string.maketrans(alphabet, shifted_alphabet)
    return plaintext.translate(table)

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
        index += (out[c]/float(total))*((out[c]-1)/float(total-1))
    return index

def toGroups(paText, paCipherLength):
    groups = []
    for i in range(0, paCipherLength, 1):
        groups.append(paText[i::paCipherLength])
    return groups

def findCipher(paGroups, paLanguage):
    cipher = ""
    best_key = ""
    referencia = frekvAnalyza("referencia_{0}.txt".format(paLanguage))
    for group in paGroups:
        temp_cipher_prob = float('inf')
        for key in string.ascii_lowercase:
            min_prob = float(0)
            for i in string.ascii_lowercase:
                pi = referencia[i] / float(referencia["total"])
                # print pi
                j = chr(((ord(i)-ord('a')+ord(key)-ord('a')) % MODULO)+ord('a'))
                pj = group.count(j)/float(len(group))
                min_prob += abs(float(pi)-float(pj))
                # print i, cipher, j, pi, pj, min_prob, len(group)#, pi, pj, abs(pi-pj), min_prob
            # print cipher, min_prob
            if min_prob < temp_cipher_prob:
                temp_cipher_prob = min_prob
                best_key = key
        cipher += best_key
        # print best_key
    return cipher


# def test():
#     with open('text.txt', 'r') as read_file:
#         intext = read_file.read().lower()
#     modified = ""
#     for c in intext:
#         if c.isalpha():
#             modified += c
#     groups = toGroups(modified, CIPHER_LENGTH)
#     # print groups
#     return findCipher(groups)

def decryptViegenere(paText, paCipher):
    decrypted = ""

    counter = 0
    for c in paText:
        if c.isalpha():
            decrypted += toChar((toNumber(c) - toNumber(paCipher[counter])) % MODULO)
            counter = (counter + 1) % len(paCipher)
        else:
            decrypted += c
    return {'cipher': paCipher, 'text': decrypted, 'encrypted': paText}


def main(paText, paCipherLength, paLanguage):
    modified = ""
    for c in paText:
        if c.isalpha():
            modified += c
    groups = toGroups(modified, paCipherLength)
    cipher = findCipher(groups, paLanguage)

    return decryptViegenere(paText, cipher)


def semestralka(paLanguage, paInFile, paOutFile):
    reference_file = "referencia_{0}.txt".format(paLanguage)
    reference_index = indexKoincidencie(reference_file)
    decrypted_list = []
    for cipher_len in range(20,30):
        with open(paInFile, "r") as read_file:
            intext = read_file.read().lower()

        decrypted = main(intext, cipher_len, paLanguage)
        index = indexKoincidencie(decrypted['text'])
        # if abs(reference_index_svk - index) < abs(reference_index_svk - best_index):
        #     best_index = index
        #     best_decrypted = decrypted
        decrypted_list.append(decrypted)
        print "Index pola: {0}".format(len(decrypted_list)-1)
        print "Index koincidencie", index
        print decrypted['cipher'], '\n', decrypted['text']
        print decrypted_list

        print '---------------------------------------'
    decrypted_index = int(input("Zadajte index pola najlepsej dekrypcie: "))
    print decrypted_list[decrypted_index]
    with open(paOutFile, 'w') as write_file:
        json.dump(decrypted_list[decrypted_index], write_file, indent=4)

def cipherCorrection(paInFile, paOutFile):
    with open(paInFile, 'r') as read_file:
        decrypted = json.load(read_file)
    regex = re.compile('[^a-zA-Z ]')
    decrypted_text = regex.sub('', decrypted['text'])
    print decrypted_text


    # modified_decrypted = ""
    user_text = ""
    while(len(''.join([i for i in user_text if i.isalpha()])) < len(decrypted['cipher'])):
        user_text = raw_input("Zadajte spravny text(aspon {0} znakov):\n".format(len(decrypted['cipher'])))
    num_decrypted = toNumberList(''.join([i for i in decrypted_text if i.isalpha()]))
    num_user = toNumberList(''.join([i for i in user_text if i.isalpha()]))
    num_cipher = toNumberList(decrypted['cipher'])
    new_cipher = []
    for i in range(len(num_cipher)):
        result = num_cipher[i] - num_user[i] + num_decrypted[i]
        print num_cipher[i], num_user[i], num_decrypted[i], result, result % MODULO
        new_cipher.append(result%MODULO)
    print num_cipher
    print new_cipher
    new_decrypted = decryptViegenere(decrypted['encrypted'], toText(new_cipher))
    print new_decrypted
    with open(paOutFile, 'w') as write_file:
        json.dump(new_decrypted, write_file, indent=4)








if __name__ == '__main__':
    pass
    # semestralka('svk', 'text1_enc.txt', 'text1_dec.json')
    # cipherCorrection('text1_dec.json', 'text1_dec_final.json')
    # semestralka('svk', 'text2_enc.txt', 'text2_dec.json')
    # cipherCorrection('text2_dec.json', 'text2_dec_final.json')
    # semestralka('svk', 'text3_enc.txt', 'text3_dec.json')
    # cipherCorrection('text3_dec.json', 'text3_dec_final.json')
    # semestralka('eng', 'text4_enc.txt', 'text4_dec.json')
    # cipherCorrection('text4_dec.json', 'text4_dec_final.json')
