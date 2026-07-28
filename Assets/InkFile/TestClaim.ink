INCLUDE globals.ink
EXTERNAL claim(itemId)
-> main

===main===
~ ItemP1 = ItemP1 + 1
#layout:hide
You Obtained Item
~ claim("ItemPuzzle")
->END