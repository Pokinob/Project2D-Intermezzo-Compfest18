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
#showPortrait:show
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
#showPortrait:show
~P1Complete = true
Finaly...
I hope this things work
~Level = Level + 1
->END

=== Complete ===
#speaker:???
#layout:left
#showPortrait:show
I need to go now
Something happening
->END

=== NotFinish ===
#showPortrait:show
#speaker:???
#layout:left
I need to find something, but where?
->END






