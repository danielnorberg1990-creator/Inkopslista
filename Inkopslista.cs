//Using System hjälper till så att koden inte behöver specificera System i Console inmatning/utskrivning.
//Exempel: System.Console.WriteLine blir Console.WriteLine.
using System;

//Denna rad hjälper till att lagra saker i listor.
using System.Collections.Generic;

//Denna rad hjälper till att använda metoder som .Sum() på listorna.
using System.Linq;

//Denna rad hjälper till att läsa och skriva till filer.
using System.IO;
//**************************************************************
//Inköpslistans varunamn. Listan startar tom och fylls på
//antingen från filen eller när användaren lägger till varor.
List<string> inkopslista = new List<string>();

//Inköpslistans priser (i hela kronor). Hålls i synk med listan ovan:
//samma index i båda listorna betyder samma vara.
//Int används så att priset bara kan vara ett heltal, t.ex. 25.
List<int> priser = new List<int>();

//Namnet på filen där inköpslistan sparas.
string filNamn = "inkopslista.txt";

//Tolkar en pristext till ett heltal, t.ex. '25'.
//Returnerar true om texten är ett giltigt heltal, annars false.
bool TolkaPris(string? text, out int pris)
{
    //int.TryParse returnerar false för tom text och text som inte är ett heltal.
    return int.TryParse(text, out pris);
}

//Kollar om en text är ett giltigt pris: ett icke-negativt heltal,
//t.ex. 25. Sätter priset i 'pris' om det är giltigt.
bool ErtGiltigtPris(string? text, out int pris)
{
    //Först: är texten ett heltal alls?
    if (!TolkaPris(text, out pris))
    {
        return false;
    }

    //Sedan: priset får inte vara negativt.
    return pris >= 0;
}

//Kollar om en text innehåller bokstäver, t.ex. 'a', 'B' eller 'ö'.
bool InnehållerBokstäver(string text)
{
    foreach (char tecken in text)
    {
        if (char.IsLetter(tecken))
        {
            return true;
        }
    }

    return false;
}

//Kollar om en text innehåller specialtecken, t.ex. ',', '.' eller '#'.
//Minus tillåts också, så att ett negativt pris får sitt eget felmeddelande.
bool InnehållerSpecialtecken(string text)
{
    foreach (char tecken in text)
    {
        if (!char.IsDigit(tecken) && tecken != '-')
        {
            return true;
        }
    }

    return false;
}

//Läser in varorna från filen om den finns (t.ex. från en tidigare körning).
//Om filen inte finns, till exempel vid allra första start, startar listan tom.
if (File.Exists(filNamn))
{
    foreach (string rad in File.ReadAllLines(filNamn))
    {
        //Hoppa över tomma rader i filen.
        if (string.IsNullOrWhiteSpace(rad))
        {
            continue;
        }

        //Varje rad i filen ser ut så här: Namn;Pris
        string[] delar = rad.Split(';');

        //Om raden har både namn och giltigt pris, lägg till varan.
        //Trasiga rader hoppas över, så kraschar programmet inte.
        if (delar.Length == 2 && ErtGiltigtPris(delar[1], out int lagratPris))
        {
            inkopslista.Add(delar[0]);
            priser.Add(lagratPris);
        }
    }
}

//Sparar hela listan i filen, så finns varorna kvar nästa gång programmet körs.
void SparaListan()
{
    List<string> rader = new List<string>();

    //Bygg en rad per vara: Namn;Pris
    for (int i = 0; i < inkopslista.Count; i++)
    {
        rader.Add($"{inkopslista[i]};{priser[i]}");
    }

    //Skriv alla rader till filen (fileras över om den redan finns).
    File.WriteAllLines(filNamn, rader);
}

