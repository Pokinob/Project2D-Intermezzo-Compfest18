INCLUDE globals.ink

{P1Complete:
    ->Complete
  - else:
    {ItemP1 == 3:
    ->FinishQuest
    - else:
    ->NotFinish
    }
}

=== Complete ===
Hey i already do this
->END

=== FinishQuest ===
Finaly...
I hope this things work
~P1Complete = true
->END

=== NotFinish ===
I need to complete this, but how?
->END






