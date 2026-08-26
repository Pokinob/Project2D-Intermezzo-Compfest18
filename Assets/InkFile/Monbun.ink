INCLUDE globals.ink
EXTERNAL OpenGate(level)

->Main

===Main===
Are you ready to go?
* [Yes]
->Yes
* [Wait]
->No


===Yes===
Nice
I like your courage
Wait a minute, I'll open it for you
~OpenGate = true
~OpenGate(Level)
->END

===No===
It's okay, i can wait
->END
