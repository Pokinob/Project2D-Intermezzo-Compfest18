INCLUDE globals.ink
EXTERNAL claim(itemId)
-> main

===main===
~ ItemP1 = ItemP1 + 1
You Obtained Item
~ claim("ItemPuzzle")
->END