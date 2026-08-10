INCLUDE globals.ink
EXTERNAL inputName()
{MCName=="???":->main|->Complete}
->main

===main===
#speaker:???
#layout:left
hello
#speaker:Monbun
#layout:right
hi, whats your name
~inputName()
...
#speaker:???
#layout:left
halo namaku {MCName}
#speaker:Monbun
#layout:right
oh hi {MCName}
->END

===Complete===
#speaker:Monbun#layout:right
hi {MCName}
->END