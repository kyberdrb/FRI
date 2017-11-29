package viegenerova_sifra;

/**
 *
 * @author Samo
 */
public class Viegenerova_sifra 
{
    static  final int ABECEDA = 65;//97;//65;
    static final int VELKOSTPOLA = 26;
    static final double PRAVDVYSKYTU[] = new double[]
                                         {
                                         0.08945,0.01124,0.03372,
                                         0.01124,0.07596,0.00266,0.00222,
                                         0.02050,0.06590,0.01920,0.03172,
                                         0.03189,0.02539,0.05324,0.08511,
                                         0.02538,0.0000,0.03789,0.04969,
                                         0.03265,0.03202,0.04057,0.00011,
                                         0.00047,0.02322,0.02628
                                         };
    static final double PRAVDVYSKYTUENG[] = new double[]
                                            {
                                             0.0804,0.0154,0.0306,
                                             0.0399,0.1251,0.0230,0.0196,
                                             0.0549,0.0726,0.0016,0.0067,
                                             0.0414,0.0253,0.0709,0.0760,
                                             0.0200,0.0011,0.0612,0.0654,
                                             0.0925,0.0271,0.0099,0.0192,
                                             0.0019,0.0173,0.0009
                                            };
    
    
    public static void main(String[] args) 
    {
        int maxDlzkaHesla = 20;
//        String sifra = "helloworld";
//        String sifra = "vptnvffuntshtarptymjwzirappljmhhqvsubwlzzygvtyitarptyiougxiuydtgzhhvvmum" +
//                        "shwkzgstfmekvmpkswdgbilvjljmglmjfqwioiivknulvvfemioiemojtywdsajtwmtcgluy" +
//                        "sdsumfbieugmvalvxkjduetukatymvkqzhvqvgvptytjwwldyeevquhlulwpkt";
        String sifra = "UYCKTLTAERJNNJBDAUOUYGIJNNZRCRSQOHGUOCSWSRSQOQOIYRODRJHN" +
                        "PYOEDLDXDVPWOZHRVFGTYACEJLRWOAZRVFQQZUOTOCFNFLFNNJBNHVHNXAIERVNWYJV" +
                        "TOKCEYJVJBLQNDHQQTLZNGYOONHHNLLUAAMBJSTSMZLFXUHGLIPZJTPB" +
                        "DRJHNPYOEDLDXDVPWOZHRDCCSIJOSTYCSIJNWARCENHNJKSOMETSAAUWWACFQNPHN" +
                        "NHVXDUMPEUSAAACASSCEDHBNHVXJZFYJ";
        
        //int pocZnakText = 0;
        //index koincidencie
        double IC;
        //pocet znakov v texte daneho pismena
        //int pocetZnakPismena[] = new int[26];
        
        
        String koincidencia = null;
        IC = indexKoincidencie(pocetRovnakychZnakovVTexte(sifra, 0), sifra.length());
        
        if(IC >= 0.03 && IC <= 0.05)
            koincidencia = "text je zasifrovany polyalfabetickou sifrou.";
        else if(IC >= 0.06)
            koincidencia = "text je zasifrovany monoalfabetickou sifrou a "
                            + "pravdepodobne ide o slovensky text";
        
        System.out.println("Index koincidencie je: " + IC + "\n" + koincidencia);
        System.out.println("");
        int pocetZnakPismena[];
        //dlzka hesla
        for (int i = 1; i <= maxDlzkaHesla; i++)
        {
            System.out.println("IC pri dlzke hesla " + i + " je: " + priemernyICzDlzkyKluca(sifra, i));
        }
        
        System.out.println("Chi-kvadrat test:");
        //sifra dlzky 4
        //vypocita chi-kvadrat iba pre jedno pismeno
        //cize ked heslo ma 4 znaky treba pustit tuto metodu 4-krat, za kazdym 
        //s inym posunom
        
        for (int i = 0; i < 4; i++)
        {
            chiKvadratTest(sifra, 4,i);
        }
        
    }

