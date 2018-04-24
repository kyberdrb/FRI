# Projektovanie sietí 2

BGP mVPN NG

# Topológia

PC1 - CE1 - PE1 - P - PE2 - CE2 - PC2

# Zjednodušená topológia

CE1 - PE1 - P - PE2 - CE2

## Význam uzlov v topológii

PC1, PC2 - koncové stanice
CE1, CE2 - hraničné smerovače zákazníkov
PE1, PE2 - hraničné smerovače poskytovateľa
P - vnútorné smerovače poskytovateľa; v našom prípade reprezentovaný iba jediným smerovačom

## Použité zariadenia

PC1 - koncová stanica  
PC2 - koncová stanica  
CE1, CE2 - Cisco 2811 - IOS 15.3(3)-XB12 2013-11-19 - model: 2800  
P, PE1 - Juniper M7 - JunOS 13.3R8.7 2015-10-23 - model: m7i  
PE2 - Juniper SSG 320M - JunOS 12.1X46-D72.2 2017-12-23 - model: j2320

## Adresácia


### PC1
eth0 (bridged eth1) - 192.168.1.2/24

### CE1
f0/0 - 192.168.1.1/24  
f0/1 - 10.0.1.2/30
lo0 - 10.255.255.21/32

### PE1
fe-0/3/1 - 10.0.1.1/30  
fe-0/3/0 - 10.0.10.2/30
lo0 - 10.255.255.101/32

### P
fe-0/3/0 - 10.0.10.1/30  
fe-0/3/2 - 10.0.20.1/30
lo0 - 10.255.255.100/32

### PE2
ge-0/0/2 - 10.0.20.2/30  
ge-0/0/1 - 10.0.2.1/30
lo0 - 10.255.255.102/32

### CE2
f0/1 - 10.0.2.2/30
f0/0 - 192.168.2.1/24
lo0 - 10.255.255.22/32

### PC2
eth0 (bridged eth1) - 192.168.2.2/24