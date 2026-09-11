using System;
using System.Collections.Generic;

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
1.15,
2.32,
3.89
};


Console.WriteLine("Ange vilken produkt du vill köpa");
for (int i = 0 < produkter.Count; i++)
{
    Console.WriteLine($"{i + 1}. {produkter[i]} - {priser[i]} kr");
}
