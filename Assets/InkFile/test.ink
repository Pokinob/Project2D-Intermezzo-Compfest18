EXTERNAL playDebug(Debug)
INCLUDE  globals.ink

{Pokemon == "" : -> main | -> already}

=== main ===
#showPortrait:show
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
#showPortrait:show
#speaker:Monbun #layout:right
Yoooo You already choose {Pokemon}
->END


