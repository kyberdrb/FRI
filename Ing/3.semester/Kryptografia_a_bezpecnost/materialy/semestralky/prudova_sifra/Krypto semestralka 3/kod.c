#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#define MAX_TEXT_SIZE 100000

/* generator pseudonahodnych cisel */

long long my_randx;

void my_seed(long long s)
/* pociatocna inicializacia generatora */
{
  my_randx = s;
}

double my_rand(void)
/* generuje nahodne cislo v intervale <0,1) */
{
  /* prechod do dalsieho stavu generatora (LL na konci cisla znamena typ long long)*/
  my_randx = (84589LL * my_randx + 45989LL) % 217728LL;
  /* vypocet navratovej hodnoty */
  return (double) my_randx / 217728.0;
}

/* koniec generatora pseudonahodnych cisel */

size_t readFile(const char * name, char * memory, size_t max)
{
	size_t count = 0;
	FILE * stream = fopen(name, "rb");
	if (stream == NULL)
	{
		fprintf(stderr, "Nepodarilo sa nacitat subor '%s'!\n", name);
	}
	else
	{
		count = fread(memory, sizeof(char), max, stream);
		fclose(stream);
	}

	return count;
}

void writeFile(const char * name, char * memory, int count)
{
	FILE * stream = fopen(name, "wb");
	if (stream == NULL)
	{
		fprintf(stderr, "Nepodarilo sa zapisat subor '%s'!\n", name);
	}
	else
	{
		fwrite(memory, sizeof(char), count, stream);
		fclose(stream);
	}
}

void decrypt(long long key, const char *cipherText, char *plainText, size_t count)
{
	// nastav random seed - toto je hodnota kluca
	my_seed(key); 

	int i;
	for (i = 0; i < count; i++)
	{
		int ch = toupper(cipherText[i]);
		if (ch >= 'A' && ch <= 'Z')
		{
			int c = ch - 'A';
			int k = (int) (26 * my_rand());
			int p = (c + (26 - k)) % 26;
			plainText[i] = 'A' + p;
		}
		else
		{
			plainText[i] = ch;
		}
	}
}

float vIndexKoincidencie(char * text, size_t count)
{
    float index;
    int pocet = 26;
    float pa[pocet];
    int celkPocet;
    
    for(int i = 0;i<pocet;i++)
    {
        pa[i]=0;
    }
    
    celkPocet = 0;
    for(int i = 0; i < count; i++)
    {
        if (text[i] >= 'A' && text[i] <= 'Z')
        {
        int c = text[i] - 'A';
        //printf("%d ", c);
        pa[c]++;
        celkPocet++;
        }
    }
    for(int i = 0;i<pocet;i++)
    {
        pa[i] = pa[i]/celkPocet;
    }
    index = 0;
    for(int i = 0;i<pocet;i++)
    {
        index += (pa[i]*pa[i]);
    }
    
    for(int i = 0;i<pocet;i++)
    {
        printf("%f\n", pa[i]);
    }
    printf("%f\n", index);
    
    return index;
}


int main(int argc, char * argv[])
{
	long long seed = 0;
	const char * inputFilename = "C:/Hry/krypto/text4.txt";
	const char * outputFilename = "C:/Hry/krypto/output.txt";
	float indexKoincidencieSJ =  0.06027; //index koencidencie SJ
        float indexKoincidencie = 0;
        
        
	char *inputText = (char *) malloc(MAX_TEXT_SIZE);
	char *outputText = (char *) malloc(MAX_TEXT_SIZE);
	
        if (inputText == NULL || outputText == NULL)
	{
		fprintf(stderr, "Nepodarilo sa alokovat pamat na text!\n");
		return -1;
	}

	size_t count = readFile(inputFilename, inputText, MAX_TEXT_SIZE);
	if (count == 0)
	{
		fprintf(stderr, "Vstupny text je prazdny!\n");
		return -1;
	}
	
        seed = 0; //treba menit
        while(indexKoincidencie < indexKoincidencieSJ)
        {
            indexKoincidencie = 0;
            decrypt(seed, inputText, outputText, count);
            indexKoincidencie = vIndexKoincidencie(outputText, count);
            seed++;
        }
	
        for(int i = 0;i<100;i++)
        {
            printf("%c", outputText[i]);
        }
        
        
	writeFile(outputFilename, outputText, count);
        
	free(inputText);
	free(outputText);
        seed--;
        printf("\n");
        printf("%lld\n", seed);
        
	return 0;
}
