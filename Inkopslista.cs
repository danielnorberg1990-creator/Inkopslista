//Using System hjälper till att koden inte behöver specificera System i Console inmatning/utskrivning.
//Exempel System.Console.WriteLine blir Console.WriteLine.
using System;

//Denna raden hjälper till att lagra i listor.
using System.Collections.Generic;

//Denna raden hjälper till att använda metoder som .All() och .Sum() på listorna.
using System.Linq;
//**************************************************************
//Lista med produkter.
List<string> produkter = new List<string>
{
"Mjölk",
"Bröd",
"Ost"
};

//Lista med priser (i kronor).
List<int> priser = new List<int>
{
    15,
    32,
    89
};

//Lista med lagerstatus (antal st i lager per produkt).
List<int> lager = new List<int>
{
    10,
    5,
    3
};

//Varukorgen: vilken produkt -> antal st som köpts.
Dictionary<string, int> varukorg = new Dictionary<string, int>();

//Räknar ihop priset för allt som köpts.
int totalPris = 0;

//**************************************************************
//Loopar så länge det finns något kvar i lagret eller något i varukorgen.
while (!lager.All(antal => antal == 0) || varukorg.Count > 0)
{
    //Visar alla produkter med pris och lagerstatus.
    Console.WriteLine("Våra produkter:");
    for (int i = 0; i < produkter.Count; i++)
    {
        if (lager[i] > 0)
        {
            Console.WriteLine($"{i + 1}. {produkter[i]} - {priser[i]} kr ({lager[i]} st i lager)");
        }
        else
        {
                        Console.WriteLine($"{i + 1}. {produkter[i]} - {priser[i]} kr (SLUT)");
        }
    }

    //Visar vad som ligger i varukorgen just nu.
    if (varukorg.Count > 0)
    {
        Console.WriteLine("Din varukorg:");
        foreach (var artikel in varukorg)
        {
            Console.WriteLine($"  {artikel.Key}: {artikel.Value} st");
        }
    }








        Console.Write("Ange produktnamn, menynummer eller 'borttag' (Enter = avsluta): ");
    string? val = Console.ReadLine();

    //Avslutar köpet om användaren inte anger något.
    if (string.IsNullOrWhiteSpace(val))
    {
        break;
    }

    //Kommandot "borttag" tar en vara ur varukorgen och lägger tillbaka i lagret.
    if (val.Equals("borttag", StringComparison.OrdinalIgnoreCase))
    {
        if (varukorg.Count == 0)
        {
            Console.WriteLine("Varukorgen är tom - inget att ta bort.");
            continue;
        }

        Console.WriteLine("Vilken produkt vill du ta bort? (namn eller nummer)");
        Console.Write("Ange produkten: ");
        string? bortVal = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(bortVal))
        {
            continue; //Gå tillbaka till huvudmenyn.
        }

                //Sök upp produkten som ska tas bort (nummer eller namn).
        int bortIndex = -1;
        if (int.TryParse(bortVal, out int bortMenyVal) && bortMenyVal >= 1 && bortMenyVal <= produkter.Count)
        {
            bortIndex = bortMenyVal - 1;
        }
        else
        {
            bortIndex = produkter.FindIndex(p => p.Equals(bortVal, StringComparison.OrdinalIgnoreCase));
        }

                //Kontrollera att produkten finns och att den finns i varukorgen.
        if (bortIndex != -1)
        {
            string bortProdukt = produkter[bortIndex];
            if (varukorg.ContainsKey(bortProdukt))
            {
                //Ta bort en st från varukorgen (eller radera raden helt om det var den enda).
                if (varukorg[bortProdukt] > 1)
                {
                    varukorg[bortProdukt]--;
                }
                else
                {
                    varukorg.Remove(bortProdukt);
                }

                //Lägg tillbaka i lagret och dra bort priset från totalen.
                lager[bortIndex]++;
                totalPris -= priser[bortIndex];

                Console.WriteLine($"{bortProdukt} har tagits bort från varukorgen och lagts tillbaka i lagret. ({lager[bortIndex]} st i lager)");
            }
            else
            {
                Console.WriteLine($"Du har inte {bortProdukt} i din varukorg.");
            }
        }
        else
        {
            Console.WriteLine($"Produkten '{bortVal}' hittades inte.");
        }
        continue; //Gå tillbaka till huvudmenyn.
    }

        //Felhantering: försök först att tolka valet som menynummer, annars som produktnamn.
    int index = -1;
    if (int.TryParse(val, out int menyVal))
    {
        if (menyVal >= 1 && menyVal <= produkter.Count)
        {
            //Användaren valde via meny, t.ex. "1" blir index 0.
            index = menyVal - 1;
        }
        else
        {
            Console.WriteLine($"Ogiltigt val! Välj en siffra mellan 1 och {produkter.Count}.");
            continue; //Gå tillbaka till nästa varv i loopen.
        }
    }
    else
    {
        //Inte en siffra, så sök upp produkten i listan (oavsett versaler/gesmaler).
        index = produkter.FindIndex(p => p.Equals(val, StringComparison.OrdinalIgnoreCase));
        if (index == -1)
        {
            Console.WriteLine($"Produkten '{val}' fanns inte på listan.");
            continue; //Gå tillbaka till nästa varv i loopen.
        }
    }

        //if sats där programmet räknar ut om produkten finns i lager eller inte.
    if (lager[index] > 0)
    {
        //Lägg till priset i totalen och minska antalet i lager.
        totalPris += priser[index];
        lager[index]--;

        //Lägg produkten i varukorgen (ökar antalet om den redan finns).
        if (varukorg.ContainsKey(produkter[index]))
        {
            varukorg[produkter[index]]++;
        }
        else
        {
            varukorg.Add(produkter[index], 1);
        }

        Console.WriteLine($"{produkter[index]} köpt för {priser[index]} kr! ({lager[index]} st kvar)");
    }
    else
    {
        Console.WriteLine("Tyvärr är produkten slut i lagret.");
    }
}

//Kvitto: visar varukorgen och vad som betalades i totalt.
Console.WriteLine();
Console.WriteLine("=== Ditt kvitto ===");
if (varukorg.Count > 0)
{
    foreach (var artikel in varukorg)
    {
        //Sök upp priset för artikeln.
        int prisIndex = produkter.IndexOf(artikel.Key);
        Console.WriteLine($"{artikel.Value} st {artikel.Key} - {artikel.Value * priser[prisIndex]} kr");
    }
}
else
{
    Console.WriteLine("Inga produkter köpta.");
}
Console.WriteLine($"Totalt antal artiklar: {varukorg.Values.Sum()} st");
Console.WriteLine($"Totalt betalade du {totalPris} kr.");
Console.WriteLine("Tack för ditt köp!");

