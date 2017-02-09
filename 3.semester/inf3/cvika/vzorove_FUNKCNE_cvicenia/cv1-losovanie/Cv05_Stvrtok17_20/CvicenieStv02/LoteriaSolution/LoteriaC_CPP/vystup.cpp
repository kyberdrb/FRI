#include <stdio.h>

#include "vystup.h"
#include "data.h"

void vypis(int pocetVyzrebovanych)
{
	for (int i = 0; i < pocetVyzrebovanych && i < POCET_ZREBOV; i++)
	{
		printf("%2d. miesto: %2d\t%s\n", i + 1, zreby[i].cislo, zreby[i].majitel);
	}
}