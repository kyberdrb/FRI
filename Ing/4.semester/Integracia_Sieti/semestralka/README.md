# Integrácia sietí - Dokumentácia

**Autori:** Miroslav Kozák, Andrej Šišila, Marián Vachalík

**Téma:** SDN firewall

## Realizácia

### Nasadenie Mininet prostredia

1. Stiahneme Mininet VM z [oficiálnej stránky](https://github.com/mininet/mininet/wiki/Mininet-VM-Images)
1. Po rozbalení zipu spustíme súbor s koncovkou `.ovf`. Následne sa zobrazí dialógové okno na importovanie Mininet VM do nástroja VirtualBox (ďalej len VBox). Nastavenia ponecháme na predvolených hodnotách a klikneme na *Import*.
1. Po úspešnom importovaní Mininet VM do nástroja VBox na ňu klikneme pravým tlačidlom a zvolíme *Settings*. Otvorí sa dialógové okno s nastaveniami VM. Zmeníme tieto nastavenia:
    1. **Network**

        1. Ak sme na internet pripojený káblom, stačí zapnúť jeden sieťový adaptér - *Adapter 1*. Pre *Adapter 1* zvolíme *Bridged Adapter*. Všetky ostatné adaptéry vypneme - odškrtneme voľbu *Enable Network Adapter*.

        1. Ak sme na internet pripojený bezdrôtovo, napr. cez Wi-Fi, *Bridged Adapter* nám nefungoval správne. Museli sme použiť dva sieťové adaptéry: *NAT* a *Host-only Adapter*. Typ sieťového adaptéra nastavíme v riadku *Attached to:*. *NAT* bude slúžiť na pripojenie k internetu, *Host-only Adapter* bude slúžiť na prácu s topológiou. Po stiahnutí všetkých potrebných súborov z internetu internetové pripojenie pre beh topólogie v Mininet VM nie je potrebné.

            1. Najprv vytvoríme nový *Host-only Adapter* tak, že zatvoríme dialógové okno s nastaveniami a v hlavnom VBox okne v hornom menu riadku klikneme na *File -> Host Network Manager...*.
            1. Otvorí sa dialógové okno *Host Network Manager*. Klikneme na tlačidlo *Create*, čo vytvorí nový *Host-only* sieťový adaptér s názvom napr. "vboxnet0".
            1. Pre adaptér aktivujeme DHCP zaškrtnutím voľby "Enable" v stĺpci "DHCP Server".
            1. Zatvoríme dialógové okno *Host Network Manager* a znovu otvoríme dialógové okno s nastaveniami pre Mininet VM.
            1. V ľavom stĺpci zvolíme možnosť *Network*. Nastavenia jednotlivých adaptérov upravíme nasledovne:

                * Adapter 1
                    * zaškrtneme *Enable Network Adapter*
                    * *Attached to:* NAT.

                * Adapter 2
                    * zaškrtneme *Enable Network Adapter*
                    * *Attached to:* Host-only Adapter
                    * Name: vboxnet0
                    * Klikneme na *Advanced* a nastavíme *Promiscuous Mode:* Allow All (pre istotu)

                Adapter 3, Adapter 4 - vypnuté t.j. možnosť *Enable Network Adapter*


1. Spustíme Mininet VM.
1. Po spustení sa do Mininet VM prihlásime pod predvolenými prihlasovacími údajmi `mininet/mininet`
1. Za ideálnych podmienok DHCP pridelí IP adresy Ethernet rozhraniu (káblové pripojenie), resp. adaptéru *2* pri bezdrôtovom pripojení. Avšak adaptér *1* môže byť stále vypnutý, čo skontrolujeme príkazom `ip a` v príkazovom riadku Mininet VM. Neinicializovaný adaptér sa zobrazil ako `eth1`. Vyžiadame si IP adresu pre toto rozhranie príkazom

        sudo dhclient eth1

1. Pripojíme sa na Mininet VM pomocou SSH s aktivovanou funkciou *X11 Forwarding*. Prihlásime sa s predvolenými prihlasovacími údajmi.
    1. Vo OS Windows sa na Mininet cez SSH s *X11 Forwarding* funkciou pripojíme pomocou [*Putty*](https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html). Ešte predtým ale musíme nainštalovať [*Xming*](https://sourceforge.net/projects/xming/files/latest/download). *Xming* pridá do *Putty* funkciu *X11 Forwarding*. IP adresu Mininet VM zistíme príkazom “ip a” na rozhraní “eth0”. Po nainštalovaní *Xming* a *Putty* otvoríme *Putty*. V *Putty* aktivujeme v časti *Connection -> SSH -> X11* sme aktivovali *X11 Forwarding* zaškrtnutím políčka "Enable X11 forwarding". Klikneme na 
    1. Na platforme Linux použijeme príkaz

            ssh -X mininet@<IP_adresa_Mininet_VM>

        resp.

            ssh -CY mininet@<IP_adresa_Mininet_VM>

### Vytvorenie topológie

1. Príkazom

        sudo /home/mininet/mininet/examples/miniedit.py
        
    otvoríme *Mininedit* - grafické rozhranie nástroja *Mininet*
1. V grafickom rozhraní klikneme v menu riadku na *File->Open* a otvoríme súbor  [/home/mininet/semkaTOPO.mn](semkaTOPO.mn)  
Tento súbor definuje topológiu použitú pri testovaní *SDN firewall*. Topológia obsahuje 3 koncové zariadenia (h1,h2,h3), prepínač (s1) a radič (c1).

![Topológia](obrazky/topologia.png)

1. V nastaveniach *Edit -> Preferences* sme zaškrtli *Start cli* a *IP Base* sme nastavili na 10.0.0.0/24.
1. Kliknutím a podržaním pravého tlačidla na koncových zariadeniach sa otvorí kontextové menu, z ktorého zvolíme možnosť *Properties*. Zariadeniam nastavíme adresáciu podľa nižšie uvedenej tabuľky.

    Zariadenie | IP adresa/Maska
    --- | --- | ---
    h1 | 10.0.0.1/24
    h2 | 10.0.0.2/24
    h3 | 10.0.0.3/24

1. Kontrolér *c1* nastavíme podľa obrázka. Stlačíme a podržíme pravé tlačítko myši na kontroléri a vyberieme *Properties*).

![Topológia](obrazky/controller_konfig.png)

1. Spustili sme topológiu cez Run -> Run.
1. Ako zaklad svojej prace sme pouzili POX radic a firewall modul.

        cd
        git clone https://github.com/rakeshdatta/SDN_Firewall.git
1.  Otvoríme si ďalšiu SSH reláciu na mininet pomocou putty, prihlasíme sa a dostaneme sa do zložky kontroléra pox príkazom:
mininet@mininet-vm:~$ cd /home/mininet/pox/.
1.  Spustíme POX kontrolér, ktorý bude plniť úlohu L3 SDN firewallu: mininet@mininet-vm:~/pox$ ./myacl start
1. DALSI POPIS

Zdroje:  
* https://www.virtualbox.org/manual/ch06.html#network_hostonly
* https://bbs.archlinux.org/viewtopic.php?pid=580795#p580795
* https://stackoverflow.com/questions/12145232/create-an-automatically-numbered-list/12145270#12145270
* [X11 Forwarding using Putty on Windows](https://www.youtube.com/watch?v=QRsma2vkEQE)
    