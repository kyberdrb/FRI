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

### Vytvorenie a práca s topológiou

**Poradie krokov pri spúšťaní topológie v Miniedit GUI:  
Spustenie POX radiča -> Spustenie **

1. Na vytvorenie a prácu s Mininet topológiou je potrebné mať k Mininet VM otvorené 2 SSH relácie: prvá slúži na interakciu s Mininet topológiou prostredníctvom nástroja Miniedit, druhá slúži na manipuláciu s radičom.
1. Otvoríme novú SSH reláciu k Mininet VM. Tento krát nie je potrebné aktivovať *X11 Forwarding* cez SSH.

        ssh mininet@<IP_adresa_Mininet_VM>

1. Spustíme radič, v našom prípade POX, v práve otvorenej SSH relácii. Radič zatiaľ spustíme iba na otestovanie, či prepínač preposiela prevádzku, a či je existuje konektivita medzi koncovými zariadeniami. Na otestovanie použijeme buď príkaz, ktorý používa L3 prepínač (IP adresy)

        python ~/pox/pox.py log.level --DEBUG forwarding.l3_learning &
    alebo L2 prepínač (MAC adresy)

        python ~/pox/pox.py log.level --DEBUG forwarding.l2_learning &

    Príkaz vygeneruje takýto výstup:

        [1] 3166
        ...
        DEBUG:core:POX 0.2.0 (carp) going up...
        DEBUG:core:Running on CPython (2.7.6/Oct 26 2016 20:32:47)
        DEBUG:core:Platform is Linux-4.2.0-27-generic-i686-with-Ubuntu-14.04-trusty
        DEBUG:forwarding.l3_learning:Up...
        INFO:core:POX 0.2.0 (carp) is up.
        DEBUG:openflow.of_01:Listening on 0.0.0.0:6633

    Z výstupu o.i. vyplýva, že radič je spustený v procese, ktorého PID je `3166`, počúva na porte `6633` a načítal modul `forwarding.l3_learning`.
    
1. Spustíme topológiu v Miniedit GUI
    * buď kliknutím na položku *Run* v menu lište a zvolením možnosti *Run*,
    * alebo kliknutím na tlačidlo *Run* v ľavom dolnom rohu Miniedit GUI.

    Po spustení topológie uvidíme v aktuálnej (POX) SSH relácií takýto výstup, na konci ktorého sa dostaneme do Mininet príkazového riadku.

        INFO:openflow.of_01:[None 1] closed
        INFO:openflow.of_01:[00-00-00-00-00-01 2] connected

    Po spustení topológie uvidíme v Miniedit SSH relácií takýto výstup, na konci ktorého sa dostaneme do Mininet príkazového riadku.

        Build network based on our topology.
        Getting Hosts and Switches.
        Getting controller selection:remote
        ...
        NOTE: PLEASE REMEMBER TO EXIT THE CLI BEFORE YOU PRESS THE STOP BUTTON. Not exiting will prevent MiniEdit from quitting and will prevent you from starting the network again during this sessoin.

        *** Starting CLI:
        mininet>

    Výstup obsahuje aj správu `NOTE`, ktorá vysvetľuje, ako správne vypnúť topológiu, inak nebude možné spustiť ju znova. Tomuto problému sa venujeme v ďalších krokoch tohto návodu.
1. Otestujeme konektivitu medzi jednotlivými koncovými zariadeniami vykonaním príkazu

        pingall

    z príkazového riadku `mininet>`.

    Ak sme spustili radič príkazom, ktorý používa L2 prepínač, ten funguje v POX radiči rýchlejšie, než L3 prepínač. Pri L3 prepínači môže pri reštartoch radiča nastať úvodné zdržanie, čo má za následok, že sa prevádzka nemusí preposielať, napr. pri vykonaní príkazu `pingall` v Mininet príkazovom riadku. To je zapríčinené pomalším učením L3 prepínača resp. oneskorenými ARP odpoveďami. Avšak po úvodnom naučení sa všetkých potrebných MAC adries v topológii už L3 prepínač preposiela prevádzku bez zdržania. V prípade príkazu `pingall` sa tento vykoná bez zdržania a za okamih sa úspešne ukončí.

    Ak radič pred spustením celej topológie nespustíme, prepínač pripojený ku radiču nebude preposielať prevádzku, keďže prepínač typu *Switch*, narozdiel od prepínača typu *LegacySwitch*, vyžaduje spustený radič.
