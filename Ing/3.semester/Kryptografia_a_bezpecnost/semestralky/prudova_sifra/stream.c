#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#define MAX_TEXT_SIZE 10000

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

typedef enum {
	MODE_ENCRYPT = 0,
	MODE_DECRYPT = 1
} CipherMode;

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

void encrypt(long long key, const char *plainText, char *cipherText, size_t count)
{
	// nastav random seed - toto je hodnota kluca
	my_seed(key); 

	int i;
	for (i = 0; i < count; i++)
	{
		int ch = toupper(plainText[i]);
		if (ch >= 'A' && ch <= 'Z')
		{
			int p = ch - 'A';
			int k = (int) (26 * my_rand());
			int c = (p + k) % 26;
			cipherText[i] = 'A' + c;
		}
		else
		{
			cipherText[i] = ch;
		}
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

int main(int argc, char * argv[])
{
	long long seed = 0;
	CipherMode mode = MODE_ENCRYPT;
	const char * inputFilename = "input.txt";
	const char * outputFilename = "output.txt";

	int i = 1;
	while (i < argc)
	{
		char *param = argv[i++];
		if (strcmp(param, "-s") == 0)
		{
			if (i < argc) seed = atoll(argv[i++]);
		}
		else if (strcmp(param, "-e") == 0)
		{
			mode = MODE_ENCRYPT;
		}
		else if (strcmp(param, "-d") == 0)
		{
			mode = MODE_DECRYPT;
		}
		else if (strcmp(param, "-in") == 0)
		{
			if (i < argc) inputFilename = argv[i++];
		}
		else if (strcmp(param, "-out") == 0)
		{
			if (i < argc) outputFilename = argv[i++];
		}
	}

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
	
	if (mode == MODE_ENCRYPT) encrypt(seed, inputText, outputText, count);
	else if (mode == MODE_DECRYPT) decrypt(seed, inputText, outputText, count);

	writeFile(outputFilename, outputText, count);

	free(inputText);
	free(outputText);

	return 0;
}

