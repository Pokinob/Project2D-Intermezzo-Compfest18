INCLUDE globals.ink
EXTERNAL GetItem()
EXTERNAL DelayClaim()
-> main

VAR ItemName = ""
VAR ItemType = ""
VAR ItemLevel = ""

===main===
~ItemName = GetItem()
~DelayClaim()
You Obtained {ItemName} #layout:hide
->END