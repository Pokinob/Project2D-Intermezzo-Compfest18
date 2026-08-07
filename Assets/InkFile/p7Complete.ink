INCLUDE globals.ink
EXTERNAL playQuest(IDQuest)

{P7Complete:
    ->Complete
  -else:
    {P7Start:
        ->OnQuest
    -else:
        ->StartQuest
    }
}


=== StartQuest ===
#speaker:???
#layout:left
~P7Start=true
What is this?
Wait.
It say something

#layout:hide
"Believe what you see"
"And follow what you see or what you experience…"
~playQuest(7)
...
#speaker:???
#layout:left
Wait, what just happened?
Hmm
I think the words on this stone tablet might help me
->END

=== OnQuest ===
#layout:hide
"Believe what you see"
"And follow what you see or what you experience…"
...
->END

=== Complete ===
#layout:hide
"Believe what you see"
"And follow what you see or what you experience…"
->END







