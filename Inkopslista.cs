//Using System hjälper till att koden inte behöver specificera System i Console inmatning/utskrivning.
//Exempel System.Console.WriteLine blir Console.WriteLine.
using System;

//Denna raden hjälper till att lagra i listor.
using System.Collections.Generic;
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

//Loopar så länge det finns något kvar i lagret.
while (!lager.All(antal => antal == 0))
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

    Console.Write("Ange produktnamn eller menynummer (eller tryck Enter för att avsluta): ");
    string? val = Console.ReadLine();

    //Avslutar köpet om användaren inte anger något.
    if (string.IsNullOrWhiteSpace(val))
    {
        break;
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