    static double indexKoincidencie(int pocetZnakPismena[],int pocZnakText)
    {
        //index koincidencie
        double IC = 0;
        //pocet pismen i
        int Fi;
        //celkovy pocet pismen v texte
        int N;
        
        //Vzorec Index koincidencie
        for(int pocetPismen:pocetZnakPismena)
        {
            Fi = pocetPismen;
            IC += Fi*(Fi-1);
        }
        N = pocZnakText;
        IC /= N*(N-1);
        
        return IC;
    }
    
    static int[] pocetRovnakychZnakovVTexte(String sifra, int dlzkaHesla)
    {
        //pocet znakov v texte daneho pismena
        int pocetZnakPismena[] = new int[VELKOSTPOLA];
        
        for (int i = 0; i < sifra.length(); i++)
        {
                    int tempPismeno = sifra.charAt(i + dlzkaHesla);
                    tempPismeno -= ABECEDA;//65;
                    int tempHodnota = pocetZnakPismena[tempPismeno];
                    tempHodnota++;
                    pocetZnakPismena[tempPismeno] = tempHodnota;
        }
        
        return pocetZnakPismena;
    }
    
    static double priemernyICzDlzkyKluca(String sifra, int dlzkaHesla)
    {
        double IC = 0;
        //pocet znakov v texte daneho pismena
        int pocetZnakPismena[] = new int[VELKOSTPOLA];
        int dlzkaSlova = 0;
        
        for (int i = 0; i < dlzkaHesla; i++)
        {
            for (int j = 0; j < sifra.length(); j += dlzkaHesla)
            {
                //aby som nepresiel za sifrovany text
                if((i + j) >= sifra.length())
                    break;
                
                int tempPismeno = sifra.charAt(i + j);
                tempPismeno -= ABECEDA;//65;
                int tempHodnota = pocetZnakPismena[tempPismeno];
                tempHodnota++;
                pocetZnakPismena[tempPismeno] = tempHodnota;
                dlzkaSlova++;
            }
            IC += indexKoincidencie(pocetZnakPismena, dlzkaSlova);
            dlzkaSlova = 0;
            pocetZnakPismena = new int[VELKOSTPOLA];
        }
        
        return IC/dlzkaHesla;
    }
    
    static void chiKvadratTest(String sifra, int dlzkaHesla,int posun)
    {
        //povodny text
        int Ti;
        //siforvany text
        int Ci;
        //heslo
        int Ki;
        //pocet pismen napr. A-cka
        int Ca;
        //ocakavany pocet pismen napr. A-cka(pravdepodobnostny)
        double Ea;
        String subString = "";
        int pocetZnakPismena[];
        double chi_kvad;
        double najnizsiChi_kvad = Double.MAX_VALUE;
        int tempPismeno = 0;
        
        for (int i = 0; i <= 25; i++)
        {
            System.out.print((char)(i + ABECEDA) + ": ");
            for (int j = posun; j <= sifra.length(); j += dlzkaHesla)
            {
                if(j >= sifra.length())
                    break;
                
                Ci = sifra.charAt(j);
                Ci -= ABECEDA;
                Ki = i;
                //vzorec na desifrovanie
                Ti = (Ci - Ki)%26;
                //toto riesi zaporne modulo
                    if(Ti < 0)
                        Ti += 26;
                    
                System.out.print((char)(Ti+ABECEDA) + " ");
                subString += (char)(Ti+ABECEDA);
            }
//            System.out.println("substring: " + subString);
            pocetZnakPismena = pocetRovnakychZnakovVTexte(subString, 0);
            
            Ca = pocetZnakPismena[i];
//            Ea = PRAVDVYSKYTU[i];
            Ea = PRAVDVYSKYTU[i];
//            //vzorec pre Chi-kvadrat test
            chi_kvad = (Math.pow((Ca - (Ea*subString.length())),2))/(Ea*subString.length());
            System.out.print("Chi-kvadrat: " + chi_kvad);
            System.out.println("");
            subString = "";
            
            if(chi_kvad < najnizsiChi_kvad)
            {
                najnizsiChi_kvad = chi_kvad;
                tempPismeno = i;
            }
        }
        
        System.out.println("Najnizsi chi-kvadrat je: " + najnizsiChi_kvad + " pre pismeno: " + (char)(tempPismeno + ABECEDA));
    }
}

    