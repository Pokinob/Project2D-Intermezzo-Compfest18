EXTERNAL playDebug(Debug)
INCLUDE  Load_Global.ink

{Pokemon == "" : -> main | -> already}

=== main ===
#speaker:Monbun #layout:right
Hello
What pokemon do you want?
* [Trecko]
    ->test("Trecko")
* [Mudkip]
    ->test("Mudkip")
* [Torchic]
    ->test("Torchic")

=== test(poke) ===
~ Pokemon = poke
~ playDebug("Success")
#speaker:Ash #layout:left
I choose {poke}
->END

=== already ===
#speaker:Monbun #layout:right
Yoooo You already choose {Pokemon}
->END