//**************************************************************
//Loopar så länge programmet inte har avslutats, och visar i varje varv
//listan som en numrerad lista med totalsumma och sedan en meny.
bool avsluta = false;
while (!avsluta)
{
    //Visa inköpslistan, numrerad, med pris för varje vara.
    Console.WriteLine("=== Din inköpslista ===");
    if (inkopslista.Count == 0)
    {
        Console.WriteLine("(listan är tom)");
    }
    else
    {
        for (int i = 0; i < inkopslista.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {inkopslista[i]} - {priser[i]} kr");
        }
    }

    //Räkna ihop totalsumman för alla varor på listan.
    Console.WriteLine($"Totalsumma: {priser.Sum()} kr");

    //Visa menyn.
    Console.WriteLine();
    Console.WriteLine("=== Meny ===");
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Avsluta programmet");
    Console.Write("Välj: ");
    string? val = Console.ReadLine();

    //Felhantering: utan ett menyval kan inget göras.
    if (string.IsNullOrWhiteSpace(val))
    {
        Console.WriteLine("Inget menyval angavs, välj mellan 1 och 4.");
        continue;
    }

    //Felhantering: ett val som inte är ett tal eller som inte finns i menyn.
    if (!int.TryParse(val, out int menyVal) || menyVal < 1 || menyVal > 4)
    {
        Console.WriteLine($"'{val}' är inte ett giltigt menyval, välj mellan 1 och 4.");
        continue;
    }

    switch (menyVal)
    {
        case 1: //Lägg till en ny vara (namn + pris) sist i listan.
            Console.Write("Varans namn: ");
            string? namn = Console.ReadLine();

            //Felhantering: utan ett namn kan inget läggas till.
            if (string.IsNullOrWhiteSpace(namn))
            {
                Console.WriteLine("Inget namn angavs, du behöver ange varans namn.");
                break;
            }

            Console.Write("Ange varans pris i hela kronor (t.ex. 25): ");
            string? prisText = Console.ReadLine();

            //Felhantering: utan ett pris kan inget läggas till.
            if (string.IsNullOrWhiteSpace(prisText))
            {
                Console.WriteLine("Inget pris angavs, inget blev tillagt.");
                break;
            }

            //Felhantering: bokstäver i priset, t.ex. 'abc' eller '10kr'.
            if (InnehållerBokstäver(prisText))
            {
                Console.WriteLine($"'{prisText}' innehåller bokstäver, priset får bara innehålla siffror i heltal, varan blev tillagt.");
                break;
            }

            //Felhantering: decimalavdelare i priset, t.ex. '12,50' eller '12.50'.
            //Priset måste vara ett heltal, så både komma och punkt får ett eget meddelande.
            if (prisText.Contains(',') || prisText.Contains('.'))
            {
                Console.WriteLine("Du behöver skriva ett heltal utan decimaler, försök igen");
                break;
            }

            //Felhantering: specialtecken i priset, t.ex. '10#5' eller '10;5'.
            //Endast siffror är tillåtna (samt - för negativt pris).
            if (InnehållerSpecialtecken(prisText))
            {
                Console.WriteLine($"'{prisText}' innehåller specialtecken, priset får bara innehålla siffror, inget blev tillagt.");
                break;
            }

            //Felhantering: texten som helhet är inte ett heltal, t.ex. '-'.
            if (!TolkaPris(prisText, out int pris))
            {
                Console.WriteLine($"'{prisText}' är inte ett giltigt pris, inget blev tillagt.");
                break;
            }

            //Felhantering: ett negativt pris accepteras inte.
            if (pris < 0)
            {
                Console.WriteLine($"'{prisText}' är ett negativt pris, priset får inte vara negativt, inget blev tillagt.");
                break;
            }

            //Lägg till både namn och pris sist i sina respektive listor,
            //så hålls de i synk.
            inkopslista.Add(namn);
            priser.Add(pris);

            //Spara listan i filen, så finns varan kvar nästa gång programmet körs.
            SparaListan();

            Console.WriteLine($"{namn} ({pris} kr) har lagts till i listan.");
            break;

        case 2: //Ta bort en vara genom att ange dess nummer.
            if (inkopslista.Count == 0)
            {
                Console.WriteLine("Listan är tom - ingen vara att ta bort.");
                break;
            }

            Console.Write($"Vilket nummer ska tas bort? (Välj mellan 1 och {inkopslista.Count}): ");
            string? bortNummer = Console.ReadLine();

            //Felhantering: om valet inte är ett tal eller tomt får användaren
            //ett felmeddelande istället för en krasch.
            if (string.IsNullOrWhiteSpace(bortNummer) || !int.TryParse(bortNummer, out int nummer))
            {
                Console.WriteLine($"'{bortNummer}' är inte ett giltigt nummer, inget blev borttaget.");
                break;
            }

            //Felhantering: ett nummer som inte finns i listan.
            if (nummer < 1 || nummer > inkopslista.Count)
            {
                Console.WriteLine($"Numret {nummer} finns inte i listan. Välj mellan 1 och {inkopslista.Count}.");
                break;
            }

            //Menynumret 1 är index 0 i listorna.
            int index = nummer - 1;

            //Kom ihåg vilket namn som togs bort, för att kunna meddela det.
            string bortNamn = inkopslista[index];

            //Ta bort varan från BÅDA listorna på samma index, så hålls de i synk.
            inkopslista.RemoveAt(index);
            priser.RemoveAt(index);

            //Spara listan i filen, så sparas även borttagningen.
            SparaListan();

            Console.WriteLine($"{bortNamn} har tagits bort från listan.");
            break;

        case 3: //Spara listan manuellt till filen.
            SparaListan();
            Console.WriteLine($"Listan har sparats i {filNamn}.");
            break;

        case 4: //Avsluta programmet.
            Console.WriteLine("Hej då!");
            avsluta = true;
            break;
    }
}