1. Pripojíme sa na Mininet VM pomocou SSH s aktivovanou funkciou *X11 Forwarding*. Prihlásime sa s predvolenými prihlasovacími údajmi.
    1. Vo OS Windows sa na Mininet cez SSH s *X11 Forwarding* funkciou pripojíme pomocou [*Putty*](https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html). Ešte predtým ale musíme nainštalovať [*Xming*](https://sourceforge.net/projects/xming/files/latest/download). *Xming* pridá do *Putty* funkciu *X11 Forwarding*. IP adresu Mininet VM zistíme príkazom `ip a` na rozhraní `eth0`. Po nainštalovaní *Xming* a *Putty* otvoríme *Putty*. V *Putty* aktivujeme v časti *Connection -> SSH -> X11* sme aktivovali *X11 Forwarding* zaškrtnutím políčka "Enable X11 forwarding". Klikneme na 
    1. Na platforme Linux použijeme príkaz

            ssh -X mininet@<IP_adresa_Mininet_VM>

        resp.

            ssh -CY mininet@<IP_adresa_Mininet_VM>

1. V práve otvorenej SSH relácii spustíme nástroj *Miniedit*, čo je grafický nástroj na úpravu Minient topológií.

        sudo ~/mininet/examples/miniedit.py

1. V grafickom rozhraní klikneme v menu riadku na *File->Open*, a z adresára `/home/mininet/`resp. z domovského adresára `~`, otvoríme súbor [semkaTOPO.mn](semkaTOPO.mn). Týmto súborom je  definovaná topológia použitá pri testovaní *SDN firewall*.

    Ak tento súbor v spomenutom adresári nie je prítomný, stiahneme si ho z [adresára pre semestrálnu prácu](https://github.com/kyberdrb/FRI/blob/master/Ing/4.semester/Integracia_Sieti/semestralka/semkaTOPO.mn). Na stránke pravým tlačidlom klikneme na *Raw* a z kontextovej ponuky zvolíme možnosť *Save Link As...*. Následne súbor presunieme do adresára `/home/mininet/`, resp. do domovského adresára `~` na Mininet VM napr. pomocou nástroja *FileZilla*.

    Súbory typu `*.mn` sú textové súbory napísané vo formáte JSON, preto ich môžeme upravovať ako *sudo/root* v textovom editore. Musíme ale otvoriť novú SSH reláciu, keďže po spustení Miniedit GUI v aktuálnej SSH relácií nie je možné zadávať príkazy do príkazového riadku.

        ssh mininet@<IP_adresa_Mininet_VM>
        sudo vim ~/semkaTOPO.mn

    Jednotlivé kľúčové slová definujú objekty resp. prvky v topológii. Manuálna úprava súboru je užitočná vtedy, keď chceme spresniť súradnice, na ktorých sú umiestnené jednotlivé prvky topológie, čím môžeme docieliť lepší vzhľad topológie.

    Topológia obsahuje 3 koncové zariadenia (Host - h1,h2,h3), prepínač (Switch - s1) a SDN radič (Controller - c1) (ďalej len *radič*).

    ![Topológia](obrazky/topologia.png)

1. V Mininedit GUI klineme na menu lište na *Edit -> Preferences*. Zmeníme nastavenia takto:
    * *IP Base*: 10.0.0.0/24
    * *Default Terminal*: xterm
    * *Start CLI*: zaškrtnuté
    * *Default Switch*: Open vSwitch Kernel Mode
    * *Open vSwitch*: 1.0
1. Kliknutím a podržaním pravého tlačidla na koncových zariadeniach sa otvorí kontextové menu, z ktorého zvolíme možnosť *Properties*. Zariadeniam nastavíme adresáciu podľa nižšie uvedenej tabuľky.

    Zariadenie | IP adresa/Maska
    --- | ---
    h1 | 10.0.0.1/24
    h2 | 10.0.0.2/24
    h3 | 10.0.0.3/24

1. Radič *c1* nastavíme podľa nižšie uvedeného obrázka. Stlačíme a podržíme pravé tlačítko myši na kontroléri a vyberieme *Properties*).

    ![Topológia](obrazky/radic_konfiguracia.png)

1. Prepneme sa do Miniedit SSH relácie a do príkazového riadku `mininet>` zadáme príkaz

        pingall

    Ten overí konektivitu každého koncového zariadenia ku všetkým ostatným koncovým zariadeniam.
    
    Nižšie sú uvedené výstupy príkazu `pingall` pred a po spustení radiča.

        mininet> pingall
        *** Ping: testing ping reachability
        h3 -> X X 
        h2 -> X X 
        h1 -> X X 
        *** Results: 100% dropped (0/6 received)
        mininet> pingall
        *** Ping: testing ping reachability
        h3 -> h2 h1 
        h2 -> h3 h1 
        h1 -> h3 h2 
        *** Results: 0% dropped (6/6 received)
1. Potom, ako bola overená funkčnosť topológie, ju môžeme zavrieť:

    1. Najprv v Miniedit SSH relácií zadáme do `mininet>` príkazového riadku príkaz

            quit
    1. Prepneme sa do POX SSH relácie a ukončíme proces spustený radičom. Použijeme PID, ktorý sme  zadáme do

            pkill -f pox.py

    1. V Miniedit GUI zastavíme topológiu
        * buď kliknutím na položku *Run* v menu lište a zvolením možnosti *Stop*,
        * alebo kliknutím na tlačidlo *Stop* v ľavom dolnom rohu Miniedit GUI.

        Mininet topológia sa zastaví.
    1. Nakoniec zatvoríme okno Miniedit GUI.

    Topológiu musíme pred opätovným spustením vypnúť vyššie uvedenou postupnosťou krokov! Vyhneme sa tak zbytočným komplikáciám s aplikáciou a konektivitou v topológií.

    Nedodržanie poradia týchto krokov môže viesť aj ku zamrznutiu Mininet SSH relácie, ku nepredvídateľnému správaniu resp. k pádu Mininet procesu.

### Nasadenie modulu pre SDN firewall do SDN radiča POX

1. Po ukončení radiča môžeme prejsť k nasadeniu firewall modulu do POX radiča. Pri vytváraní firewall modulu sme ako základ použili [už vytvorený firewall balíček pre POX radič](https://github.com/rakeshdatta/SDN_Firewall). Jeho autorom je Rakesh Datta.

    Repozitár bol skopírovaný do nášho GitHub účtu, v ktorom sme vykonávali všetky úpravy. Potom bol tento repozitár naklonovaný do adresára `~/pox/ext/`, keďže POX radič hľadá rozširujúce balíčky v podadresároch <br>`pox` a `ext` t.j. <br> `~/pox/pox/` a `~/pox/ext/`.

    Firewall balíček môže byť naklonovaný do ľubovoľného adresára pre rozširujúce balíčky pre POX radič t.j. `/pox` aj `/ext`. Nakoniec sme si vybrali adresár `/ext`, keďže [dokumentácia k POX radiču](https://github.com/att/pox/blob/master/pox/boot.py) odporúča použiť spomenutý adresár pred adresárom `/pox` na vlastné rozširujúce balíčky.

    Predpokladáme, že POX radič, konkrétne súbor `pox.py` je umiestnený v domovskom adresári používateľa, t.j. `$HOME/pox/` resp. `~/pox/`. V opačnom prípade spúšťací skript nebude pracovať správne.

        cd ~/pox/ext/
        git clone https://github.com/kyberdrb/sdnfirewall.git

1. V POX SSH relácií sa presunieme do adresára firewall balíčka pre POX radič a spustíme ho príslušným skriptom:

        cd ~/pox/ext/sdnfirewall
        ./launcher.sh start
1. V Miniedit SSH relácií spustíme Miniedit GUI a otvoríme v ňom súbor s Mininet topológiou `semkaTOPO.mn`.

        sudo ~/mininet/examples/miniedit.py


**TODO - PRIDAT: funkcionality POX firewallu, riadenie POX firewallu skriptom, fw pravidla (csv, uprava pravidiel), testovanie firewallu**

1. Terminály všetkých koncových zariadení otvoríme z príkazového riadku `mininet>` príkazom

        xterm h1 h2 h3

1. Zmena pravidiel si vyžaduje reštart radiča príkazom

        ./launcher.sh restart

1. Po skončení práce s POX radičom ho ukončíme pomocou spúšťacieho skriptu príkazom

        ./launcher.sh stop

1. Aktualizácia firewall balíčka vykonáme stiahnutím najnovšej verzie z nášho GitHub repozitára:

        git -C ~/pox/ext/sdnfirewall pull
        ~/pox/ext/sdnfirewall/launcher.sh restart

    resp.

        cd ~/pox/ext/sdnfirewall
        git pull
        ./launcher.sh restart

Zdroje:  
* Markdown cheat sheet https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet#tables
* Host-only network adapter explanation https://www.virtualbox.org/manual/ch06.html#network_hostonly
* X11 Forwarding for SSH in XFCE desktop environment https://bbs.archlinux.org/viewtopic.php?pid=580795#p580795
* Markdown - automatic item numbering in a numbered list https://stackoverflow.com/questions/12145232/create-an-automatically-numbered-list/12145270#12145270
* X11 Forwarding using Putty on Windows https://www.youtube.com/watch?v=QRsma2vkEQE
* The 'launcher' shell script has been verified with https://www.shellcheck.net/
* Switch-case in Bash http://tldp.org/LDP/Bash-Beginners-Guide/html/sect_07_03.html
* Print new line with echo in Bash https://stackoverflow.com/questions/8467424/echo-newline-in-bash-prints-literal-n/8467449#8467449
* Print exit status of the last executed command http://tldp.org/LDP/abs/html/exit-status.html
* Print variable value with printf in Bash http://wiki.bash-hackers.org/commands/builtin/printf
* Functions in Bash https://ryanstutorials.net/bash-scripting-tutorial/bash-functions.php

* Reset Mininet prostredia https://github.com/mininet/mininet/issues/737#issuecomment-374834781

* Properly working with a file in Python http://www.pythonforbeginners.com/files/with-statement-in-python and  https://stackoverflow.com/questions/40416072/reading-file-using-relative-path-in-python-project/40416154
* Viacriadkové 'if' podmienky a zreťazenia - riešenie cet spätné lomítko '\' https://stackoverflow.com/questions/181530/styling-multi-line-conditions-in-if-statements/181557#181557
* Časti Python kódu boli testované na správnu syntax pomocou https://pythonbuddy.com/
* Písanie formátovanie argumentov vo funkcií https://github.com/python/typing/issues/433
* Python - Vykonanie úlohy po časovom odstupe https://stackoverflow.com/questions/3433559/python-time-delays/3433565#3433565
* Python - Vykonanie úlohy po časovom odstupe - dokumentácia k modulu "threading.Timer" https://docs.python.org/3/library/threading.html#timer-objects
* Python - Anonymný Timer https://stackoverflow.com/questions/3393612/run-certain-code-every-n-seconds/3393759#3393759
* Python - Vlákna a prenos argumentov k nim https://www.saltycrane.com/blog/2008/09/simplistic-python-thread-example/  a  https://stackoverflow.com/questions/5683411/timer-objects-python-with-arguments?rq=1#comment6490538_5683442
* Python - import modulov http://python-textbok.readthedocs.io/en/1.0/Packaging_and_Testing.html#modules  a  https://stackoverflow.com/questions/4142151/how-to-import-the-class-within-the-same-directory-or-sub-directory/4142178#4142178  a  https://stackoverflow.com/questions/41276067/importing-class-from-another-file/41276151#41276151  a  https://stackoverflow.com/questions/4534438/typeerror-module-object-is-not-callable/4534443#4534443
* Python - počítanie kontrolných súčtov (hashing/checksums) https://stackoverflow.com/questions/5297448/how-to-get-md5-sum-of-a-string-using-python/5297495#5297495  a  https://pymotw.com/2/hashlib/

* Návod na používanie Mininet VM s rôznymi radičmi (aj POX, Floodlight a pod.) https://github.com/mininet/openflow-tutorial/wiki/Create-a-Learning-Switch
*  How to write a simple own POX component - _handle metódy http://ofworkshop.blogspot.sk/2013/10/how-to-write-simple-own-pox-component.html
* Learning POX OpenFlow controller : hub implementation - _handle metódy https://haryachyy.wordpress.com/2014/05/29/learning-pox-sdn-controller-hub-py-module/
* OpenFlow klasifikátory - atribúty, ktoré je možné na prevádzke kontrolovať http://flowgrammable.org/sdn/openflow/classifiers/
* OpenFlow 1.0 Match štruktúra - slúži porovnávanie parametrov prevádzky http://flowgrammable.org/sdn/openflow/message-layer/match/#tab_ofp_1_0
* Parametre prevádzky a paketov, s ktorými POX dokáže pracovať https://github.com/att/pox/tree/master/pox/lib/packet
* POX OpenFlow - priority http://pox-dev.noxrepo.narkive.com/StbumRJo/installing-flow-really-appreciate-your-time
* OpenFlow 1.0 Message štruktúra http://flowgrammable.org/sdn/openflow/message-layer/flowmod/
* OpenFlow - hard_timeout https://www.juniper.net/documentation/en_US/junos/topics/concept/junos-sdn-openflow-flow-entry-timers-overview.html


* Implementácia "swtich-case" mechanizmu pre Python - užitočné na zisťovanie zhody s viacerými protokolmi, transportnými portami a pod. https://stackoverflow.com/questions/60208/replacements-for-switch-statement-in-python/103081#103081



* Zisťovanie času, kedy bol súbor naposledy zmenený https://stackoverflow.com/questions/182197/how-do-i-watch-a-file-for-changes/182259#182259
* Polling - periodické vykonávanie ľubovoľnej operácie, napr. aj sledovanie času poslednej zmeny súboru https://stackoverflow.com/questions/41713234/better-way-to-write-a-polling-function-in-python/41713452#41713452  a  
https://medium.com/@justiniso/using-the-polling-module-in-python-87052d7da4d9  a  
https://github.com/justiniso/polling