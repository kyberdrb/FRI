# popping from 0
# x0 ide do cipher
import pickle

c = [1,1,1,0,1,1,1,1,1]
register = [1,1,1,0,1,1,1,1,1]
def xor(paA, paB):
    return (paA and not paB) or (not paA and paB)

def calculate_next(paRegister, paC):
    return sum([paRegister[i]*paC[i] for i in range(len(paC))]) % 2

def generator(paC, paRegister, paCipherLength):
    cipher = []
    register = paRegister
    for i in range(paCipherLength):
        register.append(calculate_next(register, paC))
        cipher.append(register.pop(0))
    return cipher

cipher = generator(c, register, 20000)
print cipher
with open('fips.txt', 'w') as write_file:
    for i in cipher:
        write_file.write(str(i))








