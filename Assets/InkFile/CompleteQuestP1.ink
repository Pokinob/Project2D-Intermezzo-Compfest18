INCLUDE globals.ink
EXTERNAL playQuest(IDQuest)

{P1Complete:
    ->Complete
  - else:
    {P1Start:
        ->OnQuest
    - else:
        ->StartQuest
    }
}


=== StartQuest ===
#speaker:???
#layout:left
~P1Start=true
...
Hmm...
I think I can put something here
But what?
~playQuest(Level)
->END
=== OnQuest ===
{ItemP1 == 3:
->FinishQuest
- else:
->NotFinish
}

=== FinishQuest ===
#speaker:???
#layout:left
~P1Complete = true
~Level = Level + 1
Finaly...
I hope this things work
->END

=== Complete ===
#speaker:???
#layout:left
I need to go now
Something happening
->END

=== NotFinish ===
#speaker:???
#layout:left
I need to find something, but where?
->END






