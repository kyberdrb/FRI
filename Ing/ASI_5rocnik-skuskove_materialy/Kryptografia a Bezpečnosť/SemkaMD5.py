#!/usr/bin/python
from hashlib import md5
from base64 import b64encode
import time
import csv

def crypt( passwd, salt ):
  m = md5()
  m.update( passwd )
  m.update( salt )
  return b64encode( m.digest() )

login=[]
sol=[]
hashZoSuboru=[]
hesla=[]

heslo="OWK5"
# heslo = heslo.encode("UTF-8")
solheslo="pooW9iep"
# solheslo = solheslo.encode("UTF-8")


# with open('8heslacisla.txt', 'w') as file:
#     for p in product('0123456789', repeat=8):
#         file.write("".join(p) + "\n")

start_time = time.time()

# subor = open("shadow1.txt", "r")
# for line in subor:
# 	stlpce = line.split(':')

# 	login.append(stlpce[0])
# 	sol.append(stlpce[1])
# 	hashZoSuboru.append(stlpce[2])

with open('shadow1.txt','r') as f:
	reader = csv.reader(f,delimiter=':')
	for loginn,soln,hashZoSuborun in reader:
		login.append(loginn)
		sol.append(soln)
		hashZoSuboru.append(hashZoSuborun)


# hesla = [suborHesla.strip() for suborHesla in open("slovenske_mena.txt","r")]

hesla = [suborHesla.strip() for suborHesla in open("slovenske_mena.txt","r")]
hesla = [item.lower() for item in hesla]

for i in range(len(hesla)):
	# if ( (i%10000) == 0) : print (i)
	for j in range(len(hashZoSuboru)):
		if (crypt(hesla[i],sol[j]) == hashZoSuboru[j]): 
			print (login[j] + " - " + hesla[i])

print("--- %s seconds ---" % (time.time() - start_time))

# subor.close()

# hesla v subore shadow1.txt:
# soukal - OWK5 (zapis4.txt)
# sagan00 - nbusr (5hesla.txt)
# bartosik2 - veronika (pass.txt , slovenske_mena.txt(lowerase))